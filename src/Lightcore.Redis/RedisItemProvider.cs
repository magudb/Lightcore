using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Globalization;
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
            _server.SubscribeEvents(HandleChangeEvent);
        }

        public async Task<Item> GetItemAsync(GetItemCommand command)
        {
            var database = _server.Connection.GetDatabase();
            var snapshotVersion = await GetSnapshotVersion(database);
            var id = await GetId(snapshotVersion, command.PathOrId, database);
            var key = (snapshotVersion + ":version:" + id + ":" + command.Language.Name).ToLowerInvariant();
            var value = await database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                if (!command.Language.Name.Equals(Language.Default.Name))
                {
                    key = (snapshotVersion + ":version:" + id + ":" + Language.Default.Name).ToLowerInvariant();
                    value = await database.StringGetAsync(key);
                }

                if (value.IsNullOrEmpty)
                {
                    return null;
                }
            }

            return ItemSerializer.Deserialize(value.ToString());
        }

        public async Task<IEnumerable<Item>> GetVersionsAsync(GetVersionsCommand command)
        {
            var database = _server.Connection.GetDatabase();
            var snapshotVersion = await GetSnapshotVersion(database);
            var id = await GetId(snapshotVersion, command.PathOrId, database);
            var versions = await database.HashGetAsync(snapshotVersion + ":index:versions", id);

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

        private async Task<long> GetSnapshotVersion(IDatabase database)
        {
            var value = await database.StringGetAsync("snapshot");

            long version;

            if (!value.TryParse(out version))
            {
                // TODO: Throw exception
            }

            return version;
        }

        private void HandleChangeEvent(string value)
        {
            Debug.WriteLine("Change: " + value);
        }

        private async Task<string> GetId(long snapshotVersion, string pathOrId, IDatabase database)
        {
            if (pathOrId.Contains("/"))
            {
                var path = ("/" + pathOrId).ToLowerInvariant();
                var idValue = await database.HashGetAsync(snapshotVersion + ":index:paths", path);

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