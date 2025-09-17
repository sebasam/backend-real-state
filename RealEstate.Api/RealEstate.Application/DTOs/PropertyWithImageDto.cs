namespace RealEstate.Application.Dtos
{
    public class PropertyWithImageDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public string OwnerId { get; set; } = null!;
        public string? OwnerName { get; set; }
        public string? ImageUrl { get; set; }
    }

}
