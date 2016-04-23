using Sitecore.Publishing;

namespace Lightcore.Server.SitecoreRedis
{
    public class NotifyCompleted : PublishCompletedEventHandlerBase
    {
        private readonly string _channel;

        public NotifyCompleted(string channel)
        {
            _channel = channel;
        }

        public override void Completed(DistributedPublishOptions[] options)
        {
            foreach (var option in options)
            {
                Connection.GetSubscriber().Publish(_channel, "published:" + option.LanguageName.ToLowerInvariant());
            }
        }
    }
}