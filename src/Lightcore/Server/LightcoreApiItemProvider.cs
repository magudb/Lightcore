using System;
using System.Diagnostics;
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

        public LightcoreApiItemProvider(IOptions<LightcoreOptions> config, IMemoryCache cache)
        {
            _serializer = new JsonSerializer();
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
            var getWatch = Stopwatch.StartNew();
            var device = _config.Sitecore.Device;
            var database = _config.Sitecore.Database;
            var url = $"{_config.ServerUrl}/-/lightcore/item/{pathOrId}?sc_database={database}&sc_lang={language.Name}&sc_device={device}";

            using (var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    getWatch.Stop();

                    var readWatch = Stopwatch.StartNew();

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            using (JsonReader jsonReader = new JsonTextReader(streamReader))
                            {
                                readWatch.Stop();

                                var parseWatch = Stopwatch.StartNew();
                                var length = response.Content.Headers.ContentLength;
                                var apiResponse = _serializer.Deserialize<ServerResponseModel>(jsonReader);
                                var item = ModelToItemMapper.Map(apiResponse, language);

                                parseWatch.Stop();

                                item.Trace = $"Loaded {length} bytes in {getWatch.ElapsedMilliseconds} ms, " +
                                             $"read in {readWatch.ElapsedMilliseconds} ms,  " +
                                             $"mapped in {parseWatch.ElapsedMilliseconds} ms";

                                return item;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}