using System;

namespace Lightcore.Kernel.Data.Fields
{
    public class LinkField : Field
    {
        public LinkField(string key, string type, Guid id, string description, string targetUrl, Guid targetId)
            : base(key, type, id, !string.IsNullOrEmpty(targetUrl) ? targetId.ToString() : targetUrl)
        {
            Description = description;
            TargetUrl = targetUrl;
            TargetId = targetId;
        }

        public string Description { get; }
        public string TargetUrl { get; }
        public Guid TargetId { get; }
    }
}