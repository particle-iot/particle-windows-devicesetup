using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Particle.Setup
{
    /// <summary>
    /// Delegete define for callbacks in setup
    /// </summary>
    public delegate void SetupEventHandler();

    /// <summary>
    /// A class for setting options for the setup process
    /// </summary>
    public class SetupConfig
    {
        #region Events

        /// <summary>
        /// Global event called by setup when the process exits
        /// </summary>
        public event SetupEventHandler OnSetupExit;

        /// <summary>
        /// Global event called by setup when the user logs in
        /// </summary>
        public event SetupEventHandler OnSetupLogin;

        /// <summary>
        /// Global event called by setup when the user logs out
        /// </summary>
        public event SetupEventHandler OnSetupLogout;

        #endregion

        #region Public Members

        /// <summary>
        /// The main application frame
        /// </summary>
        public Frame AppFrame { get; set; }

        /// <summary>
        /// The page to show after completion
        /// </summary>
        public Type CompletionPageType { get; set; }

        /// <summary>
        /// Allow the user to skip authentication
        /// </summary>
        public bool CanSkipAuthentication { get; set; }

        /// <summary>
        /// A HashSet of current device names to check against when assigning a name
        /// </summary>
        public HashSet<string> CurrentDeviceNames { get; set; }

        /// <summary>
        /// Device/product name
        /// </summary>
        public string DeviceName { get; set; } = "Particle device";

        /// <summary>
        /// Custom product image to display on "Get ready" page
        /// </summary>
        public ImageSource DeviceImage { get; set; } = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Particle.Setup/Assets/Setup/Device.png"));

        /// <summary>
        /// Your brand name
        /// </summary>
        public string BrandName { get; set; } = "Particle";

        /// <summary>
        /// Your brand logo to fit in header setup pages
        /// </summary>
        public ImageSource BrandImage { get; set; } = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Particle.Setup/Assets/Setup/Logo.png"));

        /// <summary>
        /// The color of the LED when product is in listen mode
        /// </summary>
        public string ListenModeLEDColorName { get; set; } = "blue";

        /// <summary>
        /// The mode button name on your product
        /// </summary>
        public string ModeButtonText { get; set; } = "mode button";

        /// <summary>
        /// The SSID prefix of the Soft AP Wi-Fi network of your product while in listen mode
        /// </summary>
        public string NetworkNamePrefix { get; set; } = "Photon";

        /// <summary>
        /// URL for terms of service of the app/device usage
        /// </summary>
        public Uri TermsOfServiceLinkURL { get; set; } = new Uri("https://www.particle.io/tos");

        /// <summary>
        /// URL for privacy policy of the app/device usage
        /// </summary>
        public Uri PrivacyPolicyLinkURL { get; set; } = new Uri("https://www.particle.io/privacy");

        /// <summary>
        /// Background color for header / brand image
        /// </summary>
        public SolidColorBrush BrandImageBackgroundColor { get; set; } = null;

        /// <summary>
        /// Background color for setup pages
        /// </summary>
        public SolidColorBrush PageBackgroundColor { get; set; } = null;

        /// <summary>
        /// Background image for setup pages (replaces PageBackgroundColor)
        /// </summary>
        public ImageSource PageBackgroundImage { get; set; } = null;

        /// <summary>
        /// Normal text color
        /// </summary>
        public SolidColorBrush TextForegroundColor { get; set; } = null;

        /// <summary>
        /// Link text color (will be underlined)
        /// </summary>
        public SolidColorBrush LinkForegroundColor { get; set; } = null;

        /// <summary>
        /// Buttons/spinners background color
        /// </summary>
        public SolidColorBrush ElementBackgroundColor { get; set; } = null;

        /// <summary>
        /// Buttons text color
        /// </summary>
        public SolidColorBrush ElementForegroundColor { get; set; } = null;

        /// <summary>
        /// This will mask the checkmark/warning/wifi symbols in the setup process to match text color (useful for light backgrounds)
        /// </summary>
        public SolidColorBrush MaskSetupImages { get; set; } = null;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Internal function to signal setup exiting
        /// </summary>
        internal void SetupExit()
        {
            OnSetupExit?.Invoke();
        }

        /// <summary>
        /// Internal function to signal setup exiting
        /// </summary>
        internal void SetupLogin()
        {
            OnSetupLogin?.Invoke();
        }

        /// <summary>
        /// Internal function to signal setup exiting
        /// </summary>
        internal void SetupLogout()
        {
            OnSetupLogout?.Invoke();
        }

        #endregion
    }
}
