using System.Configuration;
using System.Data;
using System;
using System.Windows;
using System.Windows.Input;
using DesktopSecurityApp.Services;
using System.Windows.Media;
using EmailSenderApp; // Dodavanje ovog using statement

//[assembly: ThemeInfo(
//    ResourceDictionaryLocation.None,
//    ResourceDictionaryLocation.SourceAssembly
//)]

namespace DesktopSecurityApp
{
    public partial class App : Application
    {
        private KeybindHandling keyHandler;
        private Mailer mailer; // Dodana varijabla

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // instanca Mailer-a sa odgovarajućim parametrima
            mailer = new Mailer("sender@example.com", 587, true, "yourusername", "yourpassword");

            // instanca KeybindHandling i prosljeđuje instancu Mailer-a u konstruktor
            keyHandler = new KeybindHandling(mailer);

            // Pozovite metodu iz nove klase za registraciju key bindinga
            KeybindHandling.RegisterKeyBindings(MainWindow);
            // Dodajte handler za događaj tastature za key-bind otvaranja overlay-a
        }
    }
}
