using System;

namespace Lightcore.Redis.Models
{
    public class ItemPropertyModel
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string Language { get; set; }
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid TemplateId { get; set; }
        public bool HasVersion { get; set; }
    }
}