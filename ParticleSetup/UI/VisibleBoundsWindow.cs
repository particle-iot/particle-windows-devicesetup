using System.ComponentModel;
using Windows.UI.Xaml;

namespace Particle.UI
{
    public class VisibleBoundsWindow : INotifyPropertyChanged
    {
        #region Private Members

        private GridLength statusBarHeight;
        private GridLength navigationBarHeight;
        private static VisibleBoundsWindow resizeWindow = null;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Static Properties

        public static VisibleBoundsWindow VisibleBounds
        {
            get
            {
                if (resizeWindow == null)
                    resizeWindow = new VisibleBoundsWindow();

                return resizeWindow;
            }
        }

        #endregion

        #region Properties

        public GridLength StatusBarHeight
        {
            get { return statusBarHeight; }
            set
            {
                if (statusBarHeight != value)
                {
                    statusBarHeight = value;
                    OnPropertyChanged("StatusBarHeight");
                }
            }
        }

        public GridLength NavigationBarHeight
        {
            get { return navigationBarHeight; }
            set
            {
                if (statusBarHeight != value)
                {
                    navigationBarHeight = value;
                    OnPropertyChanged("NavigationBarHeight");
                }
            }
        }

        #endregion

        #region Static Methods

        public static void SetBounds(Windows.Foundation.Rect windowBounds, Windows.Foundation.Rect visibleBounds)
        {
            resizeWindow.StatusBarHeight = new GridLength(visibleBounds.Top);

            var navigationBarHeight = windowBounds.Height - visibleBounds.Height - visibleBounds.Top;
            if (navigationBarHeight < 0)
                navigationBarHeight = 0;

            resizeWindow.NavigationBarHeight = new GridLength(navigationBarHeight);
        }

        #endregion

        #region Private Methods

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
