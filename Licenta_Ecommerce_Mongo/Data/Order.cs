using MongoDB.Bson.Serialization.Attributes;

namespace Licenta_Ecommerce_Mongo.Data
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string UserID { get; set; }= string.Empty;
        public List<string> ProductId { get; set; } = new List<string>();
        public string Date { get; set; } = string.Empty;
    }
}
