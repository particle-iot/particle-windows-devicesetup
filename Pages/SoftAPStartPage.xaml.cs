using Particle.SDK;
using Particle.Setup.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages
{
    public sealed partial class SoftAPStartPage : SoftAPPage
    {
        #region Constructors

        public SoftAPStartPage()
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

        private async void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            ReadyButton.IsEnabled = false;

            ProgressBar.IsIndeterminate = true;

            SoftAP.SoftAPResult.Result = SoftAPSetupResult.Started;
            SoftAPConfig.SoftAPData.ClaimCode = await ParticleCloud.SharedCloud.CreateClaimCodeAsync();

            ProgressBar.IsIndeterminate = false;

            Frame.Navigate(typeof(SoftAPConnectPage));
        }

        #endregion

        #region Private Methods

        private void SetupPage()
        {
            SoftAP.SoftAPResult.Result = SoftAPSetupResult.NotStarted;
            Username.Text = SoftAP.CurrentSoftAPSettings.Username;
        }

        #endregion
    }
}
