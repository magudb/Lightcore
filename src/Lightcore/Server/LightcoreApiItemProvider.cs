using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lightcore.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Providers;
using Lightcore.Server.Models;
using Microsoft.Extensions.OptionsModel;
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
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Lightcore");
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
            var query = new StringBuilder();

            query.Append(_config.ServerUrl);
            query.Append("/-/lightcore/item/");
            query.Append(command.PathOrId);
            query.AppendFormat("?sc_database={0}", database);
            query.AppendFormat("&sc_lang={0}", command.Language.Name);
            query.AppendFormat("&sc_device={0}", device);
            query.AppendFormat("&cdn={0}", cdn);

            if (command.ItemFields.Any())
            {
                query.AppendFormat("&itemfields={0}", string.Join(",", command.ItemFields));
            }

            if (command.ChildFields.Any())
            {
                query.AppendFormat("&childfields={0}", string.Join(",", command.ChildFields));
            }

            return ItemFactory.Create(await GetAsync(query.ToString())).FirstOrDefault();
        }

        public async Task<IEnumerable<Item>> GetVersionsAsync(GetVersionsCommand command)
        {
            var database = _config.Sitecore.Database;
            var query = new StringBuilder();

            query.Append(_config.ServerUrl);
            query.Append("/-/lightcore/versions/");
            query.Append(command.PathOrId);
            query.AppendFormat("?sc_database={0}", database);

            if (command.ItemFields.Any())
            {
                query.AppendFormat("&itemfields={0}", string.Join(",", command.ItemFields));
            }

            return ItemFactory.Create(await GetAsync(query.ToString()));
        }

        private async Task<ServerResponseModel> GetAsync(string query)
        {
            using (var response = await _client.GetAsync(query, HttpCompletionOption.ResponseHeadersRead))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        using (JsonReader jsonReader = new JsonTextReader(streamReader))
                        {
                            return _serializer.Deserialize<ServerResponseModel>(jsonReader);
                        }
                    }
                }
            }
        }
    }
}