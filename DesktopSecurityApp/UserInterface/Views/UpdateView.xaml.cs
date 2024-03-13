using System.Windows;
using System.Windows.Controls;
using DesktopSecurityApp.Services;
using DesktopSecurityApp.UserInterface.ViewModels;

namespace DesktopSecurityApp.UserInterface.Views
{
    /// <summary>
    /// Interaction logic for UpdateView.xaml
    /// </summary>
    public partial class UpdateView : UserControl
    {
        public UpdateView()
        {
            InitializeComponent();
            DataContext = new UpdateViewModel(); // Dodajemo ViewModel kao DataContext
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdateViewModel viewModel)
            {
                viewModel.SaveActivationKey(null);
            }
        }

    }
}
