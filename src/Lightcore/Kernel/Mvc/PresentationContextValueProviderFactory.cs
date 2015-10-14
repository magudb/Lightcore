using Microsoft.AspNet.Mvc.ModelBinding;

namespace Lightcore.Kernel.Mvc
{
    public class PresentationContextValueProviderFactory : IValueProviderFactory
    {
        public IValueProvider GetValueProvider(ValueProviderFactoryContext context)
        {
            return new PresentationContextValueProvider(context);
        }
    }
}