using Particle.Setup.Models;
using Particle.Setup.Pages;

namespace Particle.Setup
{
    public class SoftAP
    {
        #region Private Static Members

        private static SoftAPResult softAPResult = null;

        #endregion

        #region Internal Static Properties

        internal static SoftAPSettings CurrentSoftAPSettings { get; set; }

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

        #endregion

        #region Public Static Methods

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
        public static void Start(SoftAPSettings softAPSettings)
        {
            ResetSoftAPResult();
            SoftAPConfig.ResetSoftAPData();

            CurrentSoftAPSettings = softAPSettings;
            CurrentSoftAPSettings.AppFrame.Navigate(typeof(SoftAPStartPage));
        }

        /// <summary>
        /// Function to call when back button is pressed (on page or hardware/soft button) to go back and/or exit
        /// </summary>
        public static bool BackButtonPressed()
        {
            if (SoftAP.SoftAPResult.Result != SoftAPSetupResult.Started)
            {
                CurrentSoftAPSettings.SoftAPExit();
                if (SoftAP.SoftAPResult.Result != SoftAPSetupResult.NotStarted)
                    return false;
            }

            if (CurrentSoftAPSettings.AppFrame.CanGoBack)
            {
                CurrentSoftAPSettings.AppFrame.GoBack();
                return true;
            }

            return false;
        }

        #endregion
    }
}
