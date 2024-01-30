using System;
using System.Drawing;
using System.IO;
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

            // Kreirajte punu putanju do Documents foldera ili drugog odabranog mjesta za pohranu
            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

            // Spremite sliku na disk
            SaveImageToDisk(bitmapSource, outputPath);

            // Zaustavite kameru nakon što uhvatite jedan okvir
            videoSource.SignalToStop();
            videoSource.WaitForStop();
        }
        private void SaveImageToDisk(BitmapSource bitmapSource, string outputPath)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (var stream = File.Create(outputPath))
            {
                encoder.Save(stream);
            }

            MessageBox.Show($"Image saved to {outputPath}");
        }

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
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