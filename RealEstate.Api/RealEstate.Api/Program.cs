using RealEstate.Domain.Entities;
using RealEstate.Application.Interfaces;
using RealEstate.Infrastructure.Mongo;
using RealEstate.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

// Registrar contexto Mongo
builder.Services.AddSingleton(sp =>
{
    var cfg = builder.Configuration.GetSection("MongoSettings");
    return new MongoDbContext(cfg["ConnectionString"]!, cfg["DatabaseName"]!);
});

// Registrar colecciones de Mongo
builder.Services.AddSingleton(sp =>
{
    var context = sp.GetRequiredService<MongoDbContext>();
    return context.GetCollection<Property>("Properties");
});
builder.Services.AddSingleton(sp =>
{
    var context = sp.GetRequiredService<MongoDbContext>();
    return context.GetCollection<PropertyImage>("PropertyImages");
});
// ?? NUEVO: registrar colección de Owners
builder.Services.AddSingleton(sp =>
{
    var context = sp.GetRequiredService<MongoDbContext>();
    return context.GetCollection<Owner>("Owners");
});

// Registrar repositorios y servicios
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyService, PropertyService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ejecutar seed solo en desarrollo
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

    var propertyCollection = context.GetCollection<Property>("Properties");
    var imageCollection = context.GetCollection<PropertyImage>("PropertyImages");
    var ownerCollection = context.GetCollection<Owner>("Owners"); // ?? nuevo

    await PropertySeed.SeedAsync(propertyCollection, imageCollection, ownerCollection);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();

public class MongoSettings
{
    public string ConnectionString { get; set; } = "";
    public string DatabaseName { get; set; } = "";
}
