using RealEstate.Application.Interfaces;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _repo;

    public PropertyService(IPropertyRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<PropertyDto>> GetFiltered(string? name, string? address, decimal? minPrice, decimal? maxPrice)
    {
        var props = await _repo.GetAllAsync(name, address, minPrice, maxPrice);
        return props.Select(p => new PropertyDto
        {
            IdOwner = p.IdOwner,
            Name = p.Name,
            Address = p.Address,
            Price = p.Price,
            Image = ""
        }).ToList();
    }
}
