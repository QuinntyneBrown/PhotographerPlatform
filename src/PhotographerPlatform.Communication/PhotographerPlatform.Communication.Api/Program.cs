using System.Text.Json.Serialization;
using PhotographerPlatform.Communication.Core.Repositories;
using PhotographerPlatform.Communication.Core.Services;
using PhotographerPlatform.Communication.Infrastructure.Email;
using SendGrid;
using PhotographerPlatform.Communication.Infrastructure.Repositories;

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

builder.Services.AddSingleton<IEmailTemplateRepository, InMemoryEmailTemplateRepository>();
builder.Services.AddSingleton<IEmailLogRepository, InMemoryEmailLogRepository>();
builder.Services.AddSingleton<IMessageRepository, InMemoryMessageRepository>();
builder.Services.AddSingleton<INoteRepository, InMemoryNoteRepository>();
var sendGridOptions = builder.Configuration.GetSection("Email:SendGrid").Get<SendGridOptions>() ?? new SendGridOptions();
builder.Services.AddSingleton(sendGridOptions);
if (!string.IsNullOrWhiteSpace(sendGridOptions.ApiKey))
{
    builder.Services.AddSingleton(new SendGridClient(sendGridOptions.ApiKey));
    builder.Services.AddSingleton<IEmailSender, SendGridEmailSender>();
}
else
{
    builder.Services.AddSingleton<IEmailSender, InMemoryEmailSender>();
}
builder.Services.AddSingleton<ITemplateRenderer, SimpleTemplateRenderer>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IMessagingService, MessagingService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("DefaultCors");
app.MapControllers();
app.Run();
