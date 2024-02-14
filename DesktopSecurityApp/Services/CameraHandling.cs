using System;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopSecurityApp.Services
{
    public class CameraHandling
    {
        private VideoCaptureDevice videoSource;

        public void StartCamera()
        {
            // Provjerite postoji li bar jedna kamera
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }

            // Koristite prvu pronađenu kameru
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BitmapSource bitmapSource = ConvertBitmapToBitmapSource(bitmap);

            // Generirajte jedinstveno ime datoteke
            string fileName = $"capture_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            // Kreirajte relativnu putanju do CameraPhotos foldera unutar Data foldera
            string relativePath = Path.Combine(@"C:\Users\hamme\source\repos\AdnanDz251\DesktopSecurityApp\DesktopSecurityApp\Data\CameraPhotos", fileName);

            // Dobijte apsolutnu putanju do trenutnog direktorija izvršavanja
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string outputPath = Path.Combine(currentDirectory, relativePath);

            // Provjerite da li postoji direktorijum, i ako ne, kreirajte ga
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);

                // Postavite prava na folder ako je uspješno kreiran
                SetFolderSecurity(outputDirectory);

            }

            // Spremite sliku na disk
            SaveImageToDisk(bitmapSource, outputPath);

            // Zaustavite kameru nakon što uhvatite jedan okvir
            videoSource.SignalToStop();
            videoSource.WaitForStop();
        }

        private void SetFolderSecurity(string folderPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(folderPath);

            // Postavite osnovna prava na folder
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

            // Zaključajte folder
            dInfo.Attributes |= FileAttributes.Encrypted;

            MessageBox.Show($"Security settings applied to {folderPath}");
        }


        private void SaveImageToDisk(BitmapSource bitmapSource, string outputPath)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (var stream = File.Create(outputPath))
            {
                encoder.Save(stream);
            }

            // Zaključajte datoteku
            File.Encrypt(outputPath);

            MessageBox.Show($"Image saved to {outputPath}");
        }

        public BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}