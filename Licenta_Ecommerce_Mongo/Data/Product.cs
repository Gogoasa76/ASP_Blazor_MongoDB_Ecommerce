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

        public Product()
        {

        }

        public Product(string name,string description, string imageBase64,List<string> tags,int price,int discount)
        {
            Name = name;
            Description = description;
            ImageBase64 = imageBase64;
            Tags = tags;
            //se asigura ca pretul nu e negativ
            Price = price < 0 ? -price : price;
            //se asigura ca discountul e intre 0 si 100
            Discount = discount < 0 ? -discount : discount;
            Discount = discount % 100;
        }
    }
}
