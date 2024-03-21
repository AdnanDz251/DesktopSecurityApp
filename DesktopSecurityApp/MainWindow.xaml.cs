using DesktopSecurityApp.Services;
using System.Windows;
using System.Windows.Input;

namespace DesktopSecurityApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string txtActivationKey = "";
        public MainWindow()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            //KeybindHandling.RegisterKeyBindings(this); // Dodajte ovu liniju za registraciju keybindovass

            // Postavi trenutni key bind u TextBox
            txtActivationKey = KeybindHandling.GetActivationKey().ToString();

        }
        private void txtActivationKey_TextChanged(object sender, RoutedEventArgs e)
        {
            if (Enum.TryParse<Key>(txtActivationKey, out Key selectedKey))
            {
                KeybindHandling.SetActivationKey(selectedKey);
                Key currentKey = KeybindHandling.GetActivationKey();
            }
            else
            {
                // Ako korisnik unese nešto što nije validan taster, možete prikazati poruku ili izvršiti odgovarajuće radnje.
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}