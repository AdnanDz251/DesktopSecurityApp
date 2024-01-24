using System.Windows;
using System.Windows.Input;

namespace DesktopSecurityApp.Services
{
    internal class KeybindHandling
    {
        private static OverlayWindow overlayWindow;
        public static void RegisterKeyBindings(Window window)
        {
            EventManager.RegisterClassHandler(typeof(Window), Window.KeyDownEvent,new KeyEventHandler(KeyBindHandler));
        }
        public static void KeyBindHandler(object sender, KeyEventArgs e)
        {
            // Provera key-bind kombinacije (Ctrl + Alt + S)
            if ( e.Key == Key.S)
            {
                // Ako je key-bind ispravan, izvršite željene akcije
                MessageBox.Show("Key-bind uspešno pritisnut!");
                // Dodajte ovde dodatni kôd ili pozovite funkciju koja će se izvršiti
            }
            else
            {
                // Ako key-bind nije Ctrl + Alt + S, proverite otvaranje/zatvaranje overlay-a
                HandleOverlay(e);
            }
        }
        private static void HandleOverlay(KeyEventArgs e)
        {
            // Ako je key-bind ispravan, otvorite/zatvorite overlay
            if (overlayWindow == null)
            {
                overlayWindow = new OverlayWindow();
                overlayWindow.Show();
            }
            else
            {
                overlayWindow.Close();
                overlayWindow = null;
            }
        }
    }
}

