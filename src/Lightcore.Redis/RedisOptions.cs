using Microsoft.Extensions.OptionsModel;

namespace Lightcore.Redis
{
    public class RedisOptions : IOptions<RedisOptions>
    {
        public string Configuration { get; set; }

        public RedisOptions Value => this;
    }
}