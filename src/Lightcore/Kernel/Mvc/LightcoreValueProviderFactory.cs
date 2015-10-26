using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Lightcore.Kernel.Mvc
{
    public class LightcoreValueProviderFactory : IValueProviderFactory
    {
        public Task<IValueProvider> GetValueProviderAsync(ValueProviderFactoryContext context)
        {
            var collection = new Dictionary<string, object>();
            var lightcoreContext = context.HttpContext.LightcoreContext();
            var item = lightcoreContext?.Item;

            if (item != null)
            {
                var controller = context.RouteValues["controller"] as string;

                if (controller != null && !controller.Equals("Lightcore"))
                {
                    var rendering = item.PresentationDetails.Renderings
                                        .FirstOrDefault(r => r.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase));

                    if (rendering != null)
                    {
                        foreach (var parameter in rendering.Parameters)
                        {
                            collection.Add(parameter.Key, parameter.Value);
                        }

                        collection.Add("datasource", rendering.Datasource);
                        collection.Add("language", item.Language.Name);
                        collection.Add("itemId", item.Id);
                        collection.Add("templateId", item.TemplateId);

                        collection.Add("renderingContext.Datasource", rendering.Datasource);
                        collection.Add("renderingContext.ItemLanguageName", item.Language.Name);
                        collection.Add("renderingContext.ItemId", item.Id);
                        collection.Add("renderingContext.ItemTemplateId", item.TemplateId);
                    }
                }
            }

            return Task.FromResult<IValueProvider>(new DictionaryBasedValueProvider(BindingSource.ModelBinding, collection));
        }
    }
}