using PhotographerPlatform.Galleries.Core.Models;
using PhotographerPlatform.Galleries.Core.Services;
using PhotographerPlatform.Galleries.Infrastructure.Repositories;
using PhotographerPlatform.Galleries.Infrastructure.Security;
using Xunit;

namespace PhotographerPlatform.Galleries.Core.Tests;

public sealed class GalleriesServiceTests
{
    private static GalleriesService CreateService()
    {
        return new GalleriesService(
            new InMemoryGalleryRepository(),
            new InMemoryImageRepository(),
            new InMemoryImageSetRepository(),
            new InMemoryFavoriteRepository(),
            new InMemoryCommentRepository(),
            new InMemoryWatermarkRepository(),
            new Pbkdf2PasswordHasher());
    }

    [Fact]
    public async Task CreateGallery_SetsCoverWhenFirstImageAdded()
    {
        var service = CreateService();
        var gallery = await service.CreateGalleryAsync("project_1", "Gallery", null, null);

        var uploads = new List<ImageUploadDescriptor>
        {
            new()
            {
                FileName = "first.jpg",
                OriginalPath = "path/original.jpg"
            }
        };

        var images = await service.AddImagesAsync(gallery.GalleryId, uploads);
        var updated = await service.GetGalleryAsync(gallery.GalleryId);

        Assert.Single(images);
        Assert.Equal(images[0].ImageId, updated?.CoverImageId);
    }

    [Fact]
    public async Task ToggleFavorite_EnforcesSelectionLimit()
    {
        var service = CreateService();
        var gallery = await service.CreateGalleryAsync("project_1", "Gallery", null, null);
        gallery.Settings.Proofing.SelectionLimit = 1;

        var uploads = new List<ImageUploadDescriptor>
        {
            new()
            {
                FileName = "one.jpg",
                OriginalPath = "path/one.jpg"
            },
            new()
            {
                FileName = "two.jpg",
                OriginalPath = "path/two.jpg"
            }
        };

        var images = await service.AddImagesAsync(gallery.GalleryId, uploads);
        await service.ToggleFavoriteAsync(gallery.GalleryId, images[0].ImageId, "client_1");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await service.ToggleFavoriteAsync(gallery.GalleryId, images[1].ImageId, "client_1"));
    }

    [Fact]
    public async Task GetDownloadPermission_HonorsImageOverride()
    {
        var service = CreateService();
        var gallery = await service.CreateGalleryAsync("project_1", "Gallery", null, null);
        await service.SetDownloadSettingsAsync(gallery.GalleryId, new DownloadSettings { Permission = DownloadPermission.FullSize });

        var uploads = new List<ImageUploadDescriptor>
        {
            new()
            {
                FileName = "one.jpg",
                OriginalPath = "path/one.jpg"
            }
        };

        var images = await service.AddImagesAsync(gallery.GalleryId, uploads);
        images[0].DownloadOverride = DownloadPermission.WebSize;

        var permission = await service.GetDownloadPermissionAsync(gallery.GalleryId, images[0].ImageId);

        Assert.Equal(DownloadPermission.WebSize, permission);
    }

    [Fact]
    public async Task VerifyAccess_ReturnsTokenForValidPassword()
    {
        var service = CreateService();
        var gallery = await service.CreateGalleryAsync("project_1", "Gallery", null, null);

        await service.SetAccessPasswordAsync(gallery.GalleryId, "secret");
        var result = await service.VerifyAccessAsync(gallery.GalleryId, "secret");

        Assert.True(result.IsAuthorized);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
    }
}
