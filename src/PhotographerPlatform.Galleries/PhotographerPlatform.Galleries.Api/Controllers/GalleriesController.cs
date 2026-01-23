using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Galleries.Api.Models;
using PhotographerPlatform.Galleries.Api.Services;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;
using PhotographerPlatform.Galleries.Core.Services;
using Shared.Cdn;

namespace PhotographerPlatform.Galleries.Api.Controllers;

[ApiController]
[Route("api/galleries")]
public sealed class GalleriesController : ControllerBase
{
    private readonly IGalleriesService _service;
    private readonly IImageRepository _images;
    private readonly IImageSetRepository _sets;
    private readonly IDownloadService _downloads;
    private readonly ImageUrlBuilder _urlBuilder;

    public GalleriesController(
        IGalleriesService service,
        IImageRepository images,
        IImageSetRepository sets,
        IDownloadService downloads,
        ImageUrlBuilder urlBuilder)
    {
        _service = service;
        _images = images;
        _sets = sets;
        _downloads = downloads;
        _urlBuilder = urlBuilder;
    }

    [HttpPost]
    public async Task<ActionResult<GalleryResponse>> Create([FromBody] GalleryCreateRequest request, CancellationToken ct)
    {
        var gallery = await _service.CreateGalleryAsync(request.ProjectId, request.Name, request.Description, request.EventDate, ct)
            .ConfigureAwait(false);
        return Ok(await BuildGalleryResponseAsync(gallery, ct).ConfigureAwait(false));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GalleryResponse>> GetById(string id, CancellationToken ct)
    {
        var gallery = await _service.GetGalleryAsync(id, ct).ConfigureAwait(false);
        if (gallery is null)
        {
            return NotFound();
        }

        return Ok(await BuildGalleryResponseAsync(gallery, ct).ConfigureAwait(false));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GalleryResponse>> Update(string id, [FromBody] GalleryUpdateRequest request, CancellationToken ct)
    {
        var gallery = await _service.UpdateGalleryAsync(id, request.Name, request.Description, request.EventDate, ct)
            .ConfigureAwait(false);
        return Ok(await BuildGalleryResponseAsync(gallery, ct).ConfigureAwait(false));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _service.DeleteGalleryAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPut("{id}/cover")]
    public async Task<IActionResult> SetCover(string id, [FromBody] GalleryCoverRequest request, CancellationToken ct)
    {
        await _service.SetCoverImageAsync(id, request.ImageId, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPut("{id}/access")]
    public async Task<IActionResult> SetAccess(string id, [FromBody] AccessLevelRequest request, CancellationToken ct)
    {
        await _service.SetAccessLevelAsync(id, request.Level, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPut("{id}/password")]
    public async Task<IActionResult> SetPassword(string id, [FromBody] PasswordRequest request, CancellationToken ct)
    {
        await _service.SetAccessPasswordAsync(id, request.Password, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPost("{id}/verify-password")]
    public async Task<ActionResult<AccessVerificationResult>> VerifyPassword(string id, [FromBody] PasswordRequest request, CancellationToken ct)
    {
        var result = await _service.VerifyAccessAsync(id, request.Password, ct).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("{id}/expiration")]
    public async Task<IActionResult> SetExpiration(string id, [FromBody] AccessExpirationRequest request, CancellationToken ct)
    {
        await _service.SetAccessExpirationAsync(id, request.ExpiresAt, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPut("{id}/download-settings")]
    public async Task<IActionResult> SetDownloadSettings(string id, [FromBody] DownloadSettingsRequest request, CancellationToken ct)
    {
        var settings = new DownloadSettings
        {
            Permission = request.Permission,
            ExpiresAtUnixMs = request.ExpiresAt?.ToUnixTimeMilliseconds(),
            WebSizeMaxWidth = request.WebSizeMaxWidth ?? 2048,
            WebSizeMaxHeight = request.WebSizeMaxHeight ?? 2048,
            WebSizeQuality = request.WebSizeQuality ?? 85
        };

        await _service.SetDownloadSettingsAsync(id, settings, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpGet("{id}/download-status")]
    public async Task<ActionResult<object>> GetDownloadStatus(string id, [FromQuery] string imageId, CancellationToken ct)
    {
        var permission = await _service.GetDownloadPermissionAsync(id, imageId, ct).ConfigureAwait(false);
        return Ok(new { permission });
    }

    [HttpPost("{id}/download")]
    public async Task<IActionResult> DownloadGallery(string id, [FromQuery] DownloadPermission permission, CancellationToken ct)
    {
        var gallery = await _service.GetGalleryAsync(id, ct).ConfigureAwait(false);
        if (gallery is null)
        {
            return NotFound();
        }

        var images = await _images.ListByGalleryAsync(id, ct).ConfigureAwait(false);
        if (permission == DownloadPermission.None)
        {
            return Forbid();
        }

        var result = await _downloads.CreateGalleryZipAsync(gallery, images, permission, ct).ConfigureAwait(false);
        return File(result.Content, result.ContentType, result.FileName);
    }

    [HttpGet("{id}/sets")]
    public async Task<ActionResult<IReadOnlyList<ImageSet>>> ListSets(string id, CancellationToken ct)
    {
        var sets = await _sets.ListByGalleryAsync(id, ct).ConfigureAwait(false);
        return Ok(sets);
    }

    [HttpPost("{id}/sets")]
    public async Task<ActionResult<ImageSet>> CreateSet(string id, [FromBody] ImageSetCreateRequest request, CancellationToken ct)
    {
        var set = await _service.CreateImageSetAsync(id, request.Name, request.ImageIds, ct).ConfigureAwait(false);
        return Ok(set);
    }

    [HttpPut("/api/sets/{id}/images")]
    public async Task<IActionResult> UpdateSetImages(string id, [FromBody] ImageSetUpdateRequest request, CancellationToken ct)
    {
        await _service.UpdateImageSetImagesAsync(id, request.ImageIds, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPut("{id}/watermark")]
    public async Task<IActionResult> SetWatermark(string id, [FromBody] WatermarkSettingsRequest request, CancellationToken ct)
    {
        var settings = new WatermarkSettings
        {
            Enabled = request.Enabled,
            WatermarkId = request.WatermarkId,
            Position = request.Position,
            Opacity = request.Opacity,
            Scale = request.Scale
        };

        await _service.SetGalleryWatermarkAsync(id, settings, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpPut("{id}/proofing")]
    public async Task<IActionResult> SetProofing(string id, [FromBody] ProofingUpdateRequest request, CancellationToken ct)
    {
        var gallery = await _service.GetGalleryAsync(id, ct).ConfigureAwait(false);
        if (gallery is null)
        {
            return NotFound();
        }

        var settings = new ProofingSettings
        {
            Enabled = request.Enabled ?? gallery.Settings.Proofing.Enabled,
            SelectionLimit = request.SelectionLimit ?? gallery.Settings.Proofing.SelectionLimit,
            DeadlineUnixMs = request.Deadline?.ToUnixTimeMilliseconds() ?? gallery.Settings.Proofing.DeadlineUnixMs
        };

        await _service.SetProofingSettingsAsync(id, settings, ct).ConfigureAwait(false);
        return NoContent();
    }

    [HttpGet("{id}/favorites")]
    public async Task<ActionResult<IReadOnlyList<Favorite>>> ListFavorites(string id, [FromQuery] string clientId, CancellationToken ct)
    {
        var favorites = await _service.ListFavoritesAsync(id, clientId, ct).ConfigureAwait(false);
        return Ok(favorites);
    }

    private async Task<GalleryResponse> BuildGalleryResponseAsync(Gallery gallery, CancellationToken ct)
    {
        var images = await _images.ListByGalleryAsync(gallery.GalleryId, ct).ConfigureAwait(false);
        var response = new GalleryResponse
        {
            GalleryId = gallery.GalleryId,
            ProjectId = gallery.ProjectId,
            Name = gallery.Name,
            Description = gallery.Description,
            EventDate = gallery.EventDate,
            CoverImageId = gallery.CoverImageId,
            Status = gallery.Status.ToString(),
            Settings = gallery.Settings,
            Images = images.Select(BuildImageResponse).ToList()
        };
        return response;
    }

    private ImageResponse BuildImageResponse(Image image)
    {
        var variants = new List<ImageVariantResponse>
        {
            new()
            {
                Url = _urlBuilder.BuildImageUrl(image.ImageId, new ImageTransformOptions { Width = 400, Height = 300, Format = ImageFormat.WebP }),
                Width = 400,
                Format = "WebP"
            },
            new()
            {
                Url = _urlBuilder.BuildImageUrl(image.ImageId, new ImageTransformOptions { Width = 800, Height = 600, Format = ImageFormat.WebP }),
                Width = 800,
                Format = "WebP"
            }
        };

        return new ImageResponse
        {
            ImageId = image.ImageId,
            FileName = image.FileName,
            Order = image.Order,
            OriginalUrl = Url.Action("DownloadImage", "Images", new { id = image.ImageId, mode = "full" }, Request.Scheme),
            WebUrl = Url.Action("DownloadImage", "Images", new { id = image.ImageId, mode = "web" }, Request.Scheme),
            ThumbnailSmallUrl = image.ThumbnailSmallPath,
            ThumbnailMediumUrl = image.ThumbnailMediumPath,
            ThumbnailLargeUrl = image.ThumbnailLargePath,
            Variants = variants,
            UploadedAtUnixMs = image.UploadedAtUnixMs
        };
    }
}
