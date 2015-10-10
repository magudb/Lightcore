using System;

namespace Lightcore.Server.Models
{
    public class ItemModel
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool HasVersion { get; set; }
    }
}