using API.BackgroundServices;
using API.Data.Contexts;
using API.Providers;
using API.Services;
using API.Utils;
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


builder.Services.AddHostedService<MigrationBackgroundService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
