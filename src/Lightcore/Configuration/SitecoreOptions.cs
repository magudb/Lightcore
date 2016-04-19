namespace Lightcore.Configuration
{
    public class SitecoreOptions
    {
        public string StartItem { get; set; }

        public void Verify()
        {
            if (string.IsNullOrEmpty(StartItem))
            {
                throw new InvalidConfigurationException($"{nameof(StartItem)} was null or empty");
            }
        }
    }
}