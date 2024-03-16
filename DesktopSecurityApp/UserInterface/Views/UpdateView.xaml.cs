using DesktopSecurityApp.Services;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using DesktopSecurityApp.Models;
using DesktopSecurityApp.Services;

namespace DesktopSecurityApp.UserInterface.Views
{
    public partial class UpdateView : UserControl
    {
        private bool capturingKey = false;

        public UpdateView()
        {
            InitializeComponent();
            Focusable = true;
            Focus();
            currentKeybind.Content += UserInformationManagement.GetJSONFile().Key;

        }

        private async Task ReloadUpdateViewAsync()
        {
            // Get the key asynchronously
            string key = await Task.Run(() => UserInformationManagement.GetJSONFile().Key);

            // Update the content
            currentKeybind.Content = currentKeybind.Content.ToString().Substring(0, currentKeybind.Content.ToString().Length - 1) + key;

            // Reinitialize the UpdateView
            InitializeComponent();
        }


        private void captureButton_Click(object sender, RoutedEventArgs e)
        {
            capturingKey = true;
            captureButton.Content = "Press a key (Esc to cancel)";
        }

        public class UpdateUserInfo
        {
            public string NewUsername { get; set; }
            public string NewKey { get; set; }
            public string NewEmail { get; set; }
        }


        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (capturingKey)
            {
                if (e.Key == System.Windows.Input.Key.Escape)
                {
                    capturingKey = false;
                    captureButton.Content = "Press a key";
                }
                else
                {
                    // Set the captured key as the new keybind
                    string newKeybind = e.Key.ToString();
                    // You may need to convert the key to a more user-friendly format if necessary
                    // For example, converting "Oem5" to "]" or "D5" to "5"

                    // Display the captured keybind
                    captureButton.Content = newKeybind;

                    // Show the Save button
                    saveButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of UpdateUserInfo
            DesktopSecurityApp.Models.UpdateUserInfo updatedUserInfo = new DesktopSecurityApp.Models.UpdateUserInfo();

            // Populate the properties with the new information
            updatedUserInfo.NewKey = captureButton.Content.ToString(); // Replace "NewKeyHere" with the new key

            // Update user info in the JSON file
            try
            {
                UserUpdater.UpdateUserInfoInJsonFile(updatedUserInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Display a message indicating that the key has been update

            // Reset the UI
            capturingKey = false;
            captureButton.Content = "Press a key";
            saveButton.Visibility = Visibility.Collapsed;
            ReloadUpdateViewAsync();
        }
    }
}
