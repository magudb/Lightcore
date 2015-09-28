using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Framework.Caching.Memory;
using Newtonsoft.Json;

namespace Lightcore.Kernel.Data
{
    public class ItemProvider : IItemProvider, IDisposable
    {
        private readonly MemoryCache _cache;
        private readonly HttpClient _client;

        public ItemProvider()
        {
            _client = new HttpClient();

            // TODO: Cache implementation should be injected

            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public async Task<Item> GetItem(string pathOrId, Language language)
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

            var watch = Stopwatch.StartNew();

            // TODO: How to handle fields... can't combine payload=content + __renderings field
            var url = "http://sc80-150812.ad.codehouse.com/-/item/v1" + query +
                      "&language=" + language.Name +
                      "&payload=full&fields=Title|Text|__Renderings&scope=s|c";

            Item item;

            if (_cache.TryGetValue(url, out item))
            {
                item.Trace = "Loaded from cache...";

                return item;
            }

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var sitecoreApiReponse = JsonConvert.DeserializeObject<SitecoreApiReponse>(content);

                watch.Stop();

                if (sitecoreApiReponse.StatusCode != 200 || sitecoreApiReponse.Result.ResultCount <= 0)
                {
                    // Also cache not found items
                    _cache.Set(url, null, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(30)
                    });

                    return null;
                }

                var sitecoreApiItem = sitecoreApiReponse.Result.Items.First();

                // Map item
                item = Map(sitecoreApiItem);

                // Map item children
                var sitecoreApiItems = sitecoreApiReponse.Result.Items.Skip(1);
                var items = new List<Item>(sitecoreApiReponse.Result.Items.Length - 1);

                items.AddRange(sitecoreApiItems.Select(Map));
                item.Children = items;

                item.Trace = $"Load took {watch.ElapsedMilliseconds} ms";

                //_cache.Set(url, item, new MemoryCacheEntryOptions
                //{
                //    AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10)
                //});

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

                // TODO: Some url provider...
                Url = "/" + sitecoreApiItem.Language.ToLowerInvariant() + sitecoreApiItem.Path.ToLowerInvariant().Replace("/sitecore/content/home", ""),
                Path = sitecoreApiItem.Path,
                Language = new Language(sitecoreApiItem.Language),
                
                // TODO: Parse __Renderings for reals
                Layout = "/Views/Layout.cshtml",
                Renderings = new List<Rendering>
                {
                    new Rendering("content", "Menu"),
                    new Rendering("content", "Article"),
                    new Rendering("footer", "Footer")
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
                    Name = keyValuePair.Value.Name,
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