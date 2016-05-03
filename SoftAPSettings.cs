using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Particle.Setup
{
    /// <summary>
    /// A class for setting options for during the soft AP process
    /// </summary>
    public class SoftAPSettings
    {
        /// <summary>
        /// The main application frame
        /// </summary>
        public Frame AppFrame { get; set; }

        /// <summary>
        /// The page to show after compleation
        /// </summary>
        public Type CompletionPageType { get; set; }

        /// <summary>
        /// The username of the user if you want it shown
        /// </summary>
        public HashSet<string> CurrentDeviceNames { get; set; }

        /// <summary>
        /// A HashSet of current device names to check against when assigning a name
        /// </summary>
        public string Username { get; set; }
    }
}
