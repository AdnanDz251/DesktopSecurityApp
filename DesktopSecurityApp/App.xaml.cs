using System.Configuration;
using System.Data;
using System;
using System.Windows;
using System.Windows.Input;
using DesktopSecurityApp.Services;
using System.Windows.Media;
using EmailSenderApp; // Dodavanje ovog using statement
using DotNetEnv;

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

            // Dohvatanje korisničkog imena iz Environment varijable
            DotNetEnv.Env.Load();
            string userEmail = Environment.GetEnvironmentVariable("Email");

            // instanca Mailer-a sa odgovarajućim parametrima
            mailer = new Mailer("smtp.gmail.com", 465, true, userEmail);  //Informacije maila NA KOJI SE ŠALJE MAIL

            // instanca KeybindHandling i prosljeđuje instancu Mailer-a u konstruktor
            keyHandler = new KeybindHandling(mailer, "ajanovic.m97@gmail.com"); //STA TI JE BAA !?!?!?!

            // Pozovite metodu iz nove klase za registraciju key bindinga
            KeybindHandling.RegisterKeyBindings(MainWindow);
            // Dodajte handler za događaj tastature za key-bind otvaranja overlay-a
        }
    }
}
