using System.Text.Json.Serialization;
using PhotographerPlatform.Workspace.Core.Repositories;
using PhotographerPlatform.Workspace.Core.Services;
using PhotographerPlatform.Workspace.Infrastructure.Repositories;

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

builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
builder.Services.AddSingleton<IClientRepository, InMemoryClientRepository>();
builder.Services.AddSingleton<IClientNoteRepository, InMemoryClientNoteRepository>();
builder.Services.AddSingleton<ITagRepository, InMemoryTagRepository>();
builder.Services.AddSingleton<IWorkspaceService, WorkspaceService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("DefaultCors");
app.MapControllers();
app.Run();
