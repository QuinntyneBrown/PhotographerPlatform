using Shared.Cdn;

namespace PhotographerPlatform.Galleries.Api.Services;

public sealed class ImageUrlBuilder
{
    private readonly CdnUrlBuilder _cdn;
    private readonly SignedUrlGenerator? _signer;
    private readonly TimeSpan _signatureLifetime;

    public ImageUrlBuilder(string baseUrl, string? signedUrlSecret, TimeSpan signatureLifetime)
    {
        _cdn = new CdnUrlBuilder(baseUrl);
        _signatureLifetime = signatureLifetime;
        if (!string.IsNullOrWhiteSpace(signedUrlSecret))
        {
            _signer = new SignedUrlGenerator(signedUrlSecret);
        }
    }

    public string BuildImageUrl(string imageId, ImageTransformOptions options)
    {
        var url = _cdn.BuildImageUrl(imageId, options);
        return SignIfNeeded(url);
    }

    public string BuildVariantUrl(string imageId, ResponsiveImageVariant variant)
    {
        var url = _cdn.BuildResponsiveVariantUrl(imageId, variant);
        return SignIfNeeded(url);
    }

    private string SignIfNeeded(string url)
    {
        if (_signer is null)
        {
            return url;
        }

        var expires = DateTimeOffset.UtcNow.Add(_signatureLifetime).ToUnixTimeMilliseconds();
        return _signer.SignUrl(url, expires);
    }
}
