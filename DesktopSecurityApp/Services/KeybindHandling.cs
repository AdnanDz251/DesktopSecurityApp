using System.Windows;
using System.Windows.Input;

namespace DesktopSecurityApp.Services
{
    public class KeybindHandling
    {
        public static OverlayWindow overlayWindow;
        public static void RegisterKeyBindings(Window window)
        {
            EventManager.RegisterClassHandler(typeof(Window), Window.KeyDownEvent,new KeyEventHandler(KeyBindHandler));
        }
        public static void KeyBindHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                HandleValidKeyBind();
            }
            else
            {
                HandleFalseKeyBind();
            }
        }

        public static void HandleValidKeyBind()
        {
            if (overlayWindow == null)
            {
                OpenOverlayWindow();
            }
            else
            {
                CloseOverlayWindow();
            }
        }

        public static void HandleFalseKeyBind()
        {
            //Logika za kameru ...
        }

        public static void OpenOverlayWindow()
        {
            overlayWindow = new OverlayWindow();
            overlayWindow.Show();
        }

        public static void CloseOverlayWindow()
        {
            overlayWindow.Close();
            overlayWindow = null;
        }
    }
}

