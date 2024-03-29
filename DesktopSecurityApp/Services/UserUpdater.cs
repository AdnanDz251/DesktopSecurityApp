﻿using DesktopSecurityApp.Models;
using Newtonsoft.Json;
using System.IO;

namespace DesktopSecurityApp.Services
{
    public static class UserUpdater
    {
        public static void UpdateUserInfoInJsonFile(UpdateUserInfo updatedUserInfo, string customFolderPath)
        {
            string jsonFilePath = Path.Combine(customFolderPath, "user_data.json");

            // Provjera postojanja datoteke
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException("user_data.json file not found.");
            }

            // Čitanje JSON podataka iz datoteke
            string encryptedJsonData = File.ReadAllText(jsonFilePath);

            // Dekriptiranje JSON podataka
            string decryptedJsonData = JsonEncryptionDecryption.DecryptFromJsonFile<string>(jsonFilePath);

            // Deserijalizacija JSON podataka u objekt
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(decryptedJsonData);

            // Ažuriranje informacija o korisniku
            if (!string.IsNullOrEmpty(updatedUserInfo.NewUsername))
            {
                userInfo.Username = updatedUserInfo.NewUsername;
            }
            if (!string.IsNullOrEmpty(updatedUserInfo.NewKey))
            {
                userInfo.Key = updatedUserInfo.NewKey;
            }
            if (!string.IsNullOrEmpty(updatedUserInfo.NewEmail))
            {
                userInfo.Email = updatedUserInfo.NewEmail;
            }

            // Ponovno enkriptiranje i spremanje ažuriranih podataka u datoteku
            JsonEncryptionDecryption.EncryptToJsonFile(userInfo, "user_data.json", customFolderPath);
        }

    }
}
