using Particle.Setup.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages.SoftAP
{
    public sealed partial class SelectWiFiPage : SoftAPPage
    {
        #region Constructors

        public SelectWiFiPage()
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

        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ManualNetworkPage));
        }

        private void DeviceWiFiListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var scanAP = (SoftAPScanAP)e.ClickedItem;
            if (scanAP == null)
                return;

            SoftAPConfig.SoftAPData.ScanAP = scanAP;

            if (scanAP.Security == SecurityType.SecurityOpen)
                Frame.Navigate(typeof(ConfigurePage));
            else
                Frame.Navigate(typeof(PasswordPage));
        }

        private void DeviceWiFiListViewItemGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var container = (DependencyObject)sender;

            if (ParticleSetup.CurrentSetupSettings.TextForegroundColor != null)
            {
                List<TextBlock> controls = FindTypeInContainer<TextBlock>(container);
                foreach (TextBlock control in controls)
                    control.Foreground = ParticleSetup.CurrentSetupSettings.TextForegroundColor;
            }

            if (ParticleSetup.CurrentSetupSettings.MaskSetupImages != null)
            {
                List<BitmapIcon> controls = FindTypeInContainer<BitmapIcon>(container);
                foreach (var control in controls)
                    control.Foreground = ParticleSetup.CurrentSetupSettings.MaskSetupImages;
            }
        }

        private void RescanButton_Click(object sender, RoutedEventArgs e)
        {
            GetDeviceWiFiAsync();
        }

        #endregion

        #region Private Methods

        private async void GetDeviceWiFiAsync()
        {
            DeviceWiFiListView.DataContext = null;
            LoadWiFiProgress.IsActive = true;
            int maxRetries = 5;
            List<SoftAPScanAP> setupScanAPs = null;

            for (int tries = 0; tries < maxRetries; ++tries)
            {
                if (setupScanAPs == null)
                    setupScanAPs = await SoftAPConfig.GetScanAPsAsync();
            }

            LoadWiFiProgress.IsActive = false;
            DeviceWiFiListView.DataContext = setupScanAPs;
        }

        private void SetupPage()
        {
            SoftAPConfig.SoftAPData.ScanAP = null;
            SoftAPConfig.SoftAPData.Password = null;

            SetCustomization(RootGrid);

            GetDeviceWiFiAsync();
        }

        #endregion
    }
}
