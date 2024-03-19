using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using System.Text.Json;


namespace DesktopSecurityApp.Services 
{ 
    public static class JsonEncryptionDecryption
    {
        // Ključ za enkripciju/dekripciju. Preporučuje se korištenje sigurnog i nasumičnog ključa.
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("YourSecretKey123"); // Promijenite ključ prema potrebi

        private static readonly byte[] IV = new byte[16]; // IV za AES, 16 bajtova

        public static void EncryptToJsonFile<T>(T data, string filePath, string customFolderPath)
        {
            // Provjera da li postoji prilagođena mapa
            if (!Directory.Exists(customFolderPath))
            {
                Directory.CreateDirectory(customFolderPath);
            }

            // Formiranje potpune putanje do JSON datoteke
            string jsonFilePath = Path.Combine(customFolderPath, filePath);

            // Serijalizacija objekta u JSON format
            string jsonData = JsonConvert.SerializeObject(data);

            // Enkripcija JSON podataka
            byte[] encryptedData = EncryptStringToBytes(jsonData, Key);

            // Upis enkriptovanih podataka u datoteku
            File.WriteAllBytes(jsonFilePath, encryptedData);
        }

        public static T DecryptFromJsonFile<T>(string filePath)
        {
            // Čitanje enkriptovanih podataka iz datoteke
            byte[] encryptedData = File.ReadAllBytes(filePath);

            // Dekriptovanje podataka
            string decryptedJson = DecryptStringFromBytes(encryptedData, Key).ToString();
            string jsonAsString = JsonConvert.SerializeObject(decryptedJson);

            // Deserijalizacija JSON podataka u objekat
            return JsonConvert.DeserializeObject<T>(jsonAsString);
        }

        public static byte[] EncryptStringToBytes(string plainText, byte[] key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                // Inicijalizacija kriptografskog transformatora za enkripciju
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Upis enkriptovanih podataka u kriptografski tok
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

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                // Inicijalizacija kriptografskog transformatora za dekripciju
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
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
