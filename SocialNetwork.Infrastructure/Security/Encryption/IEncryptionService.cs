using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Encryption
{
    public interface IEncryptionService
    {
        string CreateSaltKey(int size);
        string CreatePasswordHash(string password, string saltkey);
        string EncryptText(string plainText, string privateKey);
        string DecryptText(string cipherText, string encryptionPrivateKey);
    }
}
