using EmailSenderApp;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace DesktopSecurityApp.Services
{
    public class KeybindHandling
    {
        // Import Windows API functions
        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_MINIMIZE = 6;

        private static Mailer _mailer; // Dodajte ovo kao privatno polje
        private static string _userEmail;

        public KeybindHandling(Mailer mailer, string userEmail) // Dodajte konstruktor za postavljanje Mailer-a
        {
            _mailer = mailer;
            _userEmail = userEmail;
        }

        public static OverlayWindow overlayWindow;

        private static Key activationKey = (Key)Enum.Parse(typeof(Key), DesktopSecurityApp.Services.UserInformationManagement.GetJSONFile().Key);

        public static void RefreshActivationKey()
        {
            SetActivationKey((Key)Enum.Parse(typeof(Key), DesktopSecurityApp.Services.UserInformationManagement.GetJSONFile().Key));
        }

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
                else if (overlayWindow != null) // Slusaj sve ostale keybindove samo kad je overlay upalje, tj kad nije null (bilo je samo else, znaci sve je pratilo)
                {
                    HandleFalseKeyBind();
                }
        }

        private static void MinimizeAllWindows()
        {
            IntPtr shellWindowHandle = GetShellWindow();
            ShowWindowAsync(shellWindowHandle, SW_MINIMIZE);
        }
        private static void HandleValidKeyBind()
        {
            if (overlayWindow == null)
            {
                MinimizeAllWindows(); // Minimize 
                OpenOverlayWindow();
                BlockCtrlAltDelete.StartBlocking(); // Start blocking Ctrl + Alt + Delete 
            }
            else
            {
                CloseOverlayWindow();
            }
        }
        public static void HandleFalseKeyBind()
        {
            try
            {
                //DotNetEnv.Env.Load();
                string DSA_username = Environment.GetEnvironmentVariable("DSA_USERNAME");

                // Pozivamo metodu CaptureAndSave sa prosljeđenom putanjom (LOGIKA ZA KAMERU)
                CameraHandling cameraHandler = new CameraHandling();
                cameraHandler.StartCamera();

                // Slanje emaila na adminovu email adresu
                _mailer.SendEmail(_userEmail, "Wrong Key-Bind !");

                Thread.Sleep(1000);
                cameraHandler.CameraStop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception(ex.ToString());
            }
        }

        private static void OpenOverlayWindow()
        {
            if (Keyboard.IsKeyDown(Key.L))
            {
                // Open the overlay window
                overlayWindow = new OverlayWindow();
                overlayWindow.Show();

            }
        }

        public static void CloseOverlayWindow()
        {
            overlayWindow.Close();
            overlayWindow = null;
            BlockCtrlAltDelete.StopBlocking(); // Stop blocking Ctrl + Alt + Delete
        }
    }
}