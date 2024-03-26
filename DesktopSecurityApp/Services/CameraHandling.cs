using AForge.Video;
using AForge.Video.DirectShow;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DesktopSecurityApp.Services
{
    public class CameraHandling
    {
        private VideoCaptureDevice? videoSource;

        public void CameraStop()
        {
            try
            {
                Debug.WriteLine("Camera stop");
                videoSource.SignalToStop();
                videoSource.NewFrame -= new NewFrameEventHandler(VideoSource_NewFrame);
                videoSource = null;
                //videoSource.WaitForStop();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void StartCamera()
        {
            try
            {
                // Provjerite postoji li bar jedna kamera
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                {
                    MessageBox.Show("No video devices found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // Koristite prvu pronađenu kameru
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
                videoSource.Start();
                //Thread.Sleep(2000);
                // CameraStop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception();
            }
        }



        private async void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BitmapSource bitmapSource = ConvertBitmapToBitmapSource(bitmap);

            // Generirajte jedinstveno ime datoteke
            string fileName = $"capture_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            // Dobijte apsolutnu putanju do direktorija izvršne datoteke aplikacije
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;

            // Kreirajte relativnu putanju do CameraPhotos foldera unutar Data foldera
            string relativeFolderPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executablePath).FullName).FullName).FullName).FullName, "Data", "CameraPhotos");

            // Dobijte apsolutnu putanju do CameraPhotos foldera
            string absoluteFolderPath = Path.Combine(executablePath, relativeFolderPath);

            // Kreirajte putanju za izlazni fajl kombinovanjem apsolutne putanje CameraPhotos foldera sa imenom fajla
            string outputPath = Path.Combine(absoluteFolderPath, fileName);

            // Provjerite da li postoji direktorijum, i ako ne, kreirajte ga
            if (!Directory.Exists(absoluteFolderPath))
            {
                Directory.CreateDirectory(absoluteFolderPath);

                // Postavite prava na folder ako je uspješno kreiran
                SetFolderSecurity(absoluteFolderPath);
            }

            // Spremite sliku na disk
            SaveImageToDisk(bitmapSource, outputPath);

            // Zaustavite kameru nakon što uhvatite jedan okvir
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

            WindowManager.MinimizeTaskbar();
            MessageBox.Show($"Image saved to {outputPath}");
            WindowManager.RestoreTaskbar();
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