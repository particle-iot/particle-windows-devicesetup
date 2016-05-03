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

        #endregion

        #region Public Methods

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

        #endregion
    }
}
