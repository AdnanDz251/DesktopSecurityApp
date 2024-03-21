using DesktopSecurityApp.Services;
using System.Windows.Controls;

namespace DesktopSecurityApp.UserInterface.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            updateUsername();
        }

        public async void updateUsername()
        {

            string username = await Task.Run(() => UserInformationManagement.GetJSONFile().Username.ToString());
            if (usernameText.Text.ToString() == "HI,")
                usernameText.Text = usernameText.Text + username;
        }
    }
}
