using System;

namespace Lightcore.Kernel.Data
{
    public class LinkField : Field
    {
        public string Description { get; set; }
        public string TargetUrl { get; set; }
        public Guid TargetId { get; set; }
    }
}