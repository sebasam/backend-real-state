using RealEstate.Application.Dtos;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using MongoDB.Driver;
using RealEstate.Application.DTOs;

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

    public async Task<List<PropertyWithImageDto>> GetFiltered(PropertyFilterDto filter)
    {
        var props = await _repo.GetAllAsync(filter);

        if (!props.Any())
            return new List<PropertyWithImageDto>();

        var propertyIds = props.Select(p => p.Id).ToList();

        var images = await _imagesCollection
            .Find(img => propertyIds.Contains(img.PropertyId) && img.Enabled)
            .ToListAsync();

        var ownerIds = props.Select(p => p.OwnerId).Distinct().ToList();

        var owners = await _ownersCollection
            .Find(o => ownerIds.Contains(o.Id))
            .ToListAsync();

        var result = props.Select(p => new PropertyWithImageDto
        {
            Id = p.Id,
            Name = p.Name,
            Address = p.Address,
            Price = p.Price,
            OwnerId = p.OwnerId,
            ImageUrl = images.FirstOrDefault(i => i.PropertyId == p.Id)?.File,
            OwnerName = owners.FirstOrDefault(o => o.Id == p.OwnerId)?.Name
        }).ToList();

        return result;
    }

    public async Task<PropertyWithImageDto?> GetByIdAsync(string id)
    {
        var property = await _repo.GetByIdAsync(id);
        if (property == null) return null;

        var image = await _imagesCollection
            .Find(img => img.PropertyId == property.Id && img.Enabled)
            .FirstOrDefaultAsync();

        var owner = await _ownersCollection
            .Find(o => o.Id == property.OwnerId)
            .FirstOrDefaultAsync();

        return new PropertyWithImageDto
        {
            Id = property.Id,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            OwnerId = property.OwnerId,
            OwnerName = owner?.Name,
            ImageUrl = image?.File
        };
    }

}
