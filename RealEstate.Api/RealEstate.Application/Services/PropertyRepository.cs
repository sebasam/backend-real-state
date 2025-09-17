using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Mongo;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Dtos;
using RealEstate.Application.DTOs;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<PropertyImage> _images;

    public PropertyRepository(MongoDbContext ctx)
    {
        _properties = ctx.GetCollection<Property>("Properties");
        _images = ctx.GetCollection<PropertyImage>("PropertyImages");
    }

    public async Task<List<PropertyWithImageDto>> GetAllAsync(PropertyFilterDto filter)
    {
        var mongoFilter = Builders<Property>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(filter.Name))
            mongoFilter &= Builders<Property>.Filter.Regex(
                x => x.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i"));

        if (!string.IsNullOrWhiteSpace(filter.Address))
            mongoFilter &= Builders<Property>.Filter.Regex(
                x => x.Address, new MongoDB.Bson.BsonRegularExpression(filter.Address, "i"));

        if (filter.MinPrice.HasValue)
            mongoFilter &= Builders<Property>.Filter.Gte(x => x.Price, filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            mongoFilter &= Builders<Property>.Filter.Lte(x => x.Price, filter.MaxPrice.Value);

        var properties = await _properties.Find(mongoFilter).ToListAsync();

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
