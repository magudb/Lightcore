using System;
using System.IO;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Server.Models;
using Newtonsoft.Json;

namespace Lightcore.Server
{
    public class LightcoreDiskItemProvider : IItemProvider, IDisposable
    {
        private readonly JsonSerializer _serializer;

        public LightcoreDiskItemProvider()
        {
            _serializer = new JsonSerializer();
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

                            return ItemFactory.Create(apiResponse, language);
                        }
                    }
                }
            }
        }
    }
}