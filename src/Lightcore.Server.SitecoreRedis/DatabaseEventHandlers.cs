using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Engines.DataCommands;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Events.Hooks;
using StackExchange.Redis;

namespace Lightcore.Server.SitecoreRedis
{
    public class DatabaseEventHandlers : IHook
    {
        private static ConnectionMultiplexer _connection;
        private readonly string[] _acceptedPaths;
        private readonly string _databaseName;
        private readonly ItemModelFactory _itemModelFactory;
        private readonly string _redisConnection;

        public DatabaseEventHandlers(string database, string paths, string connection)
        {
            _databaseName = database;
            _acceptedPaths = paths.Split('|');
            _redisConnection = connection;
            _itemModelFactory = new ItemModelFactory();
        }

        private IConnectionMultiplexer RedisConnection
        {
            get { return _connection ?? (_connection = ConnectionMultiplexer.Connect(_redisConnection)); }
        }

        public void Initialize()
        {
            var database = Factory.GetDatabase(_databaseName);

            database.DataManager.DataEngine.SavedItem += OnSaved;
            database.DataManager.DataEngine.AddedVersion += OnVersionAdded;
            database.DataManager.DataEngine.DeleteProcessedSingleItem += OnDeleted;
            database.DataManager.DataEngine.RemovedVersion += OnVersionRemoved;
        }

        private void OnVersionAdded(object sender, ExecutedEventArgs<AddVersionCommand> e)
        {
            SaveVersion(e.Command.Item);
        }

        private void OnVersionRemoved(object sender, ExecutedEventArgs<RemoveVersionCommand> e)
        {
            DeleteVersion(e.Command.Item.ID.ToRedisKey(e.Command.Item.Language));
        }

        private void OnDeleted(object sender, DeleteProcessedSingleItemEventArgs e)
        {
            var keys = new List<string>();

            foreach (var language in e.Database.Languages)
            {
                keys.Add(e.ItemID.ToRedisKey(language));
            }

            Delete(e.ItemID, keys);
        }

        private void OnSaved(object sender, ExecutedEventArgs<SaveItemCommand> e)
        {
            SaveVersion(e.Command.Item);
        }

        private void SaveVersion(Item item)
        {
            if (!_acceptedPaths.Any(path => item.Paths.FullPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var model = _itemModelFactory.GetItemModel(item);
            var value = JsonConvert.SerializeObject(model);
            var database = RedisConnection.GetDatabase();

            database.StringSet(model.Id, value);
            database.HashSetAsync("item:paths", item.Paths.FullPath.ToLowerInvariant(), item.ID.ToShortID().ToString().ToLowerInvariant()).Wait();
        }

        private void DeleteVersion(string key)
        {
            var database = RedisConnection.GetDatabase();

            database.KeyDelete(key);
        }

        private void Delete(ID itemId, IEnumerable<string> keys)
        {
            var redisKeys = keys.Select(key => (RedisKey)key).ToArray();

            if (redisKeys.Length <= 0)
            {
                return;
            }

            var database = RedisConnection.GetDatabase();

            database.KeyDelete(redisKeys);

            var paths = database.HashGetAll("item:paths");

            foreach (var entry in paths)
            {
                var value = entry.Value;

                if (value.IsNullOrEmpty)
                {
                    continue;
                }

                if (!value.ToString().Equals(itemId.ToShortID().ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                database.HashDelete("item:paths", entry.Name);

                break;
            }
        }
    }
}