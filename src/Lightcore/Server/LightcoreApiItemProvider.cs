using System;
using System.IO;
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
        private readonly LightcoreOptions _config;
        private readonly JsonSerializer _serializer;

        public LightcoreApiItemProvider(IOptions<LightcoreOptions> options, IMemoryCache cache)
        {
            _serializer = new JsonSerializer();
            _cache = cache;
            _config = options.Value;
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
            Requires.IsNotNullOrEmpty(pathOrId, nameof(pathOrId));
            Requires.IsNotNull(language, nameof(language));

            var device = _config.Sitecore.Device;
            var database = _config.Sitecore.Database;
            var cdn= _config.Sitecore.Cdn;
            var url = $"{_config.ServerUrl}/-/lightcore/item/{pathOrId}?sc_database={database}&sc_lang={language.Name}&sc_device={device}&cdn={cdn}";

            using (var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            using (JsonReader jsonReader = new JsonTextReader(streamReader))
                            {
                                var apiResponse = _serializer.Deserialize<ServerResponseModel>(jsonReader);

                                return ModelToItemMapper.Map(apiResponse, language);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}