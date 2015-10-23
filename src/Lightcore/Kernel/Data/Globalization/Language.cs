namespace Lightcore.Kernel.Data.Globalization
{
    public class Language
    {
        private Language(string name)
        {
            Name = name.ToLowerInvariant();
        }

        public string Name { get; }

        public static Language Default => new Language("en-US");

        public static Language Parse(string language)
        {
            return new Language(language);
        }
    }
}