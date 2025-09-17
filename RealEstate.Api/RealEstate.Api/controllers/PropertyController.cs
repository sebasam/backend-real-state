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
}
