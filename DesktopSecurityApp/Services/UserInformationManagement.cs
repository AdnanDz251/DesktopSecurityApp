﻿using DesktopSecurityApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSecurityApp.Services
{
    public static class UserInformationManagement
    {
        public static void SaveUserInfoToJsonFile()
        {
            // Instanciranje objekta UserInfo s korisničkim informacijama
            UserInfo userInfo = new UserInfo
            {
                Username = "example_user",
                Key = "L",
                Email = "example@example.com"
            };

            // Dobijte apsolutnu putanju do direktorija izvršne datoteke aplikacije
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;

            string customFolderPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executablePath).FullName).FullName).FullName).FullName, "Data", "UserData");
            Directory.CreateDirectory(customFolderPath);

            // Putanja do JSON datoteke "user_data.json" unutar direktorija "UserData"
            string jsonFilePath = Path.Combine(customFolderPath, "user_data.json");

            // Spremanje korisničkih informacija u JSON datoteku
            JsonEncryptionDecryption.EncryptToJsonFile(userInfo, jsonFilePath);
        }
        public static void UpdateUserInfoInJsonFile(UpdateUserInfo updatedUserInfo)
        {
            // Dobivanje putanje do JSON datoteke
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;
            string customFolderPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executablePath).FullName).FullName).FullName).FullName, "Data", "UserData");
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
            JsonEncryptionDecryption.EncryptToJsonFile(userInfo, jsonFilePath);
        }
    }
}
