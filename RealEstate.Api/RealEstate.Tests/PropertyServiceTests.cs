using NUnit.Framework;
using Moq;
using MongoDB.Driver;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Dtos;
using RealEstate.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RealEstate.Application.DTOs;

namespace RealEstate.Tests;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _repoMock = null!;
    private Mock<IMongoCollection<PropertyImage>> _imagesMock = null!;
    private Mock<IMongoCollection<Owner>> _ownersMock = null!;
    private PropertyService _service = null!;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IPropertyRepository>();
        _imagesMock = new Mock<IMongoCollection<PropertyImage>>();
        _ownersMock = new Mock<IMongoCollection<Owner>>();

        _service = new PropertyService(_repoMock.Object, _imagesMock.Object, _ownersMock.Object);
    }

    [Test]
    public async Task GetFiltered_ShouldReturnEmptyList_WhenRepoReturnsNone()
    {
        var filter = new PropertyFilterDto();

        _repoMock.Setup(r => r.GetAllAsync(filter))
                 .ReturnsAsync(new List<PropertyWithImageDto>());

        var result = await _service.GetFiltered(filter);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetFiltered_ShouldCombineImageAndOwnerCorrectly()
    {
        var filter = new PropertyFilterDto();

        var property = new PropertyWithImageDto
        {
            Id = "p1",
            Name = "Casa",
            Address = "Calle 1",
            Price = 100,
            OwnerId = "o1"
        };

        _repoMock.Setup(r => r.GetAllAsync(filter))
                 .ReturnsAsync(new List<PropertyWithImageDto> { property });

        var image = new PropertyImage { PropertyId = "p1", File = "file.jpg", Enabled = true };
        var owner = new Owner { Id = "o1", Name = "Juan" };

        var cursorImages = MockCursor(new List<PropertyImage> { image });
        _imagesMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<PropertyImage>>(),
                                           It.IsAny<FindOptions<PropertyImage, PropertyImage>>(),
                                           default))
                   .ReturnsAsync(cursorImages);

        var cursorOwners = MockCursor(new List<Owner> { owner });
        _ownersMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Owner>>(),
                                           It.IsAny<FindOptions<Owner, Owner>>(),
                                           default))
                   .ReturnsAsync(cursorOwners);

        var result = await _service.GetFiltered(filter);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].OwnerName, Is.EqualTo("Juan"));
        Assert.That(result[0].ImageUrl, Is.EqualTo("file.jpg"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnPropertyWithImageAndOwner()
    {
        var propertyId = "p1";
        var property = new Property
        {
            Id = propertyId,
            Name = "Casa",
            Address = "Calle 1",
            Price = 100,
            OwnerId = "o1"
        };

        _repoMock.Setup(r => r.GetByIdAsync(propertyId))
                 .ReturnsAsync(property);

        var image = new PropertyImage { PropertyId = propertyId, File = "file.jpg", Enabled = true };
        var owner = new Owner { Id = "o1", Name = "Juan" };

        var cursorImages = MockCursor(new List<PropertyImage> { image });
        _imagesMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<PropertyImage>>(),
                                           It.IsAny<FindOptions<PropertyImage, PropertyImage>>(),
                                           default))
                   .ReturnsAsync(cursorImages);

        var cursorOwners = MockCursor(new List<Owner> { owner });
        _ownersMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Owner>>(),
                                           It.IsAny<FindOptions<Owner, Owner>>(),
                                           default))
                   .ReturnsAsync(cursorOwners);

        var result = await _service.GetByIdAsync(propertyId);

        Assert.That(result!.Id, Is.EqualTo(propertyId));
        Assert.That(result.OwnerName, Is.EqualTo("Juan"));
        Assert.That(result.ImageUrl, Is.EqualTo("file.jpg"));
    }

    private static IAsyncCursor<T> MockCursor<T>(List<T> items)
    {
        var cursor = new Mock<IAsyncCursor<T>>();
        cursor.Setup(_ => _.Current).Returns(items);
        cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
              .Returns(true)
              .Returns(false);
        cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(true)
              .ReturnsAsync(false);
        return cursor.Object;
    }
}
