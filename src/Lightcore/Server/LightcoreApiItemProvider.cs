using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Lightcore.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Providers;
using Lightcore.Server.Models;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;

namespace Lightcore.Server
{
    public class LightcoreApiItemProvider : IItemProvider, IDisposable
    {
        private readonly HttpClient _client;
        private readonly LightcoreOptions _config;
        private readonly JsonSerializer _serializer;

        public LightcoreApiItemProvider(IOptions<LightcoreOptions> options)
        {
            _serializer = new JsonSerializer();
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

        public async Task<Item> GetItemAsync(GetItemCommand command)
        {
            var device = _config.Sitecore.Device;
            var database = _config.Sitecore.Database;
            var cdn = _config.Sitecore.Cdn;

            // TODO: Send command.ItemFields and command.ChildFields
            var url = $"{_config.ServerUrl}/-/lightcore/item/{command.PathOrId}?sc_database={database}&sc_lang={command.Language.Name}&sc_device={device}&cdn={cdn}";

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

                                return ItemFactory.Create(apiResponse, command.Language);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}