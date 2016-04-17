using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Providers;
using StackExchange.Redis;

namespace Lightcore.Redis
{
    public class RedisItemProvider : IItemProvider
    {
        private readonly RedisServer _server;

        public RedisItemProvider(RedisServer server)
        {
            // TODO: Subscribe and handle connection failures

            _server = server;
            _server.SubscribeChangeEvents(HandleChangeEvent);
        }

        public async Task<Item> GetItemAsync(GetItemCommand command)
        {
            var database = _server.Connection.GetDatabase();
            var id = await GetId(command.PathOrId, database);
            var key = ("version:" + id + ":" + command.Language.Name).ToLowerInvariant();
            var value = await database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return null;
            }

            return ItemSerializer.Deserialize(value.ToString()).FirstOrDefault();
        }

        public async Task<IEnumerable<Item>> GetVersionsAsync(GetVersionsCommand command)
        {
            var database = _server.Connection.GetDatabase();
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

        private void HandleChangeEvent(string value)
        {
            Debug.WriteLine("Change: " + value);
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