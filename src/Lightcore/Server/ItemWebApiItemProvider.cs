using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;

namespace Lightcore.Server
{
    public class ItemWebApiItemProvider : IItemProvider, IDisposable
    {
        private readonly HttpClient _client;
        private readonly LightcoreOptions _options;

        public ItemWebApiItemProvider(LightcoreOptions options)
        {
            _options = options;
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public async Task<Item> GetItemAsync(string pathOrId, Language language)
        {
            var getWatch = Stopwatch.StartNew();
            var url = string.Format("{0}/-/item/v1{1}?sc_database=web&language={2}&payload=full&fields=Title|Text|__Renderings&scope=s|c", _options.ServerUrl, pathOrId, language.Name);
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                getWatch.Stop();

                var parseWatch = Stopwatch.StartNew();
                var sitecoreApiReponse = JsonConvert.DeserializeObject<SitecoreApiReponse>(content);

                if (sitecoreApiReponse.StatusCode != 200 || sitecoreApiReponse.Result.ResultCount <= 0)
                {
                    return null;
                }

                var sitecoreApiItem = sitecoreApiReponse.Result.Items.First();

                // Map item
                var item = Map(sitecoreApiItem);

                // Map item children
                var sitecoreApiItems = sitecoreApiReponse.Result.Items.Skip(1);
                var items = new List<Item>(sitecoreApiReponse.Result.Items.Length - 1);

                items.AddRange(sitecoreApiItems.Select(Map));

                item.Children = items;

                parseWatch.Stop();

                item.Trace = $"Loaded in {getWatch.ElapsedMilliseconds} ms, mapped in {parseWatch.ElapsedMilliseconds} ms";

                return item;
            }

            return null;
        }

        private Item Map(SitecoreApiItem sitecoreApiItem)
        {
            var item = new Item
            {
                Id = sitecoreApiItem.Id,
                Key = sitecoreApiItem.Name.ToLowerInvariant(),
                Name = sitecoreApiItem.Name,
                Path = sitecoreApiItem.Path,
                Language = new Language(sitecoreApiItem.Language),
                Visualization = new ItemVisualization
                {
                    Layout = new Layout
                    {
                        Path = "/Views/Layout.cshtml"
                    },
                    Renderings = new List<Rendering>
                    {
                        new Rendering("content", "Menu"),
                        new Rendering("content", "Article"),
                        new Rendering("footer", "Footer")
                    }
                },
                Fields = new FieldCollection(),
                Children = Enumerable.Empty<Item>()
            };

            var sitecoreApiFields = sitecoreApiItem.Fields;
            var fields = new List<Field>(sitecoreApiFields.Count);

            foreach (var keyValuePair in sitecoreApiFields)
            {
                fields.Add(new Field
                {
                    Key = keyValuePair.Value.Name,
                    Value = keyValuePair.Value.Value,
                    Type = keyValuePair.Value.Type
                });
            }

            item.Fields = new FieldCollection(fields);

            return item;
        }

        public class SitecoreApiReponse
        {
            public int StatusCode { get; set; }
            public SitecoreApiResult Result { get; set; }
        }

        public class SitecoreApiResult
        {
            public int ResultCount { get; set; }
            public int TotalCount { get; set; }
            public SitecoreApiItem[] Items { get; set; }
        }

        public class SitecoreApiItem
        {
            public string Name { get; set; }
            public string Language { get; set; }
            public string Path { get; set; }
            public Guid Id { get; set; }
            public Dictionary<string, SitecoreApiField> Fields { get; set; }
        }

        public class SitecoreApiField
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
        }
    }
}