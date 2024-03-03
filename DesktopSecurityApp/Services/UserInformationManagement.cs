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
    }
}
