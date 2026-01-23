using Shared.Cdn;
using Xunit;

namespace Shared.Tests.Cdn;

public sealed class CdnUrlBuilderTests
{
    [Fact]
    public void BuildImageUrl_BuildsQueryWithTransformOptions()
    {
        var builder = new CdnUrlBuilder("https://cdn.example.com/");
        var options = new ImageTransformOptions
        {
            Width = 800,
            Height = 600,
            Fit = ImageFit.Contain,
            Format = ImageFormat.WebP,
            Quality = 80
        };

        var url = builder.BuildImageUrl("img_1", options);

        Assert.Equal("https://cdn.example.com/images/img_1?w=800&h=600&fit=Contain&fmt=WebP&q=80", url);
    }

    [Fact]
    public void BuildResponsiveVariantUrl_BuildsVariantUrl()
    {
        var builder = new CdnUrlBuilder("https://cdn.example.com");
        var variant = new ResponsiveImageVariant
        {
            Url = "",
            Width = 400,
            Format = ImageFormat.Jpeg
        };

        var url = builder.BuildResponsiveVariantUrl("img_2", variant);

        Assert.Equal("https://cdn.example.com/images/img_2?w=400&fmt=Jpeg", url);
    }
}
