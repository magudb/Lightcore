using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string[] _fields;
        private readonly ItemSerializer _itemSerializer;
        private readonly string _pathIndexName;
        private readonly string _redisConfiguration;
        private readonly string _versionsIndexName;

        public DatabaseEventHandlers(string database, string configuration, string paths, string fields)
        {
            _databaseName = database;
            _redisConfiguration = configuration;
            _acceptedPaths = paths.Split('|');
            _fields = fields.Split('|');
            _itemSerializer = new ItemSerializer();
            _pathIndexName = "index:paths";
            _versionsIndexName = "index:versions";
        }

        private IConnectionMultiplexer RedisConnection
        {
            get { return _connection ?? (_connection = ConnectionMultiplexer.Connect(_redisConfiguration)); }
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
            DeleteVersion(e.Command.Item, e.Command.Item.ID.ToStorageKey(e.Command.Item.Language));
        }

        private void OnDeleted(object sender, DeleteProcessedSingleItemEventArgs e)
        {
            var keys = e.Database.Languages.Select(language => e.ItemID.ToStorageKey(language));

            DeleteItem(e.ItemID, keys);
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

            var key = item.ID.ToStorageKey(item.Language);
            var value = _itemSerializer.Serialize(item, key, _fields);
            var database = RedisConnection.GetDatabase();

            database.StringSet(key, value);
            database.HashSetAsync(_pathIndexName, item.Paths.FullPath.ToLowerInvariant(), item.ID.AsLowercaseGuid()).Wait();
            database.HashSetAsync(_versionsIndexName, item.ID.AsLowercaseGuid(), item.LanguageVersionNames()).Wait();
        }

        private void DeleteVersion(Item item, string key)
        {
            var database = RedisConnection.GetDatabase();

            database.KeyDelete(key);
            database.HashSetAsync(_versionsIndexName, item.ID.AsLowercaseGuid(), item.LanguageVersionNames()).Wait();
        }

        private void DeleteItem(ID itemId, IEnumerable<string> keys)
        {
            var redisKeys = keys.Select(key => (RedisKey)key).ToArray();

            if (redisKeys.Length <= 0)
            {
                return;
            }

            var database = RedisConnection.GetDatabase();

            database.KeyDelete(redisKeys);

            var paths = database.HashGetAll(_pathIndexName);

            foreach (var entry in paths)
            {
                var value = entry.Value;

                if (value.IsNullOrEmpty)
                {
                    continue;
                }

                if (value.ToString() != itemId.AsLowercaseGuid())
                {
                    continue;
                }

                database.HashDelete(_pathIndexName, entry.Name);

                break;
            }

            database.HashDelete(_versionsIndexName, itemId.AsLowercaseGuid());
        }
    }
}