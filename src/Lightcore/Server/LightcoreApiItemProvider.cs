using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Server.Models;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;

namespace Lightcore.Server
{
    public class LightcoreApiItemProvider : IItemProvider, IDisposable
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _client;
        private readonly LightcoreConfig _config;

        public LightcoreApiItemProvider(IOptions<LightcoreConfig> config, IMemoryCache cache)
        {
            _cache = cache;
            _config = config.Options;
            _client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
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
                query = "/sitecore/content/Home";
            }
            else if (pathOrId.Contains("/"))
            {
                query = "/sitecore/content/Home" + pathOrId.ToLowerInvariant().Replace("/sitecore/content/home", "/");
            }
            else
            {
                query = pathOrId;
            }

            var getWatch = Stopwatch.StartNew();
            var url = _config.ServerUrl + "/-/lightcore/item/" + query + "?sc_database=web&sc_lang=" + language.Name + "&sc_device=default";
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var length = response.Content.Headers.ContentLength;
                var content = await response.Content.ReadAsStringAsync();

                getWatch.Stop();

                var parseWatch = Stopwatch.StartNew();
                var apiResponse = JsonConvert.DeserializeObject<ServerResponseModel>(content);
                var item = Map(apiResponse.Item, apiResponse.Fields, apiResponse.Presentation, language);

                item.Children = apiResponse.Children.Select(child => Map(child.Item, child.Fields, child.Presentation, language));

                parseWatch.Stop();

                item.Trace = $"Loaded {length} bytes in {getWatch.ElapsedMilliseconds} ms, mapped in {parseWatch.ElapsedMilliseconds} ms";

                return item;
            }

            return null;
        }

        private Item Map(ItemModel apiItem, IEnumerable<FieldModel> apiFields, PresentationModel apiPresentation, Language language)
        {
            var item = new Item
            {
                Language = language,
                Id = apiItem.Id,
                Key = apiItem.Name.ToLowerInvariant(),
                Name = apiItem.Name,
                Path = apiItem.FullPath,
                Url = "/" + language.Name.ToLowerInvariant() + apiItem.FullPath.ToLowerInvariant().Replace("/sitecore/content/home", ""),
                Fields = new FieldCollection(apiFields.Select(f => new Field
                {
                    Key = f.Key,
                    Type = f.Type,
                    Value = f.Value,
                    Id = f.Id
                }))
            };

            if (apiPresentation != null)
            {
                item.Visualization = new ItemVisualization
                {
                    Layout = new Layout
                    {
                        Path = apiPresentation.Layout.Path
                    },
                    Renderings = apiPresentation.Renderings.Select(r => new Rendering(r.Placeholder, r.Controller, r.Action))
                };
            }

            return item;
        }
    }
}