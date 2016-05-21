using Particle.Setup.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages.SoftAP
{
    public sealed partial class ManualNetworkPage : SoftAPPage
    {
        #region Constructors

        public ManualNetworkPage()
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
            ParticleSetup.BackButtonPressed();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SelectWiFiPage));
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            SoftAPConfig.SoftAPData.ScanAP = new SoftAPScanAP();
            SoftAPConfig.SoftAPData.ScanAP.SSID = NetworkSSID.Text;
            if (RequiresPasswordCheckBox.IsChecked.Value)
                SoftAPConfig.SoftAPData.ScanAP.Security = SecurityType.SecurityWpa2AesPsk;

            Frame.Navigate(typeof(PasswordPage));
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
