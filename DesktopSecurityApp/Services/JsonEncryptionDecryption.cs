﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace DesktopSecurityApp.Services
{
    public static class JsonEncryptionDecryption
    {
        // Ključ za enkripciju/dekripciju. Preporučuje se korištenje sigurnog i nasumičnog ključa.
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("YourSecretKey123"); // Promijenite ključ prema potrebi

        public static void EncryptToJsonFile<T>(T data, string filePath)
        {
            // Serijalizacija objekta u JSON format
            string jsonData = JsonConvert.SerializeObject(data);

            // Enkripcija JSON podataka
            byte[] encryptedData = EncryptStringToBytes(jsonData, Key);

            // Upis enkriptovanih podataka u datoteku
            File.WriteAllBytes(filePath, encryptedData);
        }

        public static T DecryptFromJsonFile<T>(string filePath)
        {
            // Čitanje enkriptovanih podataka iz datoteke
            byte[] encryptedData = File.ReadAllBytes(filePath);

            // Dekriptovanje podataka
            string decryptedJson = DecryptStringFromBytes(encryptedData, Key);

            // Deserijalizacija JSON podataka u objekat
            return JsonConvert.DeserializeObject<T>(decryptedJson);
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();

                // Inicijalizacija kriptografskog transformatora za enkripciju
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Upis IV u početak enkriptovanog toka kako bi se koristio prilikom dekriptovanja
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Upis enkriptovanog teksta u kriptografski tok
                            swEncrypt.Write(plainText);
                        }
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                // Izdvoj IV iz enkriptovanih podataka
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Array.Copy(cipherText, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;

                // Inicijalizacija kriptografskog transformatora za dekripciju
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText, iv.Length, cipherText.Length - iv.Length))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Čitanje dekriptovanih podataka iz kriptografskog toka
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
