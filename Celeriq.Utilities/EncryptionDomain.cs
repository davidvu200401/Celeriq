#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Celeriq.Utilities
{
    /// <summary />
    public static class EncryptionDomain
    {
        //private static AesCryptoServiceProvider aesCSP;
        //private const string encryptionKey = "릠䵎珣層熌혙鰻᳿ᘳ끔牬㩽٨Ⴁ㪽젓";
        private const string encryptionIV = "狟⊘㵕ᶷﳰ␍虭";

        static EncryptionDomain()
        {
        }

        /// <summary>
        /// Encrypt a string with a specified key
        /// </summary>
        /// <param name="inString">The string to encrypt</param>
        /// <param name="key">The key to use for encryption</param>
        public static string Encrypt(string inString, string key)
        {
            try
            {
                var aesCSP = new AesCryptoServiceProvider();
                aesCSP.Key = Encoding.Unicode.GetBytes(key);
                aesCSP.IV = Encoding.Unicode.GetBytes(encryptionIV);
                var encString = EncryptString(aesCSP, inString);
                return Convert.ToBase64String(encString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Decrypt a string with a specified key
        /// </summary>
        /// <param name="inString">The string to decrypt</param>
        /// <param name="key">The key to use for decryption</param>
        public static string Decrypt(string inString, string key)
        {
            try
            {
                var aesCSP = new AesCryptoServiceProvider();
                aesCSP.Key = Encoding.Unicode.GetBytes(key);
                aesCSP.IV = Encoding.Unicode.GetBytes(encryptionIV);
                var encBytes = Convert.FromBase64String(inString);
                return DecryptString(aesCSP, encBytes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static byte[] EncryptString(SymmetricAlgorithm symAlg, string inString)
        {
            var inBlock = Encoding.Unicode.GetBytes(inString);
            var xfrm = symAlg.CreateEncryptor();
            var outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);
            return outBlock;
        }

        private static string DecryptString(SymmetricAlgorithm symAlg, byte[] inBytes)
        {
            var xfrm = symAlg.CreateDecryptor();
            var outBlock = xfrm.TransformFinalBlock(inBytes, 0, inBytes.Length);
            return Encoding.Unicode.GetString(outBlock);
        }

        public static int Hash(string s)
        {
            var algorithm = SHA1.Create();
            var b = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
            return BitConverter.ToInt32(b, 0);
        }
    }
}