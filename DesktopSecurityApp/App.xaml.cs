﻿using DesktopSecurityApp.Services;
using EmailSenderApp; // Dodavanje ovog using statement
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using DesktopSecurityApp.UserInterface.Views;

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
            // Ucitavanje enivonment varijabli
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.Load(dotenv);

            base.OnStartup(e);

            // Ovdje možete pozvati metodu za spremanje korisničkih informacija u JSON datoteku
            UserInformationManagement.SaveUserInfoToJsonFile();


            // Konfiguracija koja uključuje environment varijable
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            // Dohvatanje korisničkog imena iz Environment varijable

            string dsaUsername = Environment.GetEnvironmentVariable("DSA_USERNAME");
            string dsaPassword = Environment.GetEnvironmentVariable("DSA_PASWORD");

            // instanca Mailer-a sa odgovarajućim parametrima
            mailer = new Mailer("mail.skim.ba", 465, true, dsaUsername);  //Informacije maila NA KOJI SE ŠALJE MAIL !sa kojeg se s

            // instanca KeybindHandling i prosljeđuje instancu Mailer-a u konstruktor
            keyHandler = new KeybindHandling(mailer, "ajanovic.m97@gmail.com"); //STA TI JE BAA !?!?!?!

            // Pozovite metodu iz nove klase za registraciju key bindinga
            KeybindHandling.RegisterKeyBindings(MainWindow);
            // Dodajte handler za događaj tastature za key-bind otvaranja overlay-a

            // Pokretanje hooka za globalnu tastaturu
            GlobalKeyboardHook.Start();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Zaustavljanje hooka za globalnu tastaturu
            GlobalKeyboardHook.Stop();
        }
    }
}
