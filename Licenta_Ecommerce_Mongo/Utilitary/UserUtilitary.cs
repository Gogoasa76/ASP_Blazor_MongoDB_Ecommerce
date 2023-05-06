
using Licenta_Ecommerce_Mongo.DBConnections;

namespace Licenta_Ecommerce_Mongo.Utilitary
{
    public class UserUtilitary
    {
        public static void CreateDefaultAdminIfNoneExist()
        {
            MongoDBWrapper mongo = new();
            mongo.CheckForDefaultAdmin();
        }
    }
}
