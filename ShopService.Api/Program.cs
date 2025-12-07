using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShopService.Api.Extensions;
using ShopService.Application.Validation;
using ShopService.Infrastructure.Data;
using ShopService.Infrastructure.Stats;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ItemsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgre")));

builder.Services.AddSwaggerGen(c =>
{
    try
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop", Version = "v1" });
    }
    catch (Exception ex)
    {
        Console.WriteLine("Swagger ex: " + ex.Message);
        throw;
    }
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ShopService.Application.AssemblyMarker).Assembly));

// Fluent 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblies([
    typeof(CreateItemDtoValidator).Assembly, 
    typeof(UpdateItemDtoValidator).Assembly,
]);

builder.Services.Configure<StatsConfig>(_ => { });

builder.Services.AddControllers()
    .AddFluentValidation();

builder.Services.AddServices();

var app = builder.Build();
// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ItemsDbContext>();
    db.Database.Migrate();
}

app.Run();

