using System.Windows;                   // Moze samo ovaj da ostane

namespace DesktopSecurityApp.Services
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();

            // Subscribe to the KeyDown event
            this.PreviewKeyDown += OverlayWindow_PreviewKeyDown;

            // Set the window to be fullscreen
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;

            // Prevent the window from being moved or resized
            this.ResizeMode = ResizeMode.NoResize;

            // Subscribe to the LostFocus event
            this.LostFocus += OverlayWindow_LostFocus;

            this.Deactivated += OverlayWindow_Deactivated;
        }

        private void OverlayWindow_LostFocus(object sender, RoutedEventArgs e)
        {
            // Set focus back to the window whenever it loses focus
            this.Focus();
        }


        private void OverlayWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Output the pressed key
            MessageBox.Show($"Key pressed: {e.Key}");


            if (e.Key.ToString() == KeybindHandling.GetActivationKey().ToString())
                KeybindHandling.CloseOverlayWindow();
            else
                KeybindHandling.HandleFalseKeyBind(); 


            // Prevent the key from being forwarded to the system
            e.Handled = true;
        }

        private void OverlayWindow_Activated(object sender, EventArgs e)
        {
            this.Focus();
        }
        private void OverlayWindow_Deactivated(object sender, EventArgs e)
        {
            this.Focus();
        }
    }
}
