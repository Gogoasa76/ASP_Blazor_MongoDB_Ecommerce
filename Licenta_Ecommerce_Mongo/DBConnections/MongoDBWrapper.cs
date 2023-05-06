using Licenta_Ecommerce_Mongo.Authentication;
using Licenta_Ecommerce_Mongo.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Licenta_Ecommerce_Mongo.DBConnections
{
    public class MongoDBWrapper
    {
        private readonly string connectionString = "mongodb://localhost:27017";
        //private readonly string connectionString = "mongodb+srv://Andrei:Parola@cluster0.r2ebwa1.mongodb.net/?retryWrites=true&w=majority";
        private readonly string dbName = "Database";

        private readonly string collectionProductName = "Products";
        private readonly string collectionUserName = "Users";
        private readonly string collectionOrderName = "Orders";

        private readonly MongoClient client;
        private readonly IMongoDatabase db;

        private readonly IMongoCollection<Product> collectionProduct;
        private readonly IMongoCollection<UserAccount> collectionUser;
        private readonly IMongoCollection<Order> collectionOrder;
        public MongoDBWrapper()
        {
            client = new MongoClient(connectionString);
            db = client.GetDatabase(dbName);
            collectionProduct = db.GetCollection<Product>(collectionProductName);
            collectionUser = db.GetCollection<UserAccount>(collectionUserName);
            collectionOrder = db.GetCollection<Order>(collectionOrderName);
        }

        //
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
        public async Task DeleteProductByID(string id)
        {
            await collectionProduct.DeleteOneAsync((P)=>P.Id==id);
        }
        public async Task UpdateProduct(Product product)
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

        //
        #region User

        public async Task<UserAccount> GetUserById(string id)
        {
            return await collectionUser.Find(P=>P.Id == id).FirstAsync();
        }
        public async Task AddUser(UserAccount userAccount)
        {
            await collectionUser.InsertOneAsync(userAccount);
        }
        public async Task DeleteUserById(string id)
        {
            await collectionUser.DeleteOneAsync((P) => P.Id == id);
        }
        public async Task UpdateUser(UserAccount userAccount)
        {
            UpdateDefinition<UserAccount> definition = Builders<UserAccount>.Update
                .Set(P => P.UserName, userAccount.UserName)
                .Set(P => P.Password, userAccount.Password)
                .Set(P => P.Email, userAccount.Email)
                .Set(P => P.Role, userAccount.Role);
            await collectionUser.UpdateOneAsync(P => P.Id == userAccount.Id, definition);
        }
        public async Task<UserAccount> CheckUser(string username, string password)
        {
            UserAccount userAccount;
            try
            {
                userAccount = (await collectionUser.FindAsync(U => (U.UserName == username && U.Password == password))).First();
                userAccount.Password = "";
            }
            catch (Exception)
            {
                userAccount = new();
            }
            return userAccount;
        }

        public async void CheckForDefaultAdmin() {
            List<UserAccount> users = await collectionUser.Find(P => P.Role == "Admin").ToListAsync();
            if (users.Count==0)
            {
                UserAccount admin= new();
                admin.UserName = "admin";
                admin.Password = Hashing.GetHash("1234");
                admin.Role = "Admin";

                await AddUser(admin);
            }
        }
        #endregion

        //
        #region Order
        public async Task<List<Order>> GetAllOrders()
        {
            return await collectionOrder.Find(_ => true).ToListAsync();
        }
        public async Task<Order> GetOrderstByUserId(string userID)
        {
            return await collectionOrder.Find((P) => P.UserID == userID).FirstAsync();
        }
        public async Task<Order> GetOrderstByDate(string date)
        {
            return await collectionOrder.Find((P) => P.Date == date).FirstAsync();
        }
        #endregion
    }
}
