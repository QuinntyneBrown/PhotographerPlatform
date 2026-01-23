using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Galleries.Api.Models;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;
using PhotographerPlatform.Galleries.Core.Services;
using Shared.Cdn;

namespace PhotographerPlatform.Galleries.Api.Controllers;

[ApiController]
public sealed class ImagesController : ControllerBase
{
    private readonly IGalleriesService _service;
    private readonly IImageRepository _images;
    private readonly IStorageService _storage;
    private readonly IImageProcessingService _processing;
    private readonly IDownloadService _downloads;

    public ImagesController(
        IGalleriesService service,
        IImageRepository images,
        IStorageService storage,
        IImageProcessingService processing,
        IDownloadService downloads)
    {
        _service = service;
        _images = images;
        _storage = storage;
        _processing = processing;
        _downloads = downloads;
    }

    [HttpPost("/api/galleries/{id}/images")]
    public async Task<ActionResult<IReadOnlyList<Image>>> UploadImages(string id, [FromForm] List<IFormFile> files, CancellationToken ct)
    {
        if (files.Count == 0)
        {
            return BadRequest("No files uploaded.");
        }

        var uploads = new List<ImageUploadDescriptor>();
        foreach (var file in files)
        {
            if (!IsAllowedFileType(file.FileName))
            {
                return BadRequest($"Unsupported file type: {file.FileName}");
            }

            await using var input = file.OpenReadStream();
            await using var buffer = new MemoryStream();
            await input.CopyToAsync(buffer, ct).ConfigureAwait(false);
            var bytes = buffer.ToArray();

            await using var originalStream = new MemoryStream(bytes);
            var storedOriginal = await _storage.SaveAsync(originalStream, file.FileName, "originals", file.ContentType, ct)
                .ConfigureAwait(false);

            await using var metadataStream = new MemoryStream(bytes);
            var metadata = await _processing.ExtractMetadataAsync(metadataStream, ct).ConfigureAwait(false);

            await using var webStream = new MemoryStream(bytes);
            var web = await _processing.ResizeAsync(webStream, new ImageProcessingOptions { MaxWidth = 2048, MaxHeight = 2048 }, ct)
                .ConfigureAwait(false);
            var webStored = await _storage.SaveAsync(web.Content, file.FileName, "processed", web.ContentType, ct)
                .ConfigureAwait(false);
            await web.Content.DisposeAsync().ConfigureAwait(false);

            await using var thumbStream = new MemoryStream(bytes);
            var thumb = await _processing.ResizeAsync(thumbStream, new ImageProcessingOptions { MaxWidth = 600, MaxHeight = 600 }, ct)
                .ConfigureAwait(false);
            var thumbStored = await _storage.SaveAsync(thumb.Content, file.FileName, "thumbnails", thumb.ContentType, ct)
                .ConfigureAwait(false);
            await thumb.Content.DisposeAsync().ConfigureAwait(false);

            uploads.Add(new ImageUploadDescriptor
            {
                FileName = file.FileName,
                OriginalPath = storedOriginal.Path,
                WebPath = webStored.Path,
                ThumbnailSmallPath = thumbStored.Path,
                ThumbnailMediumPath = thumbStored.Path,
                ThumbnailLargePath = thumbStored.Path,
                Metadata = metadata
            });
        }

        var images = await _service.AddImagesAsync(id, uploads, ct).ConfigureAwait(false);
        return Ok(images);
    }

    [HttpPost("/api/galleries/{id}/images/bulk")]
    public Task<ActionResult<IReadOnlyList<Image>>> BulkUploadImages(string id, [FromForm] List<IFormFile> files, CancellationToken ct)
    {
        return UploadImages(id, files, ct);
    }

    [HttpPut("/api/galleries/{id}/images/order")]
    public async Task<IActionResult> Reorder(string id, [FromBody] ImageOrderRequest request, CancellationToken ct)
    {
        await _service.ReorderImagesAsync(id, request.ImageIds, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpDelete("/api/images/{id}")]
    public async Task<IActionResult> DeleteImage(string id, CancellationToken ct)
    {
        await _service.DeleteImageAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpGet("/api/images/{id}")]
    public async Task<ActionResult<Image>> GetImage(string id, CancellationToken ct)
    {
        var image = await _images.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (image is null)
        {
            return NotFound();
        }

        return Ok(image);
    }

    [HttpPost("/api/images/{id}/favorite")]
    public async Task<ActionResult<FavoriteToggleResult>> ToggleFavorite(string id, [FromBody] FavoriteRequest request, CancellationToken ct)
    {
        var clientId = request.ClientId ?? Request.Headers["X-Client-Id"].ToString();
        if (string.IsNullOrWhiteSpace(clientId))
        {
            return BadRequest("ClientId is required.");
        }

        var image = await _images.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (image is null)
        {
            return NotFound();
        }

        var result = await _service.ToggleFavoriteAsync(image.GalleryId, id, clientId, ct).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("/api/images/{id}/comments")]
    public async Task<ActionResult<Comment>> AddComment(string id, [FromBody] CommentRequest request, CancellationToken ct)
    {
        var clientId = request.ClientId ?? Request.Headers["X-Client-Id"].ToString();
        if (string.IsNullOrWhiteSpace(clientId))
        {
            return BadRequest("ClientId is required.");
        }

        var comment = await _service.AddCommentAsync(id, clientId, request.Content, request.ParentCommentId, ct)
            .ConfigureAwait(false);
        return Ok(comment);
    }

    [HttpGet("/api/images/{id}/comments")]
    public async Task<ActionResult<IReadOnlyList<Comment>>> ListComments(string id, CancellationToken ct)
    {
        var comments = await _service.ListCommentsAsync(id, ct).ConfigureAwait(false);
        return Ok(comments);
    }

    [HttpDelete("/api/comments/{id}")]
    public async Task<IActionResult> DeleteComment(string id, CancellationToken ct)
    {
        await _service.DeleteCommentAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpGet("/api/images/{id}/download")]
    public async Task<IActionResult> DownloadImage(string id, [FromQuery] string mode, CancellationToken ct)
    {
        var image = await _images.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (image is null)
        {
            return NotFound();
        }

        var permission = mode.Equals("full", StringComparison.OrdinalIgnoreCase)
            ? DownloadPermission.FullSize
            : DownloadPermission.WebSize;

        var gallery = await _service.GetGalleryAsync(image.GalleryId, ct).ConfigureAwait(false);
        if (gallery is null)
        {
            return NotFound();
        }

        var allowed = await _service.GetDownloadPermissionAsync(gallery.GalleryId, image.ImageId, ct).ConfigureAwait(false);
        if (allowed == DownloadPermission.None || permission > allowed)
        {
            return Forbid();
        }

        var result = await _downloads.GetImageDownloadAsync(image, gallery, permission, ct).ConfigureAwait(false);
        var cachePolicy = new CacheControlPolicy { MaxAgeSeconds = 3600, Immutable = true };
        Response.Headers.CacheControl = cachePolicy.ToHeaderValue();
        return File(result.Content, result.ContentType, result.FileName);
    }

    private static bool IsAllowedFileType(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".png", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".raw", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".cr2", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".nef", StringComparison.OrdinalIgnoreCase);
    }
}
