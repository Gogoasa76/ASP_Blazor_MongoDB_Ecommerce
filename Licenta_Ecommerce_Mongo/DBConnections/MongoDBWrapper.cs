using Licenta_Ecommerce_Mongo.Authentication;
using Licenta_Ecommerce_Mongo.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Licenta_Ecommerce_Mongo.DBConnections
{
    public class MongoDBWrapper
    {
        private readonly string connectionString = "mongodb://localhost:27017";
        private readonly string dbName = "Database";

        private readonly string collectionProductName = "Products";
        private readonly string collectionUserName = "Users";

        private readonly MongoClient client;
        private readonly IMongoDatabase db;

        private readonly IMongoCollection<Product> collectionProduct;
        //private readonly IMongoCollection<UserAccount> collectionUser;

        public MongoDBWrapper()
        {
            client = new MongoClient(connectionString);
            db = client.GetDatabase(dbName);
            collectionProduct = db.GetCollection<Product>(collectionProductName);
            //collectionUser = db.GetCollection<UserAccount>(collectionUserName);
        }

        #region Product_Area
        public async Task<List<Product>> GetAll()
        {
            return await collectionProduct.Find(_ => true).ToListAsync();
        }
        
        public async Task<Product> GetProductById(string id)
        {
            return await collectionProduct.Find((P) => P.Id == id).FirstAsync();
        }

        public async Task<List<Product>> GetAllProductsWithName(string name)
        {
            return await collectionProduct.Find((P)=>P.Name.Contains(name)).ToListAsync();
        }
        public async Task<List<Product>> GetDiscountedProducts()
        {
            return await collectionProduct.Find((P)=>P.Discount>0).ToListAsync();
        }
        public async Task<List<Product>> GetByPrice(int minPrice, int maxPrice)
        {
            return await collectionProduct.Find((P)=>P.Price>minPrice && P.Price<maxPrice).ToListAsync();
        }
        public async Task<List<Product>> GetProductsWithTag(string tag)
        {
            return await collectionProduct.Find((P) => P.Tags.Contains(tag)).ToListAsync();
        }

        public async Task AddItem(Product product)
        {
            await collectionProduct.InsertOneAsync(product);
        }
        public async Task DeleteByID(string id)
        {
            await collectionProduct.DeleteOneAsync((P)=>P.Id==id);
        }
        public async Task UpdateItem(Product product)
        {
            UpdateDefinition<Product> definition = Builders<Product>.Update
                .Set(P => P.Name, product.Name)
                .Set(P => P.Description, product.Description)
                .Set(P => P.Discount, product.Discount)
                .Set(P => P.Price, product.Price)
                .Set(P => P.Tags, product.Tags)
                .Set(P => P.ImageBase64, product.ImageBase64)
                .Set(P => P.Quantity, product.Quantity);
            await collectionProduct.UpdateOneAsync(P => P.Id == product.Id, definition);
        }
        #endregion
    }
}
