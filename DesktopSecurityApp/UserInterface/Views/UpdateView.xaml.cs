using DesktopSecurityApp.Services;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DesktopSecurityApp.Models;

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


            MessageBox.Show($"{updatedUserInfo}");

            // Update user info in the JSON file
            try
            {
                UserUpdater.UpdateUserInfoInJsonFile(updatedUserInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Display a message indicating that the key has been updated
            MessageBox.Show($"New key saved successfully! {updatedUserInfo}");

            // Reset the UI
            capturingKey = false;
            captureButton.Content = "Press a key";
            saveButton.Visibility = Visibility.Collapsed;
        }
    }
}
