using API.BackgroundServices;
using API.Data.Contexts;
using API.Providers;
using API.Services;
using API.Utils;
using API.Utils.Middlewares;
using API.Utils.Settings;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CentralDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CentralDb")));

builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();
builder.Services.AddScoped<TenantDbDesignTimeFactory>();

builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<MigrationUpdateService>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GoogleAuthService>();

// Bind settings
IConfigurationSection jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);
JwtSettings? jwtSettings = jwtSection.Get<JwtSettings>();

// Register JwtHelper as singleton
builder.Services.AddSingleton(new JwtHelper(
    jwtSettings.SecretKey,
    jwtSettings.Issuer,
    jwtSettings.Audience,
    jwtSettings.ExpiresInMinutes
));


builder.Services.AddHostedService<MigrationBackgroundService>();

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RequestLogging>();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
