using System.Security.Cryptography;
using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Repositories;

namespace PhotographerPlatform.Galleries.Core.Services;

public sealed class GalleriesService : IGalleriesService
{
    private readonly IGalleryRepository _galleries;
    private readonly IImageRepository _images;
    private readonly IImageSetRepository _sets;
    private readonly IFavoriteRepository _favorites;
    private readonly ICommentRepository _comments;
    private readonly IWatermarkRepository _watermarks;
    private readonly IPasswordHasher _passwordHasher;

    public GalleriesService(
        IGalleryRepository galleries,
        IImageRepository images,
        IImageSetRepository sets,
        IFavoriteRepository favorites,
        ICommentRepository comments,
        IWatermarkRepository watermarks,
        IPasswordHasher passwordHasher)
    {
        _galleries = galleries;
        _images = images;
        _sets = sets;
        _favorites = favorites;
        _comments = comments;
        _watermarks = watermarks;
        _passwordHasher = passwordHasher;
    }

    public async Task<Gallery> CreateGalleryAsync(string projectId, string name, string? description, DateTimeOffset? eventDate, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var gallery = new Gallery
        {
            GalleryId = Guid.NewGuid().ToString("N"),
            ProjectId = projectId,
            Name = name,
            Description = description,
            EventDate = eventDate,
            Status = GalleryStatus.Active,
            Settings = new GallerySettings(),
            CreatedAtUnixMs = now,
            UpdatedAtUnixMs = now
        };

        await _galleries.AddAsync(gallery, ct).ConfigureAwait(false);
        return gallery;
    }

    public Task<Gallery?> GetGalleryAsync(string galleryId, CancellationToken ct = default)
    {
        return _galleries.GetByIdAsync(galleryId, ct);
    }

    public async Task<Gallery> UpdateGalleryAsync(string galleryId, string name, string? description, DateTimeOffset? eventDate, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Name = name;
        gallery.Description = description;
        gallery.EventDate = eventDate;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
        return gallery;
    }

