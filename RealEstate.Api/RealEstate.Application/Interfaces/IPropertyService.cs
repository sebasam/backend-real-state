using RealEstate.Application.Dtos;

public interface IPropertyService
{
    Task<List<PropertyWithImageDto>> GetFiltered(string? name, string? address, decimal? minPrice, decimal? maxPrice);
}
