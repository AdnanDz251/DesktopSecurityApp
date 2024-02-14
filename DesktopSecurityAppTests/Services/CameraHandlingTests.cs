using DesktopSecurityApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace DesktopSecurityApp.Tests
{
    [TestClass]
    public class CameraHandlingTests
    {
        [TestMethod]
        public void ConvertBitmapToBitmapSource_ConvertsBitmapToBitmapSource()
        {
            // Arrange
            CameraHandling cameraHandling = new CameraHandling();
            Bitmap bitmap = new Bitmap(100, 100);

            // Act
            var result = cameraHandling.ConvertBitmapToBitmapSource(bitmap);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BitmapSource));
            // Add more assertions if needed
        }

        [TestMethod]
        public void StartCamera_StartsCameraSuccessfully()
        {
            // Arrange
            CameraHandling cameraHandling = new CameraHandling();

            // Act
            cameraHandling.StartCamera();

            // Assert
            // Add assertions to check if the camera is started successfully
        }
    }
}