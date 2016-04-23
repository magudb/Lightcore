using System.Collections.Generic;
using System.Linq;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Publishing;
using StackExchange.Redis;

namespace Lightcore.Server.SitecoreRedis
{
    public class SaveSnapshot : PublishCompletedEventHandlerBase
    {
        private readonly Language _defaultLanguage;
        private readonly string[] _fields;
        private readonly ItemSerializer _itemSerializer;
        private readonly string[] _paths;

        public SaveSnapshot(string paths, string fields)
        {
            _paths = paths.Split('|');
            _fields = fields.Split('|');
            _itemSerializer = new ItemSerializer();
            _defaultLanguage = LanguageManager.DefaultLanguage;
        }

        public override void Completed(IEnumerable<DistributedPublishOptions> options)
        {
            var source = Factory.GetDatabase(options.First().TargetDatabaseName);
            var database = Connection.GetDatabase();
            var snapshotVersion = database.StringIncrement("snapshot");
            var processed = new List<Item>();

            try
            {
                foreach (var path in _paths)
                {
                    processed.AddRange(ProcessItem(database, snapshotVersion, source.Items.GetItem(path)));
                }

                var pathMap = processed.Select(item => new HashEntry(item.Paths.FullPath.ToLowerInvariant(), item.ID.Guid.ToString()));

                database.HashSet(snapshotVersion + ":index:paths", pathMap.ToArray());

                // TODO: Send message of processed items (per language?) when complete so we can clear cache etc.?  
            }
            catch
            {
                database.StringDecrement("snapshot");

                throw;
            }

            Connection.GetSubscriber().Publish("events", "publish");
        }

        private IEnumerable<Item> ProcessItem(IDatabase database, long version, Item item)
        {
            var processed = new List<Item>
            {
                SaveItem(database, version, item)
            };

            foreach (Item child in item.GetChildren(ChildListOptions.SkipSorting))
            {
                processed.AddRange(ProcessItem(database, version, child));
            }

            return processed;
        }

        private Item SaveItem(IDatabase database, long snapshotVersion, Item item)
        {
            var data = new List<KeyValuePair<RedisKey, RedisValue>>();

            // Make sure the default language version is always saved...
            var versions = new List<Item>(item.Versions.GetVersions(true).Where(v => v.Language != _defaultLanguage))
            {
                item.Database.Items.GetItem(item.ID, _defaultLanguage)
            };

            foreach (var version in versions)
            {
                var key = version.ID.ToStorageKey(snapshotVersion, version.Language);
                var value = _itemSerializer.Serialize(version, _fields);

                data.Add(new KeyValuePair<RedisKey, RedisValue>(key, value));
            }

            database.StringSet(data.ToArray());

            return item;
        }
    }
}