using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data.Engines.DataCommands;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
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
            Save(e.Command.Item);
        }

        private void OnVersionRemoved(object sender, ExecutedEventArgs<RemoveVersionCommand> e)
        {
            Delete(new RedisKey[] {e.Command.Item.ID.ToRedisKey(e.Command.Item.Language)});
        }

        private void OnDeleted(object sender, DeleteProcessedSingleItemEventArgs e)
        {
            var keys = new List<RedisKey>();

            foreach (var language in e.Database.Languages)
            {
                keys.Add(e.ItemID.ToRedisKey(language));
            }

            Delete(keys.ToArray());
        }

        private void OnSaved(object sender, ExecutedEventArgs<SaveItemCommand> e)
        {
            Save(e.Command.Item);
        }

        private void Save(Item item, [CallerMemberName] string memberName = "")
        {
            if (!_acceptedPaths.Any(path => item.Paths.FullPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var model = _itemModelFactory.GetItemModel(item);
            var value = JsonConvert.SerializeObject(model);
            var results = RedisConnection.GetDatabase().StringSet(model.Id, value);

            Log.Debug(string.Format("Redis: Handled '{0}' and saved key '{1}', response was '{2}'.", memberName, model.Id, results), this);
        }

        private void Delete(RedisKey[] keys, [CallerMemberName] string memberName = "")
        {
            if (keys.Length <= 0)
            {
                return;
            }

            var results = RedisConnection.GetDatabase().KeyDelete(keys);

            Log.Debug(string.Format("Redis: Handled '{0}' and deleted keys '{1}', response was '{2}'.", memberName, string.Join(", ", keys), results), this);
        }
    }
}