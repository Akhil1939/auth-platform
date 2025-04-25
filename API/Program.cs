using API.Data.Contexts;
using API.Providers;
using API.Services;
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

builder.Services.AddScoped<TenantService>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
