using Licenta_Ecommerce_Mongo.Data;

namespace Licenta_Ecommerce_Mongo.Authentication
{
    public class UserAccountService
    {
        private List<UserAccount> _users;

        public UserAccountService()
        {
            _users = new List<UserAccount>
            {
                new UserAccount{ UserName = "admin", Password = "12", Role = "Admin" },
                new UserAccount{ UserName = "user", Password = "1", Role = "User" }
            };
        }

        public UserAccount? GetByUserName(string userName)
        {
            return _users.FirstOrDefault(x => x.UserName == userName);
        }
    }
}
