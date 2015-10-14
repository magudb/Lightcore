namespace Lightcore.Kernel.Data
{
    public class Rendering
    {
        public Rendering(string placeholder, string dataSource, string controller, string action = "Index")
        {
            Placeholder = placeholder;
            DataSource = dataSource;
            Controller = controller;
            Action = action;
        }

        public string DataSource { get; }
        public string Controller { get; }
        public string Action { get; }
        public string Placeholder { get; }
    }
}