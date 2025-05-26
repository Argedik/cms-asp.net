using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using CmsApi.Data;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

// .env dosyasından URL yapılandırmasını al
var serverHost = Environment.GetEnvironmentVariable("SERVER_HOST") ?? "localhost";
var serverPort = Environment.GetEnvironmentVariable("SERVER_PORT") ?? "8080";
var httpsPort = Environment.GetEnvironmentVariable("HTTPS_PORT") ?? "8081";

// URL'leri ayarla
var urls = $"http://{serverHost}:{serverPort}";
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HTTPS_PORT")))
{
    urls += $";https://{serverHost}:{httpsPort}";
}

builder.WebHost.UseUrls(urls);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Environment Variables
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                      $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                      $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                      $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
var jwtExpiration = Environment.GetEnvironmentVariable("JWT_EXPIRATION");

// Entity Framework Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Sunucu başlangıç bilgisi
Console.WriteLine($"🚀 CMS API başlatılıyor...");
Console.WriteLine($"📍 HTTP URL: http://{serverHost}:{serverPort}");
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HTTPS_PORT")))
{
    Console.WriteLine($"🔒 HTTPS URL: https://{serverHost}:{httpsPort}");
}
Console.WriteLine($"📚 Swagger UI: http://{serverHost}:{serverPort}/swagger");

app.Run();

