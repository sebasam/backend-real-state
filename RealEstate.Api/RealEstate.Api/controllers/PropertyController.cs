using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Dtos;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _service;

    public PropertyController(IPropertyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PropertyFilterDto filter)
    {
        var result = await _service.GetFiltered(filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("El Id de la propiedad no puede estar vacío.");

        var property = await _service.GetByIdAsync(id);

        if (property == null)
            return NotFound($"No se encontró la propiedad con Id: {id}");

        return Ok(property);
    }
}
