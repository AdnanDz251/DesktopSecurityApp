using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;

namespace DesktopSecurityApp.UserInterface.Views
{
    /// <summary>
    /// Interaction logic for GalleryView.xaml
    /// </summary>
    public partial class GalleryView : UserControl
    {
        public GalleryView()
        {
            InitializeComponent();
            PopulateImages();
        }

        private void PopulateImages()
        {
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativeFolderPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executablePath).FullName).FullName).FullName).FullName, "Data", "CameraPhotos");
            string absoluteFolderPath = Path.Combine(executablePath, relativeFolderPath);
            string outputPath = Path.Combine(absoluteFolderPath);
            string imagesFolderPath = outputPath; // Specify the path to your images folder



            int rowIndex = 1; // Start from the second row
            int colIndex = 0;

            foreach (string imagePath in Directory.GetFiles(imagesFolderPath))
            {

                if (rowIndex > 3) break; // Limit the number of images to three rows

                if (colIndex >= 3)
                {
                    colIndex = 0;
                    rowIndex++;
                }

                if (rowIndex > 3) break; // Double check in case the loop exceeds three rows

                // Load the image from file
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();

                // Create an image control
                Image image = new Image();
                image.Source = bitmap;
                image.Stretch = Stretch.Fill;
                image.Margin = new Thickness(10);


                image.MouseLeftButtonDown += (sender, e) =>
                {
                    try
                    {
                        // Open the file's directory in File Explorer
                        string imageDirectory = Path.GetDirectoryName(imagePath);
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{imagePath}\"");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };

                image.Cursor = Cursors.Hand;

                // Add the image to the corresponding Grid cell
                Grid.SetRow(image, rowIndex);
                Grid.SetColumn(image, colIndex);
                ImgGrid.Children.Add(image); // Replace YourGridName with the name of your Grid control in XAML

                colIndex++;

            }
        }
    }
}
