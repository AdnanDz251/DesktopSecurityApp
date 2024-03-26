using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            int maxRows = 3; // Maximum number of rows
            int maxCols = 3; // Maximum number of columns
            int rowIndex = 0; // Start from the first row
            int colIndex = 0; // Start from the first column

            // Get the list of image paths and reverse it
            string[] imagePaths = Directory.GetFiles(imagesFolderPath);
            Array.Reverse(imagePaths);

            foreach (string imagePath in imagePaths)
            {
                if (rowIndex >= maxRows)
                {
                    break; // If maximum rows reached, exit the loop
                }

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
                image.Cursor = Cursors.Hand;

                // Set the event handler for clicking on the image
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

                // Add the image control to the Grid
                ImgGrid.Children.Add(image);
                Grid.SetRow(image, rowIndex);
                Grid.SetColumn(image, colIndex);

                // Move to the next column
                colIndex++;

                // If maximum columns reached, move to the next row
                if (colIndex >= maxCols)
                {
                    colIndex = 0;
                    rowIndex++;
                }
            }
        }
    }
}