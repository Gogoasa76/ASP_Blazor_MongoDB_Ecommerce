using ASP_Blazor_MongoDB_Ecommerce.Authentication;
using ASP_Blazor_MongoDB_Ecommerce.Data;
using MongoDB.Driver;

namespace ASP_Blazor_MongoDB_Ecommerce.DBConnections
{
    public class MongoDBWrapper
    {
        private readonly string connectionString = "mongodb://localhost:27017";
        //private readonly string connectionString = "mongodb+srv://Andrei:Parola@cluster0.r2ebwa1.mongodb.net/?retryWrites=true&w=majority";
        private readonly string dbName = "Database";

        private readonly string collectionProductName = "Products";
        private readonly string collectionUserName = "Users";
        private readonly string collectionImageProduct = "ImageProduct";

        private readonly MongoClient client;
        private readonly IMongoDatabase db;

        private readonly IMongoCollection<Product> collectionProduct;
        private readonly IMongoCollection<UserAccount> collectionUser;
        private readonly IMongoCollection<ImageProduct> collectionImage;

        public MongoDBWrapper()
        {
            client = new MongoClient(connectionString);
            db = client.GetDatabase(dbName);
            collectionProduct = db.GetCollection<Product>(collectionProductName);
            collectionUser = db.GetCollection<UserAccount>(collectionUserName);
            collectionImage = db.GetCollection<ImageProduct>(collectionImageProduct);
        }

        //
        #region Product
        public async Task<List<Product>> GetAll()
        {
            return await collectionProduct.Find(_ => true).ToListAsync();
        }
        public async Task<Product> GetProductById(string id)
        {
            return await collectionProduct.Find((P) => P.Id == id).FirstAsync();
        }
        public async Task<List<Product>> GetAllProductsSearch(string value)
        {
            return await collectionProduct.Find((P) => P.Name.Contains(value) || P.Tags.Contains(value)).ToListAsync();
        }
        public async Task AddItem(Product product)
        {
            await collectionProduct.InsertOneAsync(product);
        }
        public async Task DeleteProductByID(string id)
        {
            await collectionProduct.DeleteOneAsync((P) => P.Id == id);
        }
        public async Task UpdateProduct(Product product)
        {
            UpdateDefinition<Product> definition = Builders<Product>.Update
                .Set(P => P.Name, product.Name)
                .Set(P => P.Description, product.Description)
                .Set(P => P.Discount, product.Discount)
                .Set(P => P.Price, product.Price)
                .Set(P => P.Tags, product.Tags)
                .Set(P => P.ImageBase64, product.ImageBase64);
            await collectionProduct.UpdateOneAsync(P => P.Id == product.Id, definition);
        }
        public async Task UpdateProductQuantiry(string userid, string productId, int quantity)
        {
            UserAccount account = await GetUserById(userid);
            Dictionary<string, int> cart = account.ProductCart;
            cart[productId] = quantity;
            UpdateDefinition<UserAccount> definition = Builders<UserAccount>.Update.Set(P => P.ProductCart, cart);
            await collectionUser.UpdateOneAsync(P => P.Id == userid, definition);
        }
        #endregion

        //
        #region User
        public async Task<List<UserAccount>> GetAllUsers()
        {
            return await collectionUser.Find(_ => true).ToListAsync();
        }
        public async Task<UserAccount> GetUserById(string id)
        {
            return await collectionUser.Find(P => P.Id == id).FirstAsync();
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
        public async Task AddItemToCart(string userId, string productID)
        {
            UserAccount user = await GetUserById(userId);
            Dictionary<string, int> cart = user.ProductCart;
            if (cart.ContainsKey(productID)) { return; }
            cart.Add(productID, 1);

            UpdateDefinition<UserAccount> definition = Builders<UserAccount>.Update.Set(P => P.ProductCart, cart);
            await collectionUser.UpdateOneAsync(P => P.Id == userId, definition);

        }
        public async Task AddItemToFavorites(string userId, string productID)
        {
            UserAccount user = await GetUserById(userId);
            List<string> favorites = user.Favorites;
            if (favorites.Contains(productID)) { return; }
            favorites.Add(productID);

            UpdateDefinition<UserAccount> definition = Builders<UserAccount>.Update.Set(P => P.Favorites, favorites);

            await collectionUser.UpdateOneAsync(P => P.Id == userId, definition);
        }
        public async Task<List<Product>> GetFavoritesByUserId(string id)
        {
            List<string> productsIDs = (await collectionUser.FindAsync(P => P.Id == id)).First().Favorites;
            return await collectionProduct.Find(P => productsIDs.Contains(P.Id)).ToListAsync();

        }
        public async Task<List<Product>> GetCartByUserId(string id)
        {
            Dictionary<string, int> productsIDs = (await collectionUser.FindAsync(P => P.Id == id)).First().ProductCart;
            List<string> productsids = productsIDs.Keys.ToList();
            return await collectionProduct.Find(P => productsids.Contains(P.Id)).ToListAsync();
        }
        public async Task<int> GetQuantityForProduct(string userId, string productId)
        {
            return (await collectionUser.FindAsync(P => P.Id == userId)).First().ProductCart[productId];
        }
        public async void RemoveProductFromUserCart(string userId, string productId)
        {
            UserAccount user = (await collectionUser.FindAsync(P => P.Id == userId)).First();
            user.ProductCart.Remove(productId);
            UpdateDefinition<UserAccount> definition = Builders<UserAccount>.Update.Set(P => P.ProductCart, user.ProductCart);
            await collectionUser.UpdateOneAsync(P => P.Id == userId, definition);
        }
        public async void RemoveProductFromUserFavorites(string userId, string productId)
        {
            UserAccount user = (await collectionUser.FindAsync(P => P.Id == userId)).First();
            user.Favorites.Remove(productId);
            UpdateDefinition<UserAccount> definition = Builders<UserAccount>.Update.Set(P => P.Favorites, user.Favorites);
            await collectionUser.UpdateOneAsync(P => P.Id == userId, definition);
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
        public async void CheckForDefaultAdmin()
        {
            List<UserAccount> users = await collectionUser.Find(P => P.Role == "Admin").ToListAsync();
            if (users.Count == 0)
            {
                UserAccount admin = new();
                admin.UserName = "admin";
                admin.Password = Hashing.GetHash("1234");
                admin.Role = "Admin";

                await AddUser(admin);
            }
        }
        #endregion

        #region Product Image
        public async Task AddProductImage(ImageProduct imageProduct)
        {
            await collectionImage.InsertOneAsync(imageProduct);
        }
        public async Task<ImageProduct> GetProductImage(string id)
        {
            List<ImageProduct> images = await collectionImage.Find((P) => P.ProductId == id).ToListAsync();
            return images.Count==0? new():images.First();
        }
        #endregion
    }
}