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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseCors("AllowAngular");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
