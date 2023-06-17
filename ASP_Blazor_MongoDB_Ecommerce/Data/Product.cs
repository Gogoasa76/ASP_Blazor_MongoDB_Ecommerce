using MongoDB.Bson.Serialization.Attributes;

namespace ASP_Blazor_MongoDB_Ecommerce.Data
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageBase64 { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public int Price { get; set; } = 0;
        public int Discount { get; set; } = 0;

        public Product() { }
    }
}