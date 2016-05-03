using Particle.Setup.Models;
using System;
using Windows.UI.Xaml.Data;

namespace Particle.UI.Converters
{
    #region Converters

    public class RssiToImageConverter : IValueConverter
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

            return $"ms-appx:///Particle.Setup/Assets/SoftAP/WiFiSignal/WifiSignalWhite{unsecure}{image}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
