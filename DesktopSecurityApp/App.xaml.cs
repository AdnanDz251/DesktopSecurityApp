using System.Configuration;
using System.Data;
using System;
using System.Windows;
using System.Windows.Input;
using DesktopSecurityApp.Services;

//[assembly: ThemeInfo(
//    ResourceDictionaryLocation.None,
//    ResourceDictionaryLocation.SourceAssembly
//)]

namespace DesktopSecurityApp
{
    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Call the method from the new class to register key bindings
            KeybindHandling.RegisterKeyBindings(MainWindow);
        }
    }

}