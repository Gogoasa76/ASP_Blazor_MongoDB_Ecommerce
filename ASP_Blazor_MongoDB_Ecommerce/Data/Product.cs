using MongoDB.Bson.Serialization.Attributes;

namespace Licenta_Ecommerce_Mongo.Data
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageBase64 { get; set; }
        public List<string> Tags { get; set; }
        public int Price { get; set; } = 0;
        public int Discount { get; set; } = 0;

        public Product() { }
    }
}