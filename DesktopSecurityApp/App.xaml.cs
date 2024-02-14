using System.Configuration;
using System.Data;
using System;
using System.Windows;
using System.Windows.Input;
using DesktopSecurityApp.Services;
using System.Windows.Media;

//[assembly: ThemeInfo(
//    ResourceDictionaryLocation.None,
//    ResourceDictionaryLocation.SourceAssembly
//)]

namespace DesktopSecurityApp
{
    public partial class App : Application //new
    {
        private KeybindHandling keyHandler;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            keyHandler = new KeybindHandling();

            // Call the method from the new class to register key bindings
            KeybindHandling.RegisterKeyBindings(MainWindow);
            // Dodajte handler za događaj tastature za key-bind otvaranja overlay-a
        }
    }

}