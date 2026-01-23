using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Services;

namespace PhotographerPlatform.Galleries.Infrastructure.Processing;

public sealed class NoOpImageProcessingService : IImageProcessingService
{
    public Task<ImageMetadata> ExtractMetadataAsync(Stream imageStream, CancellationToken ct = default)
    {
        return Task.FromResult(new ImageMetadata());
    }

    public async Task<ProcessedImage> ResizeAsync(Stream imageStream, ImageProcessingOptions options, CancellationToken ct = default)
    {
        var memory = new MemoryStream();
        await imageStream.CopyToAsync(memory, ct).ConfigureAwait(false);
        memory.Position = 0;
        return new ProcessedImage
        {
            Content = memory,
            FileName = "processed.jpg",
            ContentType = "image/jpeg"
        };
    }
}
