using Particle.SDK;
using Particle.Setup.Models;
using System;
using System.Reflection;
using Windows.Storage;

namespace Particle.Setup
{
    /// <summary>
    /// A class for Authenticating a Particle user and setting up new devices
    /// </summary>
    public class ParticleSetup
    {
        #region Private Static Members

        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private static SoftAPResult softAPResult = null;

        #endregion

        #region Properties

        /// <summary>
        /// The last saved AccessToken from a previous login
        /// </summary>
        public static string AccessToken
        {
            get
            {
                if (localSettings.Values.ContainsKey("AccessToken"))
                    return (string)localSettings.Values["AccessToken"];
                else
                    return null;
            }
        }
        
        /// <summary>
        /// Has this device been signed in before
        /// </summary>
        public static bool HasSignedIn
        {
            get
            {
                return localSettings.Values.ContainsKey("HasSignedIn");
            }
        }

        /// <summary>
        /// Instance of a static SoftAPData for simplicity
        /// </summary>
        public static SoftAPResult SoftAPResult
        {
            get
            {
                if (softAPResult == null)
                    ResetSoftAPResult();

                return softAPResult;
            }
        }

        /// <summary>
        /// The last saved Username from a previous login
        /// </summary>
        public static string Username
        {
            get
            {
                if (localSettings.Values.ContainsKey("Username"))
                    return (string)localSettings.Values["Username"];
                else
                    return "";
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// The currently used SetupConfig
        /// </summary>
        internal static SetupConfig CurrentSetupSettings { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Function to call when back button is pressed (on page or hardware/soft button) to go back and/or exit
        /// </summary>
        public static bool BackButtonPressed()
        {
            if (ParticleSetup.SoftAPResult.Result != SoftAPSetupResult.Started)
            {
                CurrentSetupSettings.SetupExit();
                if (ParticleSetup.SoftAPResult.Result != SoftAPSetupResult.NotStarted)
                    return false;
            }

            if (CurrentSetupSettings.AppFrame.CanGoBack)
            {
                CurrentSetupSettings.AppFrame.GoBack();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to save login data
        /// </summary>
        public static void Login()
        {
            localSettings.Values["HasSignedIn"] = true;
            localSettings.Values["AccessToken"] = ParticleCloud.SharedCloud.AccessToken;
            localSettings.Values["Username"] = ParticleCloud.SharedCloud.Username;

            CurrentSetupSettings.SetupLogin();
        }

        /// <summary>
        /// Method to remove login data
        /// </summary>
        public static void Logout()
        {
            ParticleCloud.SharedCloud.Logout();

            RemoveLocalSetting("AccessToken");
            RemoveLocalSetting("Username");

            CurrentSetupSettings.SetupLogout();
            CurrentSetupSettings.AppFrame.Navigate(typeof(Pages.Auth.LoginPage));

            NavigateAndClearBackStack(gotoCompletion: false, clearAllPages: true);
        }

        /// <summary>
        /// Reset the internal SoftAPResult class
        /// </summary>
        public static void ResetSoftAPResult()
        {
            softAPResult = new SoftAPResult();
        }

        /// <summary>
        /// Start the soft AP process
        /// </summary>
        /// <param name="softAPSettings"></param>
        public static void Start(SetupConfig softAPSettings, bool authenticationOnly = false)
        {
            ResetSoftAPResult();
            SoftAPConfig.ResetSoftAPData();

            CurrentSetupSettings = softAPSettings;

            Type startPage = null;

            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                if (HasSignedIn)
                    startPage = typeof(Pages.Auth.LoginPage);
                else
                    startPage = typeof(Pages.Auth.SignupPage);
            }
            else if (authenticationOnly)
                End();
            else
                startPage = typeof(Pages.SoftAP.StartPage);

            if (startPage != null)
                CurrentSetupSettings.AppFrame.Navigate(startPage);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to signal the end of the setup process
        /// </summary>
        internal static void End()
        {
            NavigateAndClearBackStack();
            CurrentSetupSettings.SetupExit();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to clear setup pages from the back stack and optionally navigate to the completion page
        /// </summary>
        /// <param name="gotoCompletion">Goto the completion page</param>
        /// <param name="clearAuthPages">Remove Auth pages from the back stack</param>
        /// <param name="clearSoftAPPages">Remove SoftAP pages from the back stack</param>
        /// <param name="clearAllPages">Remove All pages from the back stack</param>
        private static void NavigateAndClearBackStack(bool gotoCompletion = true, bool clearAuthPages = true, bool clearSoftAPPages = true, bool clearAllPages = false)
        {
            var backStack = CurrentSetupSettings.AppFrame.BackStack;
            var authPageType = typeof(Pages.Auth.AuthPage);
            var softAPPageType = typeof(Pages.SoftAP.SoftAPPage);

            if (gotoCompletion)
            {
                var navigated = false;
                
                for (var i = backStack.Count; i > 0; --i)
                {
                    var pageStackEntry = backStack[i - 1];
                    var pageType = pageStackEntry.SourcePageType.GetTypeInfo().BaseType;

                    if (clearAllPages)
                    {
                        backStack.Remove(pageStackEntry);
                    }
                    else if (clearAuthPages && pageType == authPageType)
                    {
                        backStack.Remove(pageStackEntry);
                    }
                    else if (clearSoftAPPages && pageType == softAPPageType)
                    {
                        backStack.Remove(pageStackEntry);
                    }
                    else if (pageStackEntry.SourcePageType == CurrentSetupSettings.CompletionPageType)
                    {
                        navigated = true;
                        CurrentSetupSettings.AppFrame.GoBack();
                    }
                }

                if (!navigated)
                    CurrentSetupSettings.AppFrame.Navigate(CurrentSetupSettings.CompletionPageType);
            }

            backStack = CurrentSetupSettings.AppFrame.BackStack;
            for (var i = backStack.Count; i > 0; --i)
            {
                var pageStackEntry = backStack[i - 1];
                var pageType = pageStackEntry.SourcePageType.GetTypeInfo().BaseType;

                if (clearAllPages)
                {
                    backStack.Remove(pageStackEntry);
                }
                else if (clearAuthPages && pageType == authPageType)
                {
                    backStack.Remove(pageStackEntry);
                }
                else if (clearSoftAPPages && pageType == softAPPageType)
                {
                    backStack.Remove(pageStackEntry);
                }
            }
        }

        /// <summary>
        /// Remove a key from the local settings
        /// </summary>
        /// <param name="key"></param>
        private static void RemoveLocalSetting(string key)
        {
            if (localSettings.Values.ContainsKey(key))
                localSettings.Values.Remove(key);
        }

        #endregion
    }
}
