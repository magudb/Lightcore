namespace Lightcore.Kernel.Data
{
    public class Rendering
    {
        public Rendering(string placeholder, string controller, string action = "Index")
        {
            Placeholder = placeholder;
            Controller = controller;
            Action = action;
        }

        public string Controller { get; }
        public string Action { get; }
        public string Placeholder { get; }
    }
}