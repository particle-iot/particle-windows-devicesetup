using Particle.SDK;
using Particle.Setup.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages.SoftAP
{
    public sealed partial class StartPage : SoftAPPage
    {
        #region Constructors

        public StartPage()
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

        private async void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            ParticleSetup.SoftAPResult.Result = SoftAPSetupResult.Started;

            if (ParticleSetup.AccessToken != null)
            {
                ReadyButton.IsEnabled = false;

                ProgressBar.IsIndeterminate = true;

                if (ParticleSetup.CurrentSetupSettings.IsOrganization)
                    SoftAPConfig.SoftAPData.ClaimCode = await ParticleCloud.SharedCloud.CreateClaimCodeForOrganizationAsync(ParticleSetup.CurrentSetupSettings.OrganizationSlug, ParticleSetup.CurrentSetupSettings.ProductSlug);
                else
                    SoftAPConfig.SoftAPData.ClaimCode = await ParticleCloud.SharedCloud.CreateClaimCodeAsync();

                ProgressBar.IsIndeterminate = false;
            }

            Frame.Navigate(typeof(ConnectPage));
        }

        #endregion

        #region Private Methods

        private void SetupPage()
        {
            ParticleSetup.SoftAPResult.Result = SoftAPSetupResult.NotStarted;

            SetCustomization(RootGrid);

            if (ParticleSetup.Username == null)
                Username.Visibility = Visibility.Collapsed;
            else
                Username.Text = ParticleSetup.Username;
        }

        #endregion
    }
}
