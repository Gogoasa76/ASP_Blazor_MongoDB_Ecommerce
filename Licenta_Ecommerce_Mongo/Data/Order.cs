using MongoDB.Bson.Serialization.Attributes;

namespace Licenta_Ecommerce_Mongo.Data
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string UserID { get; set; }= string.Empty;
        public Dictionary<string,int> ProductId { get; set; } = new();
        public string Date { get; set; } = string.Empty;
    }
}
