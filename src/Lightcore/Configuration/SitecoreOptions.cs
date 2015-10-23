namespace Lightcore.Configuration
{
    public class SitecoreOptions
    {
        public string StartItem { get; set; }
        public string Database { get; set; }
        public string Device { get; set; }
        public string Cdn { get; set; }

        public void Verify()
        {
            if (string.IsNullOrEmpty(StartItem))
            {
                throw new InvalidConfigurationException($"{nameof(StartItem)} was null or empty");
            }

            if (string.IsNullOrEmpty(Database))
            {
                throw new InvalidConfigurationException($"{nameof(Database)} was null or empty");
            }

            if (string.IsNullOrEmpty(Device))
            {
                throw new InvalidConfigurationException($"{nameof(Device)} was null or empty");
            }
        }
    }
}