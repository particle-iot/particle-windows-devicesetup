using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Particle.Setup
{
    /// <summary>
    /// Delegete define for callbacks in setup
    /// </summary>
    public delegate void SetupEventHandler();

    /// <summary>
    /// A class for setting options for the setup process
    /// </summary>
    public class SetupConfig
    {
        #region Events

        /// <summary>
        /// Global event called by setup when the process exits
        /// </summary>
        public event SetupEventHandler OnSetupExit;

        /// <summary>
        /// Global event called by setup when the user logs in
        /// </summary>
        public event SetupEventHandler OnSetupLogin;

        /// <summary>
        /// Global event called by setup when the user logs out
        /// </summary>
        public event SetupEventHandler OnSetupLogout;

        #endregion

        #region Public Members

        /// <summary>
        /// The main application frame
        /// </summary>
        public Frame AppFrame { get; set; }

        /// <summary>
        /// The page to show after completion
        /// </summary>
        public Type CompletionPageType { get; set; }

        /// <summary>
        /// A HashSet of current device names to check against when assigning a name
        /// </summary>
        public HashSet<string> CurrentDeviceNames { get; set; }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Internal function to signal setup exiting
        /// </summary>
        internal void SetupExit()
        {
            OnSetupExit?.Invoke();
        }

        /// <summary>
        /// Internal function to signal setup exiting
        /// </summary>
        internal void SetupLogin()
        {
            OnSetupLogin?.Invoke();
        }

        /// <summary>
        /// Internal function to signal setup exiting
        /// </summary>
        internal void SetupLogout()
        {
            OnSetupLogout?.Invoke();
        }

        #endregion
    }
}
