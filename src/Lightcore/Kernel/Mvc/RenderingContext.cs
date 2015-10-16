using System;

namespace Lightcore.Kernel.Mvc
{
    public class RenderingContext
    {
        public virtual string DataSource { get; set; }
        public virtual string ItemLanguageName { get; set; }
        public virtual Guid ItemId { get; set; }
        public virtual Guid ItemTemplateId { get; set; }
    }
}