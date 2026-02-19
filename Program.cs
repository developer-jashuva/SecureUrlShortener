using SecureUrlShortener.Services;
using Microsoft.EntityFrameworkCore;
using SecureUrlShortener.Data;



var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UrlSafetyService>();
builder.Services.AddSingleton<ShortCodeGenerator>();
builder.Services.AddSingleton<UrlStoreService>();



var raw = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(raw))
    throw new InvalidOperationException("Connection string not found.");

// Convert Render postgres:// URL → Npgsql format
var uri = new Uri(raw);
var userInfo = uri.UserInfo.Split(':');

var npgsql =
    $"Host={uri.Host};" +
    $"Port={(uri.Port > 0 ? uri.Port : 5432)};" +
    $"Database={uri.AbsolutePath.TrimStart('/')};" +
    $"Username={userInfo[0]};" +
    $"Password={userInfo[1]};" +
    "SSL Mode=Require;Trust Server Certificate=true";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(npgsql));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseCors("AllowAll");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     db.Database.Migrate();
// }
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

app.Run();
