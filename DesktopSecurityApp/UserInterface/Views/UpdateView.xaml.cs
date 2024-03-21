
using DesktopSecurityApp.Services;
using System.Windows;
using System.Windows.Controls;

namespace DesktopSecurityApp.UserInterface.Views
{
    public partial class UpdateView : UserControl
    {
        private bool capturingKey = false;
        private string customFolderPath = UserInformationManagement.customFolderPath;

        public UpdateView()
        {
            InitializeComponent();
            FetchJSONUserInfo(true);
            Focusable = true;
            Focus();
            currentKeybind.Content += UserInformationManagement.GetJSONFile().Key;

        }

        public async void FetchJSONUserInfo(bool firstTime)
        {
            // Get the key asynchronously
            UserInfo JSON_FILE = await Task.Run(() => UserInformationManagement.GetJSONFile());
            // Update the content
            currentKeybind.Content = firstTime ? currentKeybind.Content : currentKeybind.Content.ToString().Substring(0, currentKeybind.Content.ToString().Length - 1) + JSON_FILE.Key;
            currentUsername.Content = firstTime ? currentUsername.Content + JSON_FILE.Username : "Your current Username: " + JSON_FILE.Username;
            currentEmail.Content = firstTime ? currentEmail.Content + JSON_FILE.Email : "Your current Email: " + JSON_FILE.Email;
        }

        private async Task ReloadUpdateViewAsync()
        {

            FetchJSONUserInfo(false);
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


            if (captureButton.Content.ToString() == "Click here to change the keybind" || captureButton.Content.ToString() == "Press a key (Click to Cancel)" || captureButton.Content.ToString() == "Press a key")
                updatedUserInfo.NewKey = UserInformationManagement.GetJSONFile().Key;
            else
                updatedUserInfo.NewKey = captureButton.Content.ToString();
            updatedUserInfo.NewEmail = newEmailInput.Text.ToString();
            updatedUserInfo.NewUsername = newUsernameInput.Text.ToString();

            MessageBox.Show(updatedUserInfo.ToString());

            // Update user info in the JSON file
            try
            {
                UserUpdater.UpdateUserInfoInJsonFile(updatedUserInfo, customFolderPath);
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
            KeybindHandling.RefreshActivationKey();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdateViewModel viewModel)
            {
                viewModel.SaveActivationKey(null);
            }
        }
        private void NewEmailInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveButton.Visibility = Visibility.Visible;
        }
        private void NewUsernamelInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveButton.Visibility = Visibility.Visible;
        }

    }
}
