using System.Windows;
using System.Windows.Input;

namespace DesktopSecurityApp.Services
{
    internal class KeybindHandling
    {
        public static void RegisterKeyBindings(Window window)
        {
            EventManager.RegisterClassHandler(typeof(Window), Window.KeyDownEvent,new KeyEventHandler(KeyBindHandler));
        }
        private static void KeyBindHandler(object sender, KeyEventArgs e)
        {
            // Provera key-bind kombinacije (Ctrl + Alt + S)
            if ( e.Key == Key.S)
            {
                // Ako je key-bind ispravan, izvršite željene akcije
                MessageBox.Show("Key-bind uspešno pritisnut!");
                // Dodajte ovde dodatni kôd ili pozovite funkciju koja će se izvršiti
            }
        }
    }
}

