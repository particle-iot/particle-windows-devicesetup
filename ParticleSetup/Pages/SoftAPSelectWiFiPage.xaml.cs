using Particle.Setup.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages
{
    public sealed partial class SoftAPSelectWiFiPage : SoftAPPage
    {
        #region Constructors

        public SoftAPSelectWiFiPage()
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

        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SoftAPManualNetworkPage));
        }

        private void PhotonWiFiListBox_ItemClick(object sender, ItemClickEventArgs e)
        {
            var scanAP = (SoftAPScanAP)e.ClickedItem;
            if (scanAP == null)
                return;

            SoftAPConfig.SoftAPData.ScanAP = scanAP;

            if (scanAP.Security == SecurityType.SecurityOpen)
                Frame.Navigate(typeof(SoftAPConfigurePage));
            else
                Frame.Navigate(typeof(SoftAPPasswordPage));
        }

        private void RescanButton_Click(object sender, RoutedEventArgs e)
        {
            GetPhotonWiFiAsync();
        }

        #endregion

        #region Private Methods

        private async void GetPhotonWiFiAsync()
        {
            PhotonWiFiListBox.DataContext = null;
            LoadWiFiProgress.IsActive = true;
            int maxRetries = 5;
            List<SoftAPScanAP> setupScanAPs = null;

            for (int tries = 0; tries < maxRetries; ++tries)
            {
                if (setupScanAPs == null)
                    setupScanAPs = await SoftAPConfig.GetScanAPsAsync();
            }

            LoadWiFiProgress.IsActive = false;
            PhotonWiFiListBox.DataContext = setupScanAPs;
        }

        private void SetupPage()
        {
            SoftAPConfig.SoftAPData.ScanAP = null;
            SoftAPConfig.SoftAPData.Password = null;

            GetPhotonWiFiAsync();
        }

        #endregion
    }
}
