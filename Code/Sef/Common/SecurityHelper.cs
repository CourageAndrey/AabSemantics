using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sef.Common
{
    public static class SecurityHelper
    {
        private const Int32 keyLength = 16;

        public static Byte[] Encrypt(this String text, String password)
        {
            var algorithm = Rijndael.Create();
            var cryptoTransform = algorithm.CreateEncryptor(new Rfc2898DeriveBytes(password, salt).GetBytes(keyLength), new Byte[keyLength]);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
            Byte[] data = Encoding.UTF8.GetBytes(text);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        public static String Decrypt(this Byte[] data, String password)
        {
            var algorithm = Rijndael.Create();
            var cryptoTransform = algorithm.CreateDecryptor(new Rfc2898DeriveBytes(password, salt).GetBytes(keyLength), new Byte[keyLength]);
            var memoryStream = new MemoryStream(data);
            var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
            var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        private static readonly Byte[] salt = { 1, 2, 0, 3, 1, 9, 8, 7 };
    }
}
