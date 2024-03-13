using DesktopSecurityApp.Models;
using DesktopSecurityApp.Services;
using DesktopSecurityApp.UserInterface.Core;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DesktopSecurityApp.UserInterface.Views
{
    public class UpdateViewModel : INotifyPropertyChanged
    {
        private string _activationKey;
        private string _newUsername;
        private string _newKey;
        private string _newEmail;

        public string ActivationKey
        {
            get { return _activationKey; }
            set
            {
                if (_activationKey != value)
                {
                    _activationKey = value;
                    OnPropertyChanged();
                }
            }
        }

        // Dodajte svojstva za nove podatke korisnika (npr. NewUsername, NewKey, NewEmail)

        public ICommand SaveCommand { get; }

        public UpdateViewModel()
        {
            // Inicijalizacija komande za čuvanje aktivacione ključne riječi
            SaveCommand = new RelayCommand(SaveActivationKey);
        }

        public void SaveActivationKey(object parameter)
        {
            // Ovdje dodajte logiku za čuvanje aktivacione ključne riječi
            KeybindHandling.SetActivationKey((Key)new KeyConverter().ConvertFromString(ActivationKey));

            // Poziv metode za ažuriranje korisničkih podataka
            UpdateUserInfo();
        }

        private void UpdateUserInfo()
        {
            // Prvo kreirajte objekat sa novim informacijama korisnika
            var updatedUserInfo = new UpdateUserInfo
            {
                NewUsername = _newUsername,
                NewKey = _newKey,
                NewEmail = _newEmail
            };

            // Zatim pozovite metodu za ažuriranje korisničkih podataka u JSON datoteci
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;
            string customFolderPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executablePath).FullName).FullName).FullName).FullName, "Data", "UserData");

            try
            {
                UserUpdater.UpdateUserInfoInJsonFile(updatedUserInfo, customFolderPath);
            }
            catch (FileNotFoundException ex)
            {
                // Obrada greške ako datoteka nije pronađena
            }
            catch (Exception ex)
            {
                // Obrada ostalih grešaka
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
