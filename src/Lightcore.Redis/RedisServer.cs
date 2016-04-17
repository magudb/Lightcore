using System;
using Microsoft.Extensions.OptionsModel;
using StackExchange.Redis;

namespace Lightcore.Redis
{
    public class RedisServer
    {
        private static IConnectionMultiplexer _connection;

        public RedisServer(IOptions<RedisOptions> options)
        {
            _connection = ConnectionMultiplexer.Connect(options.Value.Configuration);
        }

        public IConnectionMultiplexer Connection => _connection;

        public void SubscribeChangeEvents(Action<string> handler)
        {
            _connection.GetSubscriber().Subscribe("changes", (channel, value) => { handler.Invoke(value.ToString()); });
        }
    }
}