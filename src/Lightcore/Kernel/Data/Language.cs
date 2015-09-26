namespace Lightcore.Kernel.Data
{
    public class Language
    {
        public Language(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public static Language Default => new Language("en-us");
    }
}