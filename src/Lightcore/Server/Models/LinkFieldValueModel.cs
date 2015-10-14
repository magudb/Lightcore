using System;

namespace Lightcore.Server.Models
{
    public class LinkFieldValueModel
    {
        public string TargetUrl { get; set; }
        public Guid TargetId { get; set; }
        public string Description { get; set; }
    }
}