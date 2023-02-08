using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Interfaces
{
    public interface ICryptoGraphy
    {
        public (string Key, string IVBase64) InitSymmetricEncryptionKeyIv();
        string Encrypt(string text, string IV, string key);
        string Decrypt(string encryptedText, string IV, string key);
    }
}
