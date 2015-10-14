using System;
using System.Collections.Generic;

namespace Lightcore.Kernel.Mvc
{
    public class PresentationContext
    {
        public PresentationContext()
        {
        }

        public PresentationContext(Guid itemId, string itemLanguageName, Guid itemTemplateId, string dataSource, Dictionary<string, string> parameters)
        {
            ItemId = itemId;
            ItemLanguageName = itemLanguageName;
            ItemTemplateId = itemTemplateId;
            DataSource = dataSource;
            Parameters = parameters;
        }

        public virtual string DataSource { get; private set; }
        public virtual string ItemLanguageName { get; private set; }
        public virtual Guid ItemId { get; private set; }
        public virtual Guid ItemTemplateId { get; private set; }
        public virtual Dictionary<string, string> Parameters { get; private set; }
    }
}