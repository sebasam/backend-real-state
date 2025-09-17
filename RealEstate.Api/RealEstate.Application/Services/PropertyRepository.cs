using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Mongo;
using RealEstate.Application.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using RealEstate.Application.Dtos;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<PropertyImage> _images;

    public PropertyRepository(MongoDbContext ctx)
    {
        _properties = ctx.GetCollection<Property>("Properties");
        _images = ctx.GetCollection<PropertyImage>("PropertyImages");
    }

    public async Task<List<PropertyWithImageDto>> GetAllAsync(
        string? name, string? address, decimal? minPrice, decimal? maxPrice)
    {
        // Filtros básicos
        var filter = Builders<Property>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(name))
            filter &= Builders<Property>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

        if (!string.IsNullOrWhiteSpace(address))
            filter &= Builders<Property>.Filter.Regex(x => x.Address, new MongoDB.Bson.BsonRegularExpression(address, "i"));

        if (minPrice.HasValue)
            filter &= Builders<Property>.Filter.Gte(x => x.Price, minPrice.Value);

        if (maxPrice.HasValue)
            filter &= Builders<Property>.Filter.Lte(x => x.Price, maxPrice.Value);

        var properties = await _properties.Find(filter).ToListAsync();

        var result = new List<PropertyWithImageDto>();
        foreach (var prop in properties)
        {
            var image = await _images
                .Find(x => x.PropertyId == prop.Id && x.Enabled)
                .SortBy(x => x.Id) 
                .FirstOrDefaultAsync();

            result.Add(new PropertyWithImageDto
            {
                Id = prop.Id,
                Name = prop.Name,
                Address = prop.Address,
                Price = prop.Price,
                OwnerId = prop.OwnerId,
                ImageUrl = image?.File
            });
        }

        return result;
    }

    public Task<Property?> GetByIdAsync(string id)
        => _properties.Find(x => x.Id == id).FirstOrDefaultAsync();

    public Task CreateAsync(Property property)
        => _properties.InsertOneAsync(property);
}
