public interface IPropertyService
{
    Task<List<PropertyDto>> GetFiltered(string? name, string? address, decimal? minPrice, decimal? maxPrice);
}
