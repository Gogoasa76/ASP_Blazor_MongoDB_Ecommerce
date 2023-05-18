using System.Security.Cryptography;
using System.Text;

namespace Licenta_Ecommerce_Mongo.Authentication
{
    public static class Hashing
    {
        public static string GetHash(string inputString)
        {
            using HashAlgorithm algorithm = SHA256.Create();
            byte[] encoded = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            return BitConverter.ToString(encoded).Replace("-", "");
        }
    }
}
