using System;
using System.Collections.Generic;
using Sitecore.Configuration;
using Sitecore.Events;
using Sitecore.Publishing;
using Sitecore.Xml;
using StackExchange.Redis;

namespace Lightcore.Server.SitecoreRedis
{
    public abstract class PublishCompletedEventHandlerBase
    {
        protected PublishCompletedEventHandlerBase()
        {
            var configNode = Factory.GetConfigNode("lightcore/redis/connection");
            var configuration = XmlUtil.GetAttribute("configuration", configNode);

            Connection = ConnectionMultiplexer.Connect(configuration);
        }

        protected ConnectionMultiplexer Connection { get; private set; }

        public abstract void Completed(IEnumerable<DistributedPublishOptions> options);

        public void Run(object sender, EventArgs e)
        {
            var args = e as SitecoreEventArgs;

            if (args == null)
            {
                return;
            }

            var options = args.Parameters[0] as IEnumerable<DistributedPublishOptions>;

            if (options == null)
            {
                return;
            }

            Completed(options);
        }
    }
}