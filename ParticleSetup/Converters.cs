using Particle.Setup.Models;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Particle.Setup.Converters
{
    #region Converters

    internal class BrandImageBackgroundConverter : IValueConverter
    {
        static SolidColorBrush headerTransparency = new SolidColorBrush(Color.FromArgb(0x46, 0, 0, 0));

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (ParticleSetup.CurrentSetupSettings == null)
                return headerTransparency;
            else
                return ParticleSetup.CurrentSetupSettings.BrandImageBackgroundColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class BrandImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (ParticleSetup.CurrentSetupSettings == null)
                return $"ms-appx:///Particle.Setup/Assets/Setup/Logo.png";
            else
                return ParticleSetup.CurrentSetupSettings.BrandImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class BrandNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (ParticleSetup.CurrentSetupSettings == null)
                return "";
            else if (ParticleSetup.CurrentSetupSettings.BrandImage == null)
                return ParticleSetup.CurrentSetupSettings.BrandName;
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class DeviceImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (ParticleSetup.CurrentSetupSettings == null)
                return "ms-appx:///Particle.Setup/Assets/Setup/Device.png";
            else
                return ParticleSetup.CurrentSetupSettings.DeviceImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class PageBackgroundImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Brush brush = null;

            if (ParticleSetup.CurrentSetupSettings == null)
            {
                brush = null;
            }
            else if (ParticleSetup.CurrentSetupSettings.PageBackgroundImage != null)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = ParticleSetup.CurrentSetupSettings.PageBackgroundImage;
                brush = imageBrush;
            }
            else if (ParticleSetup.CurrentSetupSettings.PageBackgroundColor != null)
            {
                brush = ParticleSetup.CurrentSetupSettings.PageBackgroundColor;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class RssiToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // images from c:\Windows\System32\WlanMM.dll
            // quality formula from http://www.speedguide.net/faq/how-does-rssi-dbm-relate-to-signal-quality-percent-439

            var scanAP = (SoftAPScanAP)value;
            string image;
            string unsecure = "";

            if (scanAP.Security == SecurityType.SecurityOpen)
                unsecure = "unsecure";

            int quality = 2 * (scanAP.RSSI + 100);

            if (quality > 80)
                image = "4";
            else if (quality > 50)
                image = "3";
            else if (quality > 30)
                image = "2";
            else
                image = "1";

            return $"ms-appx:///Particle.Setup/Assets/Setup/WiFiSignal/WifiSignalWhite{unsecure}{image}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
