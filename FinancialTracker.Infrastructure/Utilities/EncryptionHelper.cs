using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FinancialTracker.Infrastructure.Utilities
{
    public static class EncryptionHelper
    {
        private const string DefaultKey = "4g!DVhg6"; // Replace with a secure key

        /// <summary>
        /// Encrypt an integer ID.
        /// </summary>
        /// <param name="id">The ID to encrypt.</param>
        /// <returns>The encrypted ID as a base64 string.</returns>
        public static string EncryptId(int id)
        {
            string idAsString = id.ToString();
            return Encrypt(idAsString, DefaultKey);
        }

        /// <summary>
        /// Decrypt an encrypted ID string.
        /// </summary>
        /// <param name="encryptedId">The encrypted ID string.</param>
        /// <returns>The decrypted integer ID.</returns>
        public static int DecryptId(string encryptedId)
        {
            string decryptedString = Decrypt(encryptedId, DefaultKey);
            return int.Parse(decryptedString);
        }

        /// <summary>
        /// Encrypt a string using the specified key.
        /// </summary>
        /// <param name="strToEncrypt">The string to encrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The encrypted string.</returns>
        private static string Encrypt(string strToEncrypt, string key)
        {
            try
            {
                TripleDESCryptoServiceProvider desCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash = hashMD5.ComputeHash(Encoding.ASCII.GetBytes(key));
                hashMD5.Clear();

                desCrypto.Key = byteHash;
                desCrypto.Mode = CipherMode.ECB;

                byte[] byteBuff = Encoding.ASCII.GetBytes(strToEncrypt);
                string encrypted = Convert.ToBase64String(desCrypto.CreateEncryptor()
                    .TransformFinalBlock(byteBuff, 0, byteBuff.Length));

                desCrypto.Clear();
                return encrypted;
            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting string: " + ex.Message);
            }
        }

        /// <summary>
        /// Decrypt a string using the specified key.
        /// </summary>
        /// <param name="strEncrypted">The string to decrypt.</param>
        /// <param name="key">The decryption key.</param>
        /// <returns>The decrypted string.</returns>
        private static string Decrypt(string strEncrypted, string key)
        {
            try
            {
                TripleDESCryptoServiceProvider desCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash = hashMD5.ComputeHash(Encoding.ASCII.GetBytes(key));
                hashMD5.Clear();

                desCrypto.Key = byteHash;
                desCrypto.Mode = CipherMode.ECB;

                byte[] byteBuff = Convert.FromBase64String(strEncrypted);
                string decrypted = Encoding.ASCII.GetString(desCrypto.CreateDecryptor()
                    .TransformFinalBlock(byteBuff, 0, byteBuff.Length));

                desCrypto.Clear();
                return decrypted;
            }
            catch (Exception ex)
            {
                throw new Exception("Error decrypting string: " + ex.Message);
            }
        }

        /// <summary>
        /// Encrypt an ID and encode it for use in a URL.
        /// </summary>
        /// <param name="id">The ID to encrypt.</param>
        /// <returns>The encrypted and URL-encoded string.</returns>
        public static string EncryptIdToUrl(int id)
        {
            return HttpUtility.UrlEncode(EncryptId(id));
        }

        /// <summary>
        /// Decrypt an encrypted ID from a URL.
        /// </summary>
        /// <param name="encryptedId">The encrypted and URL-encoded string.</param>
        /// <returns>The decrypted integer ID.</returns>
        public static int DecryptIdFromUrl(string encryptedId)
        {
            try
            {
                string decodedString = HttpUtility.UrlDecode(encryptedId);
                return DecryptId(decodedString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error decrypting ID from URL: " + ex.Message);
            }
        }
    }
}
