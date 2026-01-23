namespace Shared.Cdn;

public static class ImageVariantNaming
{
    public static string BuildVariantName(string imageId, int width, ImageFormat format)
    {
        return $"{imageId}_{width}w.{format.ToString().ToLowerInvariant()}";
    }
}
