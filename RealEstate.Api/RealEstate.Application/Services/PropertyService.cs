using RealEstate.Application.Dtos;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using MongoDB.Driver;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _repo;
    private readonly IMongoCollection<PropertyImage> _imagesCollection;
    private readonly IMongoCollection<Owner> _ownersCollection;

    public PropertyService(
        IPropertyRepository repo,
        IMongoCollection<PropertyImage> imagesCollection,
        IMongoCollection<Owner> ownersCollection)
    {
        _repo = repo;
        _imagesCollection = imagesCollection;
        _ownersCollection = ownersCollection;
    }

    public async Task<List<PropertyWithImageDto>> GetFiltered(
        string? name, string? address, decimal? minPrice, decimal? maxPrice)
    {
        var props = await _repo.GetAllAsync(name, address, minPrice, maxPrice);

        if (!props.Any())
            return new List<PropertyWithImageDto>();

        var propertyIds = props.Select(p => p.Id).ToList();

        var images = await _imagesCollection
            .Find(img => propertyIds.Contains(img.PropertyId) && img.Enabled)
            .ToListAsync();

        // 👇 Obtener todos los owners involucrados
        var ownerIds = props.Select(p => p.OwnerId).Distinct().ToList();
        var owners = await _ownersCollection
            .Find(o => ownerIds.Contains(o.Id))
            .ToListAsync();

        var ownerMap = owners.ToDictionary(o => o.Id, o => o.Name);

        var result = props.Select(p =>
        {
            var img = images.FirstOrDefault(i => i.PropertyId == p.Id);
            return new PropertyWithImageDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Price = p.Price,
                OwnerId = p.OwnerId,
                OwnerName = ownerMap.ContainsKey(p.OwnerId) ? ownerMap[p.OwnerId] : "Desconocido",
                ImageUrl = img?.File ?? ""
            };
        }).ToList();

        return result;
    }
}
