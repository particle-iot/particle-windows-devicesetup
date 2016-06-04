using Particle.SDK;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages.Auth
{
    public sealed partial class TokenLoginPage : AuthPage
    {
        #region Constructors

        public TokenLoginPage()
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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            TokenError.Visibility = Visibility.Collapsed;
            ErrorBorder.Visibility = Visibility.Collapsed;

            string errorText = "";
            var token = Token.Text;

            if (string.IsNullOrWhiteSpace(token))
            {
                errorText += SetupResources.ErrorTokenIsRequired;
                TokenError.Visibility = Visibility.Visible;
            }

            if (errorText.Length != 0)
            {
                ErrorText.Text = errorText;
                ErrorBorder.Visibility = Visibility.Visible;
                return;
            }

            SetEnableState(false);

            var success = await ParticleCloud.SharedCloud.TokenLoginAsync(token);

            if (success)
            {
                ParticleSetup.Login();
                ParticleSetup.End();
            }
            else
            {
                ErrorText.Text = SetupResources.AuthCredentialsError;
                ErrorBorder.Visibility = Visibility.Visible;
            }

            SetEnableState(true);
        }

        private void NoTokenButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                switch (((Control)sender).Name)
                {
                    case "Token":
                        LoginButton.Focus(FocusState.Pointer);
                        LoginButton_Click(sender, null);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        private void SetEnableState(bool enabled)
        {
            ProgressBar.IsIndeterminate = !enabled;

            LoginButton.IsEnabled = enabled;

            Token.IsEnabled = enabled;
            NoTokenHyperlink.IsEnabled = enabled;
        }

        private void SetupPage()
        {
            SetCustomization(RootGrid);
        }

        #endregion
    }
}
