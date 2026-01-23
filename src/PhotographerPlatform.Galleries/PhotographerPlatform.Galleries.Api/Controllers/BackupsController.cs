using Microsoft.AspNetCore.Mvc;
using Shared.Backup;

namespace PhotographerPlatform.Galleries.Api.Controllers;

[ApiController]
[Route("api/backups")]
public sealed class BackupsController : ControllerBase
{
    private readonly IBackupService _backupService;

    public BackupsController(IBackupService backupService)
    {
        _backupService = backupService;
    }

    [HttpPost]
    public async Task<ActionResult<BackupInfo>> Create([FromQuery] string serviceName, [FromQuery] BackupType type, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return BadRequest("serviceName is required.");
        }

        var backup = await _backupService.CreateBackupAsync(serviceName, type, ct).ConfigureAwait(false);
        return Ok(backup);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BackupInfo>>> List([FromQuery] string serviceName, [FromQuery] int limit = 10, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return BadRequest("serviceName is required.");
        }

        var backups = await _backupService.ListBackupsAsync(serviceName, limit, ct).ConfigureAwait(false);
        return Ok(backups);
    }

    [HttpPost("restore")]
    public async Task<ActionResult<RestoreResult>> Restore([FromBody] RestoreRequest request, CancellationToken ct)
    {
        var result = await _backupService.RestoreAsync(request, ct).ConfigureAwait(false);
        return Ok(result);
    }
}
