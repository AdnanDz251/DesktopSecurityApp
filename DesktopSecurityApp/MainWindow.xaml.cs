using DesktopSecurityApp.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopSecurityApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            // Postavi trenutni key bind u TextBox
            txtActivationKey.Text = KeybindHandling.GetActivationKey().ToString();
        }
        private void txtActivationKey_TextChanged(object sender, RoutedEventArgs e)
        {
            if (Enum.TryParse<Key>(txtActivationKey.Text, out Key selectedKey))
            {
                KeybindHandling.SetActivationKey(selectedKey);
                Key currentKey = KeybindHandling.GetActivationKey();
            }
            else
            {
                // Ako korisnik unese nešto što nije validan taster, možete prikazati poruku ili izvršiti odgovarajuće radnje.
            }
        }
    }
}