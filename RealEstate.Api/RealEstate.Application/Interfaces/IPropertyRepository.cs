using RealEstate.Application.Dtos;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces;

public interface IPropertyRepository
{
    Task<List<PropertyWithImageDto>> GetAllAsync(PropertyFilterDto filter);
    Task<Property?> GetByIdAsync(string id);
    Task CreateAsync(Property property);
}
