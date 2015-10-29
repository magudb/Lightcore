using FluentAssertions;
using Lightcore.Configuration;
using Lightcore.Kernel.Data;
using Lightcore.Kernel.Data.Globalization;
using Lightcore.Kernel.Url;
using Xunit;

namespace Lightcore.Tests.Urls
{
    public class ItemUrlServiceShould
    {
        [Fact]
        public void return_absolute_path_for_item()
        {
            //// Arrange
            var item = new MutableItemDefinition
            {
                Path = "/sitecore/content/Home/Products",
                Language = Language.Parse("da-DK")
            };

            var service = new ItemUrlService(LightcoreOptions.Default);

            //// Act
            var url = service.GetUrl(item);

            //// Assert
            url.Should().Be("/da-dk/products");
        }
    }
}