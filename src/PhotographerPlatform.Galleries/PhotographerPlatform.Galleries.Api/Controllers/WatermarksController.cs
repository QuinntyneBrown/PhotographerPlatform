using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Services;

namespace PhotographerPlatform.Galleries.Api.Controllers;

[ApiController]
[Route("api/watermarks")]
public sealed class WatermarksController : ControllerBase
{
    private readonly IGalleriesService _service;
    private readonly IStorageService _storage;

    public WatermarksController(IGalleriesService service, IStorageService storage)
    {
        _service = service;
        _storage = storage;
    }

    [HttpPost]
    public async Task<ActionResult<Watermark>> Upload([FromQuery] string accountId, [FromForm] IFormFile file, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(accountId))
        {
            return BadRequest("accountId is required.");
        }

        await using var stream = file.OpenReadStream();
        var stored = await _storage.SaveAsync(stream, file.FileName, "watermarks", file.ContentType, ct)
            .ConfigureAwait(false);
        var watermark = await _service.UploadWatermarkAsync(accountId, file.FileName, stored.Path, ct)
            .ConfigureAwait(false);
        return Ok(watermark);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Watermark>>> List([FromQuery] string accountId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(accountId))
        {
            return BadRequest("accountId is required.");
        }

        var watermarks = await _service.ListWatermarksAsync(accountId, ct).ConfigureAwait(false);
        return Ok(watermarks);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _service.DeleteWatermarkAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