    public async Task ArchiveGalleryAsync(string galleryId, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Status = GalleryStatus.Archived;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    public async Task DeleteGalleryAsync(string galleryId, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Status = GalleryStatus.Deleted;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
        await _galleries.DeleteAsync(galleryId, ct).ConfigureAwait(false);
    }

    public async Task SetCoverImageAsync(string galleryId, string imageId, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.CoverImageId = imageId;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Image>> AddImagesAsync(string galleryId, IReadOnlyList<ImageUploadDescriptor> uploads, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var existing = await _images.ListByGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var orderBase = existing.Count;
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var images = new List<Image>();

        for (var i = 0; i < uploads.Count; i++)
        {
            var upload = uploads[i];
            images.Add(new Image
            {
                ImageId = Guid.NewGuid().ToString("N"),
                GalleryId = galleryId,
                FileName = upload.FileName,
                OriginalPath = upload.OriginalPath,
                WebPath = upload.WebPath,
                ThumbnailSmallPath = upload.ThumbnailSmallPath,
                ThumbnailMediumPath = upload.ThumbnailMediumPath,
                ThumbnailLargePath = upload.ThumbnailLargePath,
                Metadata = upload.Metadata,
                Order = orderBase + i,
                UploadedAtUnixMs = now
            });
        }

        await _images.AddRangeAsync(images, ct).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(gallery.CoverImageId) && images.Count > 0)
        {
            gallery.CoverImageId = images[0].ImageId;
            gallery.UpdatedAtUnixMs = now;
            await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
        }

        return images;
    }

    public async Task ReorderImagesAsync(string galleryId, IReadOnlyList<string> orderedImageIds, CancellationToken ct = default)
    {
        var images = await _images.ListByGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var map = images.ToDictionary(image => image.ImageId, image => image);
        var updated = new List<Image>();
        for (var i = 0; i < orderedImageIds.Count; i++)
        {
            if (!map.TryGetValue(orderedImageIds[i], out var image))
            {
                continue;
            }

            image.Order = i;
            updated.Add(image);
        }

        await _images.UpdateRangeAsync(updated, ct).ConfigureAwait(false);
    }

    public Task DeleteImageAsync(string imageId, CancellationToken ct = default)
    {
        return _images.DeleteAsync(imageId, ct);
    }

    public async Task UpdateImageMetadataAsync(string imageId, ImageMetadata metadata, CancellationToken ct = default)
    {
        var image = await RequireImageAsync(imageId, ct).ConfigureAwait(false);
        image.Metadata = metadata;
        await _images.UpdateAsync(image, ct).ConfigureAwait(false);
    }

    public async Task<ImageSet> CreateImageSetAsync(string galleryId, string name, IReadOnlyList<string> imageIds, CancellationToken ct = default)
    {
        _ = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var existing = await _sets.ListByGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var imageSet = new ImageSet
        {
            ImageSetId = Guid.NewGuid().ToString("N"),
            GalleryId = galleryId,
            Name = name,
            ImageIds = imageIds.ToList(),
            Order = existing.Count,
            CreatedAtUnixMs = now
        };

        await _sets.AddAsync(imageSet, ct).ConfigureAwait(false);
        return imageSet;
    }

    public async Task UpdateImageSetImagesAsync(string setId, IReadOnlyList<string> imageIds, CancellationToken ct = default)
    {
        var set = await RequireImageSetAsync(setId, ct).ConfigureAwait(false);
        set.ImageIds = imageIds.ToList();
        await _sets.UpdateAsync(set, ct).ConfigureAwait(false);
    }

    public Task DeleteImageSetAsync(string setId, CancellationToken ct = default)
    {
        return _sets.DeleteAsync(setId, ct);
    }

    public async Task<FavoriteToggleResult> ToggleFavoriteAsync(string galleryId, string imageId, string clientId, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var existing = await _favorites.GetAsync(galleryId, imageId, clientId, ct).ConfigureAwait(false);
        if (existing is null)
        {
            if (gallery.Settings.Proofing.SelectionLimit.HasValue)
            {
                var favorites = await _favorites.ListByGalleryAsync(galleryId, clientId, ct).ConfigureAwait(false);
                if (favorites.Count >= gallery.Settings.Proofing.SelectionLimit.Value)
                {
                    throw new InvalidOperationException("Selection limit reached.");
                }
            }

            var favorite = new Favorite
            {
                FavoriteId = Guid.NewGuid().ToString("N"),
                GalleryId = galleryId,
                ImageId = imageId,
                ClientId = clientId,
                CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            await _favorites.AddAsync(favorite, ct).ConfigureAwait(false);
        }
        else
        {
            await _favorites.DeleteAsync(existing.FavoriteId, ct).ConfigureAwait(false);
        }

        var count = await _favorites.CountByImageAsync(imageId, ct).ConfigureAwait(false);
        return new FavoriteToggleResult
        {
            IsFavorited = existing is null,
            FavoriteCount = count
        };
    }

    public Task<IReadOnlyList<Favorite>> ListFavoritesAsync(string galleryId, string clientId, CancellationToken ct = default)
    {
        return _favorites.ListByGalleryAsync(galleryId, clientId, ct);
    }

    public async Task<Comment> AddCommentAsync(string imageId, string clientId, string content, string? parentCommentId, CancellationToken ct = default)
    {
        _ = await RequireImageAsync(imageId, ct).ConfigureAwait(false);
        var comment = new Comment
        {
            CommentId = Guid.NewGuid().ToString("N"),
            ImageId = imageId,
            ClientId = clientId,
            Content = content,
            ParentCommentId = parentCommentId,
            CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await _comments.AddAsync(comment, ct).ConfigureAwait(false);
        return comment;
    }

    public Task<IReadOnlyList<Comment>> ListCommentsAsync(string imageId, CancellationToken ct = default)
    {
        return _comments.ListByImageAsync(imageId, ct);
    }

    public Task DeleteCommentAsync(string commentId, CancellationToken ct = default)
    {
        return _comments.DeleteAsync(commentId, ct);
    }

    public async Task SetAccessLevelAsync(string galleryId, GalleryAccessLevel level, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Settings.Access.Level = level;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    public async Task SetAccessPasswordAsync(string galleryId, string password, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var salt = _passwordHasher.GenerateSalt();
        var hash = _passwordHasher.Hash(password, salt);
        gallery.Settings.Access.Level = GalleryAccessLevel.PasswordProtected;
        gallery.Settings.Access.PasswordSalt = salt;
        gallery.Settings.Access.PasswordHash = hash;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    public async Task<AccessVerificationResult> VerifyAccessAsync(string galleryId, string password, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var access = gallery.Settings.Access;
        if (access.Level != GalleryAccessLevel.PasswordProtected || string.IsNullOrWhiteSpace(access.PasswordSalt) || string.IsNullOrWhiteSpace(access.PasswordHash))
        {
            return new AccessVerificationResult { IsAuthorized = false };
        }

        if (access.ExpiresAtUnixMs.HasValue && access.ExpiresAtUnixMs.Value <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        {
            return new AccessVerificationResult { IsAuthorized = false };
        }

        var verified = _passwordHasher.Verify(password, access.PasswordSalt, access.PasswordHash);
        if (!verified)
        {
            return new AccessVerificationResult { IsAuthorized = false };
        }

        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();
        gallery.Settings.Access.AccessToken = token;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);

        return new AccessVerificationResult
        {
            IsAuthorized = true,
            AccessToken = token
        };
    }

    public async Task SetAccessExpirationAsync(string galleryId, DateTimeOffset? expiresAt, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Settings.Access.ExpiresAtUnixMs = expiresAt?.ToUnixTimeMilliseconds();
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    public async Task SetDownloadSettingsAsync(string galleryId, DownloadSettings settings, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Settings.Downloads = settings;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    public async Task SetProofingSettingsAsync(string galleryId, ProofingSettings settings, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Settings.Proofing = settings;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    public async Task<DownloadPermission> GetDownloadPermissionAsync(string galleryId, string imageId, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        var image = await RequireImageAsync(imageId, ct).ConfigureAwait(false);
        if (image.GalleryId != gallery.GalleryId)
        {
            throw new InvalidOperationException("Image does not belong to gallery.");
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var gallerySettings = gallery.Settings.Downloads;
        if (gallerySettings.ExpiresAtUnixMs.HasValue && gallerySettings.ExpiresAtUnixMs <= now)
        {
            return DownloadPermission.None;
        }

        if (image.DownloadExpiresAtUnixMs.HasValue && image.DownloadExpiresAtUnixMs <= now)
        {
            return DownloadPermission.None;
        }

        if (image.DownloadOverride.HasValue)
        {
            return image.DownloadOverride.Value;
        }

        return gallerySettings.Permission;
    }

    public async Task<Watermark> UploadWatermarkAsync(string accountId, string fileName, string storagePath, CancellationToken ct = default)
    {
        var watermark = new Watermark
        {
            WatermarkId = Guid.NewGuid().ToString("N"),
            AccountId = accountId,
            FileName = fileName,
            StoragePath = storagePath,
            CreatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        await _watermarks.AddAsync(watermark, ct).ConfigureAwait(false);
        return watermark;
    }

    public Task<IReadOnlyList<Watermark>> ListWatermarksAsync(string accountId, CancellationToken ct = default)
    {
        return _watermarks.ListByAccountAsync(accountId, ct);
    }

    public Task DeleteWatermarkAsync(string watermarkId, CancellationToken ct = default)
    {
        return _watermarks.DeleteAsync(watermarkId, ct);
    }

    public async Task SetGalleryWatermarkAsync(string galleryId, WatermarkSettings settings, CancellationToken ct = default)
    {
        var gallery = await RequireGalleryAsync(galleryId, ct).ConfigureAwait(false);
        gallery.Settings.Watermark = settings;
        gallery.UpdatedAtUnixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _galleries.UpdateAsync(gallery, ct).ConfigureAwait(false);
    }

    private async Task<Gallery> RequireGalleryAsync(string galleryId, CancellationToken ct)
    {
        var gallery = await _galleries.GetByIdAsync(galleryId, ct).ConfigureAwait(false);
        if (gallery is null)
        {
            throw new InvalidOperationException("Gallery not found.");
        }

        return gallery;
    }

    private async Task<Image> RequireImageAsync(string imageId, CancellationToken ct)
    {
        var image = await _images.GetByIdAsync(imageId, ct).ConfigureAwait(false);
        if (image is null)
        {
            throw new InvalidOperationException("Image not found.");
        }

        return image;
    }

    private async Task<ImageSet> RequireImageSetAsync(string setId, CancellationToken ct)
    {
        var set = await _sets.GetByIdAsync(setId, ct).ConfigureAwait(false);
        if (set is null)
        {
            throw new InvalidOperationException("Image set not found.");
        }

        return set;
    }
}
