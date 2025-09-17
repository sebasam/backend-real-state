using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Dtos;
using RealEstate.Application.Interfaces;
using RealEstate.Application.DTOs;

namespace RealEstate.Tests;

[TestFixture]
public class PropertyControllerTests
{
    private Mock<IPropertyService> _serviceMock = null!;
    private PropertyController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IPropertyService>();
        _controller = new PropertyController(_serviceMock.Object);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOk_WithResults()
    {
        // Arrange
        var properties = new List<PropertyWithImageDto>
        {
            new PropertyWithImageDto
            {
                Id = "1",
                Name = "Casa",
                Address = "Calle 1",
                Price = 100,
                OwnerId = "o1",
                OwnerName = "Juan",
                ImageUrl = "img.jpg"
            }
        };

        var filter = new PropertyFilterDto(); // filtros vacíos

        _serviceMock.Setup(s => s.GetFiltered(filter))
                    .ReturnsAsync(properties);

        // Act
        var result = await _controller.Get(filter) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        var list = result.Value as List<PropertyWithImageDto>;
        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(1));
        Assert.That(list[0].Name, Is.EqualTo("Casa"));
    }

    [Test]
    public async Task GetFiltered_ShouldReturnOk_WithEmptyList()
    {
        // Arrange
        var filter = new PropertyFilterDto();
        _serviceMock.Setup(s => s.GetFiltered(filter))
                    .ReturnsAsync(new List<PropertyWithImageDto>());

        // Act
        var result = await _controller.Get(filter) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        var list = result.Value as List<PropertyWithImageDto>;
        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(0));
    }
}
