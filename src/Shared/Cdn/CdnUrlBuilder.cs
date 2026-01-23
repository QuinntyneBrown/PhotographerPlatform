namespace Shared.Cdn;

public sealed class CdnUrlBuilder
{
    private readonly string _baseUrl;

    public CdnUrlBuilder(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
    }

    public string BuildImageUrl(string imageId, ImageTransformOptions options)
    {
        var query = new List<string>
        {
            $"w={options.Width}",
            $"h={options.Height}",
            $"fit={options.Fit}",
            $"fmt={options.Format}",
            $"q={options.Quality}"
        };

        return $"{_baseUrl}/images/{imageId}?{string.Join("&", query)}";
    }

    public string BuildResponsiveVariantUrl(string imageId, ResponsiveImageVariant variant)
    {
        return $"{_baseUrl}/images/{imageId}?w={variant.Width}&fmt={variant.Format}";
    }
}
