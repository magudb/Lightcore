using FluentAssertions;
using Lightcore.Kernel.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Urls;
using Xunit;

namespace Lightcore.Tests.Urls
{
    public class ItemUrlServiceShould
    {
        [Fact]
        public void return_absolute_path_for_item()
        {
            //// Arrange
            var item = new Item
            {
                Path = "/sitecore/content/Home/Products",
                Language = new Language("da-DK")
            };

            var service = new ItemUrlService(LightcoreOptions.Default);

            //// Act
            var url = service.GetUrl(item);

            //// Asset
            url.Should().Be("/da-dk/products");
        }
    }
}