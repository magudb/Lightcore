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
        private readonly LightcoreConfig _config;

        public ItemWebApiItemProvider(IOptions<LightcoreConfig> config)
        {
            _config = config.Options;
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public async Task<Item> GetItemAsync(string pathOrId, Language language)
        {
            string query;

            if (pathOrId.Equals("/") || string.IsNullOrEmpty(pathOrId))
            {
                query = "/sitecore/content/Home" + "?sc_database=web";
            }
            else if (pathOrId.Contains("/"))
            {
                query = pathOrId + "?sc_database=web";
            }
            else
            {
                query = "/?sc_itemid=" + pathOrId + "&sc_database=web";
            }

            var getWatch = Stopwatch.StartNew();
            var url = _config.ServerUrl + "/-/item/v1" + query + "&language=" + language.Name + "&payload=full&fields=Title|Text|__Renderings&scope=s|c";
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
                Url = "/" + sitecoreApiItem.Language.ToLowerInvariant() + sitecoreApiItem.Path.ToLowerInvariant().Replace("/sitecore/content/home", ""),
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
                Fields = Enumerable.Empty<Field>(),
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

            item.Fields = fields;

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