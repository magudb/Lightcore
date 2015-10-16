using System.Collections.Generic;

namespace Lightcore.Kernel.Data
{
    public class Rendering
    {
        public Rendering(string placeholder, string datasource, string controller, string action, Dictionary<string, string> parameters, Caching caching)
        {
            Placeholder = placeholder;
            Datasource = datasource;
            Controller = controller;
            Action = action;
            Parameters = parameters;
            Caching = caching;
        }

        public string Datasource { get; }
        public string Controller { get; }
        public string Action { get; }
        public Dictionary<string, string> Parameters { get; }
        public string Placeholder { get; }
        public Caching Caching { get; }
    }
}