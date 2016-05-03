using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages
{
    public sealed partial class SoftAPConnectPage : SoftAPPage
    {
        #region Constructors

        public SoftAPConnectPage()
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

        #region Private Members

        private object checkingNetworkLock = new object();
        private volatile bool checkingNetwork = false;
        private volatile bool verifiedPhoton = false;

        #endregion

        #region Event Handlers

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            await GetConnectedWiFiAsync();
        }

        #endregion

        #region Interaction Methods

        private async void OpenWiFiButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings-wifi:"));
        }

        #endregion

        #region Private Methods

        private async Task GetConnectedWiFiAsync()
        {
            lock (checkingNetworkLock)
            {
                if (checkingNetwork || verifiedPhoton)
                    return;

                checkingNetwork = true;
            }

            ConnectionProfileFilter ProfileFilter = new ConnectionProfileFilter();
            ProfileFilter.IsWlanConnectionProfile = true;
            ProfileFilter.IsConnected = true;

            ConnectionProfile photonConnectionProfile = null;

            var connectionProfilesAsync = NetworkInformation.FindConnectionProfilesAsync(ProfileFilter);
            var connectionProfiles = await connectionProfilesAsync.AsTask();
            foreach (var connectionProfile in connectionProfiles)
            {
                if (connectionProfile.ProfileName.StartsWith("Photon-") && connectionProfile.ProfileName.Length == 11)
                {
                    photonConnectionProfile = connectionProfile;
                    break;
                }
            }

            if (photonConnectionProfile == null)
            {
                checkingNetwork = false;
                return;
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                verifiedPhoton = await VerifyPhotonConnectionAsync(photonConnectionProfile);
                if (verifiedPhoton)
                {
                    checkingNetwork = false;
                    NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;
                    Frame.Navigate(typeof(SoftAPSelectWiFiPage));
                }
                else
                {
                    checkingNetwork = false;
                }
            });
        }

        private async void ScanForNetworkChangeAsync()
        {
            await GetConnectedWiFiAsync();

            if (verifiedPhoton)
                return;

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        private void SetupPage()
        {
            SoftAPConfig.SoftAPData.Version = null;
            SoftAPConfig.SoftAPData.DeviceId = null;
            SoftAPConfig.SoftAPData.PublicKey = null;

            ScanForNetworkChangeAsync();
        }

        private async Task<bool> VerifyPhotonConnectionAsync(ConnectionProfile connectionProfile)
        {
            PhotonNetworkTextBlock.Text = connectionProfile.ProfileName;
            ProgressBar.IsIndeterminate = true;
            int setClaimCode = -1;
            int maxRetries = 5;

            for (int tries = 0; tries < maxRetries; ++tries)
            {
                if (SoftAPConfig.SoftAPData.Version == null)
                    SoftAPConfig.SoftAPData.Version = await SoftAPConfig.GetVersionAsync();
                if (SoftAPConfig.SoftAPData.DeviceId == null)
                    SoftAPConfig.SoftAPData.DeviceId = await SoftAPConfig.GetDeviceIdAsync();
                if (SoftAPConfig.SoftAPData.PublicKey == null)
                    SoftAPConfig.SoftAPData.PublicKey = await SoftAPConfig.GetPublicKeyAsync();
                if (setClaimCode == -1)
                    setClaimCode = await SoftAPConfig.SetClaimCodeAsync(SoftAPConfig.SoftAPData.ClaimCode);
            }

            ProgressBar.IsIndeterminate = false;

            if (SoftAPConfig.SoftAPData.Version == null
                || SoftAPConfig.SoftAPData.DeviceId == null
                || SoftAPConfig.SoftAPData.PublicKey == null
                || setClaimCode == -1)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
