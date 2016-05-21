using Particle.SDK;
using Particle.Setup.Models;
using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages
{
    public sealed partial class SoftAPConfigurePage : SoftAPPage
    {
        #region Constructors

        public SoftAPConfigurePage()
        {
            InitializeComponent();
            RootGrid.DataContext = UI.VisibleBoundsWindow.VisibleBounds;
        }

        #endregion

        #region Override Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StartConfigure();
        }

        #endregion

        #region Private Members

        private int configurIndex = 0;
        private bool hasInternetAccess = false;

        #endregion

        #region Event Handlers

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            hasInternetAccess = false;

            ConnectionProfileFilter ProfileFilter = new ConnectionProfileFilter();
            ProfileFilter.IsConnected = true;

            var connectionProfilesAsync = NetworkInformation.FindConnectionProfilesAsync(ProfileFilter);
            var connectionProfiles = await connectionProfilesAsync.AsTask();
            foreach (var connectionProfile in connectionProfiles)
            {
                if (!connectionProfile.ProfileName.StartsWith("Photon-"))
                {
                    hasInternetAccess = connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
                    if (hasInternetAccess)
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        private void StartConfigure() // STEP 0
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

            WiFiName.Text = SoftAPConfig.SoftAPData.ScanAP.SSID;

            Step1Grid.Visibility = Visibility.Collapsed;
            Step2Grid.Visibility = Visibility.Collapsed;
            Step3Grid.Visibility = Visibility.Collapsed;
            Step4Grid.Visibility = Visibility.Collapsed;
            Step5Grid.Visibility = Visibility.Collapsed;

            ConfigureDeviceAsync();
        }

        private async void ConfigureDeviceAsync() // STEP 1
        {
            Step1ProgressRing.IsActive = true;
            Step1Grid.Visibility = Visibility.Visible;

            int result = await SoftAPConfig.SetConfigureAPAsync(configurIndex, SoftAPConfig.SoftAPData.ScanAP, SoftAPConfig.SoftAPData.Password, SoftAPConfig.SoftAPData.PublicKey);

            Step1ProgressRing.IsActive = false;
            Step1Checkmark.Visibility = Visibility.Visible;

            if (result == 0)
                ConnectDeviceAsync();
            else
                FailureToConfigure();
        }

        private async void ConnectDeviceAsync() // STEP 2
        {
            Step2ProgressRing.IsActive = true;
            Step2Grid.Visibility = Visibility.Visible;

            int result = await SoftAPConfig.SetConnectAPAsync(configurIndex);

            Step2ProgressRing.IsActive = false;
            Step2Checkmark.Visibility = Visibility.Visible;

            if (result == 0)
                CloudConnectionAsync();
            else
                FailureToConfigure();
        }

        private async void CloudConnectionAsync() // STEP 3
        {
            Step3ProgressRing.IsActive = true;
            Step3Grid.Visibility = Visibility.Visible;

            await Task.Delay(3000);

            Step3ProgressRing.IsActive = false;
            Step3Checkmark.Visibility = Visibility.Visible;

            WaitForInternetAsync();
        }

        private async void WaitForInternetAsync() // STEP 4
        {
            Step4ProgressRing.IsActive = true;
            Step4Grid.Visibility = Visibility.Visible;

            while (hasInternetAccess == false)
                await Task.Delay(500);

            Step4ProgressRing.IsActive = false;
            Step4Checkmark.Visibility = Visibility.Visible;

            VerifyOwnershipAsync();
        }

        private async void VerifyOwnershipAsync() // STEP 5
        {
            Step5ProgressRing.IsActive = true;
            Step5Grid.Visibility = Visibility.Visible;

            var devices = await ParticleCloud.SharedCloud.GetDevicesAsync();
            foreach (var device in devices)
            {
                if (device.Id == SoftAPConfig.SoftAPData.DeviceId.Id)
                {
                    ParticleSetup.SoftAPResult.ParticleDevice = device;
                    break;
                }
            }

            if (ParticleSetup.SoftAPResult.ParticleDevice == null)
            {
                ParticleSetup.SoftAPResult.Result = SoftAPSetupResult.SuccessUnknown;
            }
            else
            {
                await ParticleSetup.SoftAPResult.ParticleDevice.RefreshAsync();
                if (ParticleSetup.SoftAPResult.ParticleDevice.Connected)
                    ParticleSetup.SoftAPResult.Result = SoftAPSetupResult.Success;
                else
                    ParticleSetup.SoftAPResult.Result = SoftAPSetupResult.SuccessDeviceOffline;
            }

            Step5ProgressRing.IsActive = false;
            Step5Checkmark.Visibility = Visibility.Visible;

            Frame.Navigate(typeof(SoftAPCompletePage));
        }

        private void FailureToConfigure()
        {
            ParticleSetup.SoftAPResult.Result = SoftAPSetupResult.FailureConfigure;
            Frame.Navigate(typeof(SoftAPCompletePage));
        }

        #endregion
    }
}
