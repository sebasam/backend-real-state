using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces;

public interface IPropertyRepository
{
    Task<List<Property>> GetAllAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice);
    Task<Property?> GetByIdAsync(string id);
    Task CreateAsync(Property property);
}
