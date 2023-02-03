using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashKey
{
    public class KeyGenerator
    {
        private static string GeneratePassword(string plainText, string passPhrase, string saltValue, int passwordIterations = 3)
        {
            try
            {
                string hashAlgorithm = "SHA256";
                string initVector = "ABCDEF9876543210";
                int keySize = 256;

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
                byte[] keyBytes = password.GetBytes(keySize / 8);

                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                //symmetricKey.BlockSize = 128;
                //symmetricKey.Padding = PaddingMode.Zeros;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                string cipherText = BitConverter.ToString(cipherTextBytes).Replace("-", "");

                return cipherText;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string GetPassword(string value,string passkey = "")
        {
            string text, pass, salt;

            text = "JIP-System123-" + value;
            pass = text + "|Key-" + passkey + "|" + value + "|user";
            salt = "Key -" + passkey;
            return GeneratePassword(text, pass, salt);
        }
    }
}
