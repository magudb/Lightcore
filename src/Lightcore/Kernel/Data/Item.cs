using System;
using System.Collections.Generic;

namespace Lightcore.Kernel.Data
{
    public class Item
    {
        public string Path { get; set; }
        public string Key { get; set; }
        public Guid Id { get; set; }
        public string Layout { get; set; }
        public Language Language { get; set; }
        public Dictionary<string, string> Renderings { get; set; }
    }
}