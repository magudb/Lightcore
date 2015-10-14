using System.Collections.Generic;

namespace Lightcore.Kernel.Data
{
    public class Rendering
    {
        public Rendering(string placeholder, string dataSource, string controller, string action, Dictionary<string, string> parameters)
        {
            Placeholder = placeholder;
            DataSource = dataSource;
            Controller = controller;
            Action = action;
            Parameters = parameters;
        }

        public string DataSource { get; }
        public string Controller { get; }
        public string Action { get; }
        public Dictionary<string, string> Parameters { get; }
        public string Placeholder { get; }
    }
}