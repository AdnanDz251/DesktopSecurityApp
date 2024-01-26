using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DesktopSecurityApp.Services;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace DesktopSecurityApp.Services
{
    [TestFixture]
    public class InitialTesting
    {
        [Test]
        public void TestKeyBindHandlerWithValidKey()
        {
            // Arrange
            var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, new FakePresentationSource(), 0, Key.S);

            // Act
            KeybindHandling.KeyBindHandler(null, keyEventArgs);

            // Assert
            Assert.That(KeybindHandling.overlayWindow, Is.Not.Null);
        }

        [Test]
        public void TestKeyBindHandlerWithInvalidKey()
        {
            // Arrange
            var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, new FakePresentationSource(), 0, Key.A);

            // Act
            KeybindHandling.KeyBindHandler(null, keyEventArgs);

            // Assert
            Assert.That(KeybindHandling.overlayWindow, Is.Not.Null);
        }
    }

    // Dummy klasa za PresentationSource
    public class FakePresentationSource : PresentationSource
    {
        // Implementacija metoda i svojstava koji se koriste samo u testiranju
        // Implementirajte prazne metode koje ne rade ništa ili koristite null vrednosti
        protected override CompositionTarget GetCompositionTargetCore() => null;

        public override Visual RootVisual { get; set; }
        public override bool IsDisposed { get; }
    }
}
