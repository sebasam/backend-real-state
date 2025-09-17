using RealEstate.Application.Dtos;
using RealEstate.Application.DTOs;

public interface IPropertyService
{
    Task<List<PropertyWithImageDto>> GetFiltered(PropertyFilterDto filter);
    Task<PropertyWithImageDto?> GetByIdAsync(string id);
}
