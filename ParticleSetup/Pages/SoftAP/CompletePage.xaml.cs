using Particle.SDK.Utils;
using Particle.Setup.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages.SoftAP
{
    public sealed partial class CompletePage : SoftAPPage
    {
        #region Constructors

        public CompletePage()
        {
            InitializeComponent();
            RootGrid.DataContext = UI.VisibleBoundsWindow.VisibleBounds;
        }

        #endregion

        #region Override Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowResult();
        }

        #endregion

        #region Interaction Methods

        private async void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameNewDevicePanel.Visibility == Visibility.Visible)
            {
                var newName = NewDeviceNameTextBox.Text;
                if (!newName.Equals(ParticleSetup.SoftAPResult.ParticleDevice.Name))
                {
                    NewDeviceNameTextBox.IsEnabled = false;
                    DoneButton.IsEnabled = false;

                    ProgressBar.IsIndeterminate = true;

                    await ParticleSetup.SoftAPResult.ParticleDevice.RenameAsync(NewDeviceNameTextBox.Text);

                    ProgressBar.IsIndeterminate = false;
                }
            }

            ParticleSetup.End();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            NewDeviceNameTextBox.Text = DeviceNameGenerator.GenerateUniqueName(ParticleSetup.CurrentSetupSettings.CurrentDeviceNames);
        }

        #endregion

        #region Private Methods

        private void ShowResult()
        {
            ResultImage.Source = null;
            ResultHeader.Text = "";
            ResultText.Text = "";

            string imageSource = null;
            string headerText = null;
            string textText = null;
            bool showNameNewDevice = false;

            switch (ParticleSetup.SoftAPResult.Result)
            {
                case SoftAPSetupResult.NotStarted:
                case SoftAPSetupResult.Started:
                    break;
                case SoftAPSetupResult.Success:
                    imageSource = "Success";
                    headerText = SetupResources.SoftAPCompletedHeaderSuccess;
                    textText = SetupResources.SoftAPCompletedTextSuccess;
                    showNameNewDevice = true;
                    break;
                case SoftAPSetupResult.SuccessUnknown:
                    imageSource = "Success";
                    headerText = SetupResources.SoftAPCompletedHeaderSuccessUnknown;
                    textText = SetupResources.SoftAPCompletedTextSuccessUnknown;
                    break;
                case SoftAPSetupResult.SuccessDeviceOffline:
                    imageSource = "Warning";
                    headerText = SetupResources.SoftAPCompletedHeaderSuccessDeviceOffline;
                    textText = SetupResources.SoftAPCompletedTextSuccessDeviceOffline;
                    break;
                case SoftAPSetupResult.FailureClaiming:
                    break;
                case SoftAPSetupResult.FailureConfigure:
                    imageSource = "Failure";
                    headerText = SetupResources.SoftAPCompletedHeaderFailureConfigure;
                    textText = SetupResources.SoftAPCompletedTextFailureConfigure;
                    break;
                case SoftAPSetupResult.FailureCannotDisconnectFromDevice:
                case SoftAPSetupResult.FailureLostConnectionToDevice:
                    break;
            }

            if (imageSource != null)
                ResultImage.Source = new BitmapImage(new Uri($"ms-appx:///Particle.Setup/Assets/Setup/StatusIcons/StatusIcon{imageSource}.png"));
            if (headerText != null)
                ResultHeader.Text = headerText;
            if (textText != null)
                ResultText.Text = textText.Replace("{device}", "Photon");

            if (showNameNewDevice)
            {
                if (string.IsNullOrWhiteSpace(ParticleSetup.SoftAPResult.ParticleDevice.Name))
                    NewDeviceNameTextBox.Text = DeviceNameGenerator.GenerateUniqueName(ParticleSetup.CurrentSetupSettings.CurrentDeviceNames);
                else
                    NewDeviceNameTextBox.Text = ParticleSetup.SoftAPResult.ParticleDevice.Name;

                NameNewDevicePanel.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
