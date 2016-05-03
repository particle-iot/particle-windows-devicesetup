using Particle.Setup.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages
{
    public sealed partial class SoftAPManualNetworkPage : SoftAPPage
    {
        #region Constructors

        public SoftAPManualNetworkPage()
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SoftAPSelectWiFiPage));
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            SoftAPConfig.SoftAPData.ScanAP = new SoftAPScanAP();
            SoftAPConfig.SoftAPData.ScanAP.SSID = NetworkSSID.Text;
            if (RequiresPasswordCheckBox.IsChecked.Value)
                SoftAPConfig.SoftAPData.ScanAP.Security = SecurityType.SecurityWpa2AesPsk;

            Frame.Navigate(typeof(SoftAPPasswordPage));
        }

        #endregion

        #region Private Methods

        private void SetupPage()
        {
            SoftAPConfig.SoftAPData.ScanAP = null;
            SoftAPConfig.SoftAPData.Password = null;
        }

        #endregion
    }
}
