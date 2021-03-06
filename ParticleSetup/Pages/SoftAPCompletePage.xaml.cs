﻿using Particle.SDK.Utils;
using Particle.Setup.Models;
using System;
using System.Reflection;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages
{
    public sealed partial class SoftAPCompletePage : SoftAPPage
    {
        #region Constructors

        public SoftAPCompletePage()
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
                if (!newName.Equals(SoftAP.SoftAPResult.ParticleDevice.Name))
                {
                    NewDeviceNameTextBox.IsEnabled = false;
                    DoneButton.IsEnabled = false;

                    ProgressBar.IsIndeterminate = true;

                    await SoftAP.SoftAPResult.ParticleDevice.RenameAsync(NewDeviceNameTextBox.Text);

                    ProgressBar.IsIndeterminate = false;
                }
            }

            var navigated = false;
            var backStack = SoftAP.CurrentSoftAPSettings.AppFrame.BackStack;
            for (var i = backStack.Count; i > 0; --i)
            {
                var pageStackEntry = backStack[i - 1];
                
                if (pageStackEntry.SourcePageType.GetTypeInfo().BaseType == typeof(SoftAPPage))
                {
                    backStack.Remove(pageStackEntry);
                }
                else if (pageStackEntry.SourcePageType == SoftAP.CurrentSoftAPSettings.CompletionPageType)
                {
                    navigated = true;
                    SoftAP.CurrentSoftAPSettings.AppFrame.GoBack();
                }
            }

            if (!navigated)
                Frame.Navigate(SoftAP.CurrentSoftAPSettings.CompletionPageType);

            SoftAP.CurrentSoftAPSettings.SoftAPExit();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            NewDeviceNameTextBox.Text = DeviceNameGenerator.GenerateUniqueName(SoftAP.CurrentSoftAPSettings.CurrentDeviceNames);
        }

        #endregion

        #region Private Methods

        private void ShowResult()
        {
            ResultImage.Source = null;
            ResultHeader.Text = "";
            ResultText.Text = "";

            ResourceLoader resourceLoader = new ResourceLoader();
            string imageSource = null;
            string headerText = null;
            string textText = null;
            bool showNameNewDevice = false;

            switch (SoftAP.SoftAPResult.Result)
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
                ResultImage.Source = new BitmapImage(new Uri($"ms-appx:///Particle.Setup/Assets/SoftAP/StatusIcons/StatusIcon{imageSource}.png"));
            if (headerText != null)
                ResultHeader.Text = headerText;
            if (textText != null)
                ResultText.Text = textText.Replace("{device}", "Photon");

            if (showNameNewDevice)
            {
                if (string.IsNullOrWhiteSpace(SoftAP.SoftAPResult.ParticleDevice.Name))
                    NewDeviceNameTextBox.Text = DeviceNameGenerator.GenerateUniqueName(SoftAP.CurrentSoftAPSettings.CurrentDeviceNames);
                else
                    NewDeviceNameTextBox.Text = SoftAP.SoftAPResult.ParticleDevice.Name;

                NameNewDevicePanel.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
