using PhotographerPlatform.Galleries.Core.Models;

namespace PhotographerPlatform.Galleries.Api.Models;

public sealed class GalleryCreateRequest
{
    public required string ProjectId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset? EventDate { get; init; }
}

public sealed class GalleryUpdateRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset? EventDate { get; init; }
}

public sealed class GalleryCoverRequest
{
    public required string ImageId { get; init; }
}

public sealed class AccessLevelRequest
{
    public required GalleryAccessLevel Level { get; init; }
}

public sealed class PasswordRequest
{
    public required string Password { get; init; }
}

public sealed class AccessExpirationRequest
{
    public DateTimeOffset? ExpiresAt { get; init; }
}

public sealed class DownloadSettingsRequest
{
    public DownloadPermission Permission { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
    public int? WebSizeMaxWidth { get; init; }
    public int? WebSizeMaxHeight { get; init; }
    public int? WebSizeQuality { get; init; }
}

public sealed class WatermarkSettingsRequest
{
    public bool Enabled { get; init; }
    public string? WatermarkId { get; init; }
    public WatermarkPosition Position { get; init; } = WatermarkPosition.BottomRight;
    public double Opacity { get; init; } = 0.6;
    public double Scale { get; init; } = 0.2;
}

public sealed class ProofingSettingsRequest
{
    public bool Enabled { get; init; } = true;
    public int? SelectionLimit { get; init; }
    public DateTimeOffset? Deadline { get; init; }
}
