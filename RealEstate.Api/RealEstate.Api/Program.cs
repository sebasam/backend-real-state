using RealEstate.Infrastructure.Mongo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddSingleton(sp =>
{
    var cfg = builder.Configuration.GetSection("MongoSettings");
    return new MongoDbContext(cfg["ConnectionString"]!, cfg["DatabaseName"]!);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();

public class MongoSettings
{
    public string ConnectionString { get; set; } = "";
    public string DatabaseName { get; set; } = "";
}
