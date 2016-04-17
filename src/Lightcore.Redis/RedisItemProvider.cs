using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Providers;
using Microsoft.Extensions.OptionsModel;
using StackExchange.Redis;

namespace Lightcore.Redis
{
    public class RedisItemProvider : IItemProvider, IDisposable
    {
        private static IConnectionMultiplexer _connection;
        private readonly RedisOptions _config;

        public RedisItemProvider(IOptions<RedisOptions> options)
        {
            _config = options.Value;
        }

        private IConnectionMultiplexer RedisConnection => _connection ?? (_connection = ConnectionMultiplexer.Connect(_config.Value.Configuration));

        public void Dispose()
        {
            RedisConnection?.Dispose();
        }

        public async Task<Item> GetItemAsync(GetItemCommand command)
        {
            var database = RedisConnection.GetDatabase();
            var id = await GetId(command.PathOrId, database);
            var value = await database.StringGetAsync(("version:" + id + ":" + command.Language.Name).ToLowerInvariant());

            if (value.IsNullOrEmpty)
            {
                return null;
            }

            return ItemSerializer.Deserialize(value.ToString()).FirstOrDefault();
        }

        public async Task<IEnumerable<Item>> GetVersionsAsync(GetVersionsCommand command)
        {
            var database = RedisConnection.GetDatabase();
            var id = await GetId(command.PathOrId, database);
            var versions = await database.HashGetAsync("index:versions", id);

            if (versions.IsNullOrEmpty)
            {
                return Enumerable.Empty<Item>();
            }

            var items = new List<Item>();

            foreach (var version in versions.ToString().Split('|'))
            {
                var item = await GetItemAsync(new GetItemCommand(id, version));

                if (item == null)
                {
                    continue;
                }

                items.Add(item);
            }

            return items;
        }

        private async Task<string> GetId(string pathOrId, IDatabase database)
        {
            if (pathOrId.Contains("/"))
            {
                var path = ("/" + pathOrId).ToLowerInvariant();
                var idValue = await database.HashGetAsync("index:paths", path);

                if (!idValue.IsNullOrEmpty)
                {
                    return idValue.ToString().ToLowerInvariant();
                }

                throw new MissingIdInIndexException($"Could not get id for path '{path}'.");
            }

            return pathOrId.ToLowerInvariant();
        }
    }
}