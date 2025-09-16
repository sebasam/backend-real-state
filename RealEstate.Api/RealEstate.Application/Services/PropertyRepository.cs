using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Mongo;
using RealEstate.Application.Interfaces;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _collection;
    public PropertyRepository(MongoDbContext ctx)
    {
        _collection = ctx.GetCollection<Property>("Properties");
    }

    public async Task<List<Property>> GetAllAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
    {
        var filter = Builders<Property>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(name))
            filter &= Builders<Property>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

        if (!string.IsNullOrWhiteSpace(address))
            filter &= Builders<Property>.Filter.Regex(x => x.Address, new MongoDB.Bson.BsonRegularExpression(address, "i"));

        if (minPrice.HasValue)
            filter &= Builders<Property>.Filter.Gte(x => x.Price, minPrice.Value);

        if (maxPrice.HasValue)
            filter &= Builders<Property>.Filter.Lte(x => x.Price, maxPrice.Value);

        return await _collection.Find(filter).ToListAsync();
    }

    public Task<Property?> GetByIdAsync(string id)
        => _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public Task CreateAsync(Property property)
        => _collection.InsertOneAsync(property);
}
