using MongoDB.Bson.Serialization.Attributes;

namespace Licenta_Ecommerce_Mongo.Data
{
    public class ImageProduct
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public List<string> ImageBase64 { get; set; } = new();

        public ImageProduct() { }
    }
}
