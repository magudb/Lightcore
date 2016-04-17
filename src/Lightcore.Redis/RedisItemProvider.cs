using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Providers;
using StackExchange.Redis;

namespace Lightcore.Redis
{
    public class RedisItemProvider : IItemProvider, IDisposable
    {
        private readonly IConnectionMultiplexer _connection;

        public RedisItemProvider()
        {
            // TODO: Stop with the ToLowerInverant insanity!
            // TODO: Create setting
            _connection = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public async Task<Item> GetItemAsync(GetItemCommand command)
        {
            var database = _connection.GetDatabase();
            var id = await GetId(command.PathOrId, database);
            var value = await database.StringGetAsync(("version:" + id + ":" + command.Language.Name).ToLowerInvariant());

            if (value.IsNullOrEmpty)
            {
                return null;
            }

            // TODO: Could we store fields in a HASH instead so we can pick only those we need according to the command, content fields in one hash and system fields in another?

            return ItemSerializer.Deserialize(value.ToString()).FirstOrDefault();
        }

        public async Task<IEnumerable<Item>> GetVersionsAsync(GetVersionsCommand command)
        {
            var database = _connection.GetDatabase();
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
                var idValue = await database.HashGetAsync("index:paths", ("/" + pathOrId).ToLowerInvariant());

                if (!idValue.IsNullOrEmpty)
                {
                    return idValue.ToString().ToLowerInvariant();
                }
            }

            return pathOrId.ToLowerInvariant();
        }
    }
}