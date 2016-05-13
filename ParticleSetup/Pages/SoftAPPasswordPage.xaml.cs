using Particle.Setup.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages
{
    public sealed partial class SoftAPPasswordPage : SoftAPPage
    {
        #region Constructors

        public SoftAPPasswordPage()
        {
            InitializeComponent();
            RootGrid.DataContext = UI.VisibleBoundsWindow.VisibleBounds;
        }

        #endregion

        #region Override Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetupPage();
        }

        #endregion

        #region Interaction Methods

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SoftAP.BackButtonPressed();
        }

        private void ChangeNetworkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SoftAPSelectWiFiPage));
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password.Password))
                return;

            SoftAPConfig.SoftAPData.Password = Password.Password;
            Frame.Navigate(typeof(SoftAPConfigurePage));
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                switch (((Control)sender).Name)
                {
                    case "Password":
                        ConnectButton.Focus(FocusState.Pointer);
                        ConnectButton_Click(sender, null);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        private void SetupPage()
        {
            SoftAPConfig.SoftAPData.Password = null;

            ResourceLoader resourceLoader = new ResourceLoader();
            string securedWithValue;

            if (SoftAPConfig.SoftAPData.ScanAP.Security.HasFlag(SecurityType.SecurityWepPsk) || SoftAPConfig.SoftAPData.ScanAP.Security.HasFlag(SecurityType.SecurityWepShared))
                securedWithValue = "WEP";
            else if (SoftAPConfig.SoftAPData.ScanAP.Security.HasFlag(SecurityType.SecurityWpaTkipPsk) || SoftAPConfig.SoftAPData.ScanAP.Security.HasFlag(SecurityType.SecurityWpaAesPsk))
                securedWithValue = "WPA";
            else if (SoftAPConfig.SoftAPData.ScanAP.Security.HasFlag(SecurityType.SecurityWpa2AesPsk) || SoftAPConfig.SoftAPData.ScanAP.Security.HasFlag(SecurityType.SecurityWpa2TkipPsk) || SoftAPConfig.SoftAPData.ScanAP.Security.HasFlag(SecurityType.SecurityWpa2MixedPsk))
                securedWithValue = "WPA2";
            else
                securedWithValue = SetupResources.Unknown;

            SsidTextBlock.Text = SoftAPConfig.SoftAPData.ScanAP.SSID;
            SecuredWithTextBlock.Text = string.Format(SetupResources.SecuredWith, securedWithValue);
        }

        #endregion
    }
}
