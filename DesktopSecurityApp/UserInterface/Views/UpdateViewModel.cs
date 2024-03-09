using DesktopSecurityApp.Services;
using DesktopSecurityApp.UserInterface.Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Input;

namespace DesktopSecurityApp.UserInterface.Views
{
    public class UpdateViewModel : INotifyPropertyChanged
    {
        private string _activationKey;

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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
