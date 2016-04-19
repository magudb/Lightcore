using System;

namespace Lightcore.Redis.Models
{
    public class LinkFieldValueModel
    {
        public string TargetUrl { get; set; }
        public Guid TargetId { get; set; }
        public string Description { get; set; }
    }
}