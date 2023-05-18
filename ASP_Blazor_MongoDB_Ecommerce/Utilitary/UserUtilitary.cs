using ASP_Blazor_MongoDB_Ecommerce.DBConnections;

namespace ASP_Blazor_MongoDB_Ecommerce.Utilitary
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
