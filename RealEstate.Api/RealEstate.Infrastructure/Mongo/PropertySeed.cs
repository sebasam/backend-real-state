using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Seed;

public static class PropertySeed
{
    public static async Task SeedAsync(
        IMongoCollection<Property> properties,
        IMongoCollection<PropertyImage> images,
        IMongoCollection<Owner> owners // 👈 nuevo
    )
    {
        // Verifica si ya hay datos
        var count = await properties.CountDocumentsAsync(FilterDefinition<Property>.Empty);
        if (count > 0) return; // ya hay datos, no hace nada

        Console.WriteLine("Seeding database...");

        var random = new Random();

        // Crear lista de owners con nombres reales simulados
        var ownersList = new List<Owner>();
        for (int o = 1; o <= 50; o++)
        {
            ownersList.Add(new Owner
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = $"Propietario {o}",
                Address = $"Dirección {o}",
                Birthday = DateTime.Now.AddYears(-random.Next(25, 60)),
                Photo = $"https://i.pravatar.cc/150?img={o}"
            });
        }
        await owners.InsertManyAsync(ownersList);

        for (int i = 1; i <= 1000; i++) // 1000 propiedades
        {
            var propertyId = ObjectId.GenerateNewId().ToString();
            var randomOwner = ownersList[random.Next(ownersList.Count)];

            var property = new Property
            {
                Id = propertyId,
                Name = $"Propiedad {i}",
                Address = $"Calle {i}, Ciudad XYZ",
                Price = random.Next(50_000, 500_000),
                OwnerId = randomOwner.Id,
                CodeInternal = $"C-{i:0000}",
                Year = random.Next(1990, 2025)
            };

            await properties.InsertOneAsync(property);

            // Crear 1 a 3 imágenes por propiedad
            int imgCount = random.Next(1, 4);
            for (int j = 1; j <= imgCount; j++)
            {
                var img = new PropertyImage
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PropertyId = propertyId,
                    File = $"https://picsum.photos/seed/{i}-{j}/600/400",
                    Enabled = true
                };
                await images.InsertOneAsync(img);
            }
        }

        Console.WriteLine("Database seeded successfully!");
    }
}
