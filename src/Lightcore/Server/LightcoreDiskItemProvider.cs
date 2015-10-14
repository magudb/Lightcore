using System;
using System.IO;
using System.Threading.Tasks;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Server.Models;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;

namespace Lightcore.Server
{
    public class LightcoreDiskItemProvider : IItemProvider, IDisposable
    {
        private readonly IMemoryCache _cache;
        private readonly LightcoreOptions _config;
        private readonly JsonSerializer _serializer;

        public LightcoreDiskItemProvider(IOptions<LightcoreOptions> config, IMemoryCache cache)
        {
            _serializer = new JsonSerializer();
            _cache = cache;
            _config = config.Options;
        }

        public void Dispose()
        {
        }

        public async Task<Item> GetItemAsync(string pathOrId, Language language)
        {
            var file = $"e:\\temp\\Lightcore\\web\\{language.Name}\\default\\{pathOrId}\\item.json";

            if (!File.Exists(file))
            {
                return null;
            }

            using (var stream = File.OpenRead(file))
            {
                using (var memory = new MemoryStream())
                {
                    await stream.CopyToAsync(memory);

                    memory.Position = 0;

                    using (var streamReader = new StreamReader(memory))
                    {
                        using (JsonReader jsonReader = new JsonTextReader(streamReader))
                        {
                            var apiResponse = _serializer.Deserialize<ServerResponseModel>(jsonReader);
                            var item = ModelToItemMapper.Map(apiResponse, language);

                            item.Trace = "...";

                            return item;
                        }
                    }
                }
            }
        }
    }
}