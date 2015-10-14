using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Lightcore.Kernel.Mvc
{
    public class PresentationContextValueProvider : IValueProvider
    {
        private readonly DictionaryBasedValueProvider _provider;

        public PresentationContextValueProvider(ValueProviderFactoryContext context)
        {
            var collection = new Dictionary<string, object>();
            var lightcoreContext = context.HttpContext.LightcoreContext();
            var item = lightcoreContext?.Item;

            if (item != null)
            {
                var controller = context.RouteValues["controller"] as string;

                if (controller != null && !controller.Equals("Lightcore"))
                {
                    var rendering = item.Visualization.Renderings
                                        .FirstOrDefault(r => r.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase));

                    if (rendering != null)
                    {
                        collection.Add("presentationContext", new PresentationContext(item.Id,
                            item.Language.Name,
                            item.TemplateId,
                            rendering.DataSource,
                            new Dictionary<string, string>()));
                    }
                }
            }

            _provider = new DictionaryBasedValueProvider(BindingSource.ModelBinding, collection);
        }

        public Task<bool> ContainsPrefixAsync(string prefix)
        {
            return _provider.ContainsPrefixAsync(prefix);
        }

        public Task<ValueProviderResult> GetValueAsync(string key)
        {
            return _provider.GetValueAsync(key);
        }
    }
}