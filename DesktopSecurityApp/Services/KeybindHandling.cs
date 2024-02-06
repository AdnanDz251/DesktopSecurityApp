using DesktopSecurityApp.Services;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace DesktopSecurityApp.Services
{
    public class KeybindHandling
    {
        public static OverlayWindow overlayWindow;

        private static Key activationKey = Key.S; // Default activation key is 'S'

        public static void RegisterKeyBindings(Window window)
        {
            EventManager.RegisterClassHandler(typeof(Window), Window.KeyDownEvent, new KeyEventHandler(KeyBindHandler));
        }

        public static void SetActivationKey(Key key)
        {
                activationKey = key;
        }

        public static Key GetActivationKey()
        {
            return activationKey;
        }

        private static void KeyBindHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == activationKey)
            {
                HandleValidKeyBind();
            }
            else
            {
                HandleFalseKeyBind();
            }
        }

        private static void HandleValidKeyBind()
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

        private static void HandleFalseKeyBind()
        {
            // Pozivamo metodu CaptureAndSave sa prosljeđenom putanjom (LOGIKA ZA KAMERU)
            CameraHandling cameraHandler = new CameraHandling();
            cameraHandler.StartCamera();
        }

        private static void OpenOverlayWindow()
        {
            overlayWindow = new OverlayWindow();
            overlayWindow.Show();
        }

        private static void CloseOverlayWindow()
        {
            overlayWindow.Close();
            overlayWindow = null;
        }
    }
}

