using MongoDB.Bson.Serialization.Attributes;

namespace ASP_Blazor_MongoDB_Ecommerce.Data
{
    public class UserAccount
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Dictionary<string,int> ProductCart { get; set; }= new ();
        public List<string> Favorites { get; set; }= new ();
    }
}