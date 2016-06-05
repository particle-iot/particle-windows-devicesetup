using Particle.SDK;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Particle.Setup.Pages.Auth
{
    public sealed partial class SignupPage : AuthPage
    {
        #region Constructors

        public SignupPage()
        {
            InitializeComponent();
            RootGrid.DataContext = UI.VisibleBoundsWindow.VisibleBounds;
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetupPage();
        }

        #region Interaction Methods

        private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                switch (((Control)sender).Name)
                {
                    case "Email":
                        Password.Focus(FocusState.Keyboard);
                        break;
                    case "Password":
                        PasswordAgain.Focus(FocusState.Keyboard);
                        break;
                    case "PasswordAgain":
                        SignupButton.Focus(FocusState.Pointer);
                        SignupButton_Click(sender, null);
                        break;
                    default:
                        break;
                }
            }
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            EmailError.Visibility = Visibility.Collapsed;
            PasswordError.Visibility = Visibility.Collapsed;
            PasswordAgainError.Visibility = Visibility.Collapsed;
            ErrorBorder.Visibility = Visibility.Collapsed;

            string errorText = "";
            var email = Email.Text;
            var password = Password.Password;
            var passwordAgain = PasswordAgain.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                errorText += SetupResources.ErrorEmailIsRequired;
                EmailError.Visibility = Visibility.Visible;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                if (errorText.Length != 0)
                    errorText += "\n";
                errorText += SetupResources.ErrorPasswordIsRequired;
                PasswordError.Visibility = Visibility.Visible;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(passwordAgain))
                {
                    if (errorText.Length != 0)
                        errorText += "\n";
                    errorText += SetupResources.ErrorPasswordsMustMatch;
                    PasswordAgainError.Visibility = Visibility.Visible;
                }
                else
                {
                    if (!password.Equals(passwordAgain))
                    {
                        errorText += SetupResources.ErrorPasswordsMustMatch;
                    }
                }
            }

            if (errorText.Length != 0)
            {
                ErrorText.Text = errorText;
                ErrorBorder.Visibility = Visibility.Visible;
                return;
            }

            SetEnableState(false);

            bool success;
            if (ParticleSetup.CurrentSetupSettings.IsOrganization)
                success = await ParticleCloud.SharedCloud.SignupWithCustomerAsync(ParticleSetup.CurrentSetupSettings.OrganizationSlug, email, password);
            else
                success = await ParticleCloud.SharedCloud.SignupAsync(email, password);

            if (success)
            {
                ParticleSetup.Login();
                ParticleSetup.End();
            }
            else
            {
                ErrorText.Text = SetupResources.CreateCredentialsError;
                ErrorBorder.Visibility = Visibility.Visible;
            }

            SetEnableState(true);
        }

        private void SkipAuthenticationButton_Click(object sender, RoutedEventArgs e)
        {
            ParticleSetup.End();
        }

        #endregion

        #region Private Methods

        private void SetEnableState(bool enabled)
        {
            ProgressBar.IsIndeterminate = !enabled;

            SignupButton.IsEnabled = enabled;

            Email.IsEnabled = enabled;
            Password.IsEnabled = enabled;
            PasswordAgain.IsEnabled = enabled;
            HaveAccountHyperlink.IsEnabled = enabled;
        }

        private void SetupPage()
        {
            SetCustomization(RootGrid);
            TermsHyperlink.NavigateUri = ParticleSetup.CurrentSetupSettings.TermsOfServiceLinkURL;
            PrivacyHyperlink.NavigateUri = ParticleSetup.CurrentSetupSettings.PrivacyPolicyLinkURL;
        }

        #endregion
    }
}
