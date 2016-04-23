using System.Collections.Generic;
using Sitecore.Publishing;
using StackExchange.Redis;

namespace Lightcore.Server.SitecoreRedis
{
    public class DeletePreviousSnapshots : PublishCompletedEventHandlerBase
    {
        public override void Completed(IEnumerable<DistributedPublishOptions> options)
        {
            var database = Connection.GetDatabase();
            var deletes = new List<RedisKey>();
            long latestSnapshotVersion;

            if (!database.StringGet("snapshot").TryParse(out latestSnapshotVersion))
            {
                // TODO: Throw exception...
                return;
            }

            foreach (var endpoint in Connection.GetEndPoints())
            {
                var server = Connection.GetServer(endpoint);

                if (!server.IsConnected)
                {
                    // TODO: Log server disconnected...
                    continue;
                }

                var keys = new List<RedisKey>();

                keys.AddRange(server.Keys(pattern: "*:version:*"));
                keys.AddRange(server.Keys(pattern: "*:index:paths"));

                foreach (var key in keys)
                {
                    if (!key.ToString().StartsWith(latestSnapshotVersion + ":"))
                    {
                        deletes.Add(key);
                    }
                }
            }

            database.KeyDelete(deletes.ToArray());

            // TODO: Log keys deleted...
        }
    }
}