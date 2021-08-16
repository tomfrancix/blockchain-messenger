using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using sakurai.Interface.IFactory;

namespace sakurai.Core.Factory
{
    public class HashFactory : IHashFactory
    {
        public string GenerateHash(string timestamp, string lastHash, string data)
        {
            var hashValue = timestamp + lastHash + data;
            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(hashValue, salt, 100000); 
            
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36]; 
            
            Array.Copy(salt, 0, hashBytes, 0, 16); Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
