using DesktopSecurityApp.Services;
using System.Windows;
using System.Windows.Input;
using System.IO;
using EmailSenderApp;
using MailKit.Net.Smtp;
using MimeKit;
using DotNetEnv;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;

namespace DesktopSecurityApp.Services
{
    public class KeybindHandling
    {
        private static Mailer _mailer; // Dodajte ovo kao privatno polje
        private static string _userEmail;

        public KeybindHandling(Mailer mailer, string userEmail) // Dodajte konstruktor za postavljanje Mailer-a
        {
            _mailer = mailer;
            _userEmail = userEmail;
        }

        public static OverlayWindow overlayWindow;

        private static Key activationKey = (Key)Enum.Parse(typeof(Key), DesktopSecurityApp.Services.UserInformationManagement.GetJSONFile().Key);
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
            //DotNetEnv.Env.Load();
            string DSA_username = Environment.GetEnvironmentVariable("DSA_USERNAME");

            // Pozivamo metodu CaptureAndSave sa prosljeđenom putanjom (LOGIKA ZA KAMERU)
            CameraHandling cameraHandler = new CameraHandling();
                cameraHandler.StartCamera();

                // Slanje emaila na adminovu email adresu
                _mailer.SendEmail( _userEmail, "Wrong Key-Bind !");
        }
        private static void OpenOverlayWindow()
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

