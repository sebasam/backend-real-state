using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("propertyId")]
        public string PropertyId { get; set; } = null!;

        [BsonElement("file")]
        public string File { get; set; } = null!;

        [BsonElement("enabled")]
        public bool Enabled { get; set; }
    }
}
