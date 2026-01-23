using System.Text.Json.Serialization;
using PhotographerPlatform.Galleries.Api.Services;
using PhotographerPlatform.Galleries.Core.Repositories;
using PhotographerPlatform.Galleries.Core.Services;
using PhotographerPlatform.Galleries.Infrastructure.Processing;
using PhotographerPlatform.Galleries.Infrastructure.Repositories;
using PhotographerPlatform.Galleries.Infrastructure.Security;
using PhotographerPlatform.Galleries.Infrastructure.Storage;
using PhotographerPlatform.Galleries.Infrastructure.Backup;
using Shared.Backup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IGalleryRepository, InMemoryGalleryRepository>();
builder.Services.AddSingleton<IImageRepository, InMemoryImageRepository>();
builder.Services.AddSingleton<IImageSetRepository, InMemoryImageSetRepository>();
builder.Services.AddSingleton<IFavoriteRepository, InMemoryFavoriteRepository>();
builder.Services.AddSingleton<ICommentRepository, InMemoryCommentRepository>();
builder.Services.AddSingleton<IWatermarkRepository, InMemoryWatermarkRepository>();
builder.Services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
builder.Services.AddSingleton<IImageProcessingService, NoOpImageProcessingService>();

builder.Services.AddSingleton<IStorageService>(_ =>
{
    var root = builder.Configuration.GetValue<string>("Storage:RootPath");
    if (string.IsNullOrWhiteSpace(root))
    {
        root = Path.Combine(AppContext.BaseDirectory, "storage");
    }

    return new FileSystemStorageService(root);
});
builder.Services.AddSingleton<IDownloadService, FileSystemDownloadService>();
builder.Services.AddSingleton<IGalleriesService, GalleriesService>();
builder.Services.AddSingleton<IBackupService, InMemoryBackupService>();
builder.Services.AddSingleton(_ =>
{
    var baseUrl = builder.Configuration.GetValue<string>("Cdn:BaseUrl") ?? "https://cdn.local";
    var secret = builder.Configuration.GetValue<string>("Cdn:SignedUrlSecret");
    var ttlMinutes = builder.Configuration.GetValue("Cdn:SignedUrlMinutes", 60);
    return new ImageUrlBuilder(baseUrl, secret, TimeSpan.FromMinutes(ttlMinutes));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("DefaultCors");
app.MapControllers();
app.Run();
