using MongoDB.Bson.Serialization.Attributes;

namespace Licenta_Ecommerce_Mongo.Authentication
{
    public class UserAccount
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;    
    }
}
