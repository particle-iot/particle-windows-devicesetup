using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Particle.UI.Controls
{
    public class AlternatingRowListView : ListView
    {
        // based on http://www.bendewey.com/index.php/523/alternating-row-color-in-windows-store-listview

        #region Properties

        public Brush EvenRowBackground
        {
            get { return (Brush)GetValue(EvenRowBackgroundProperty); }
            set { SetValue(EvenRowBackgroundProperty, (Brush)value); }
        }

        public Brush OddRowBackground
        {
            get { return (Brush)GetValue(OddRowBackgroundProperty); }
            set { SetValue(OddRowBackgroundProperty, (Brush)value); }
        }

        public static readonly DependencyProperty EvenRowBackgroundProperty = 
            DependencyProperty.Register("EvenRowBackground", typeof(Brush), typeof(AlternatingRowListView), null);

        public static readonly DependencyProperty OddRowBackgroundProperty =
            DependencyProperty.Register("OddRowBackground", typeof(Brush), typeof(AlternatingRowListView), null);

        #endregion

        #region Overrides

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);

            foreach (var item in Items)
                SetlistViewItemBackground((ListViewItem)ContainerFromItem(item));
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            SetlistViewItemBackground((ListViewItem)element);
        }

        #endregion

        #region Private Methods

        private void SetlistViewItemBackground(ListViewItem listViewItem)
        {
            if (listViewItem == null)
                return;

            var index = IndexFromContainer(listViewItem);

            if ((index + 1) % 2 == 1)
                listViewItem.Background = OddRowBackground;
            else
                listViewItem.Background = EvenRowBackground;
        }

        #endregion
    }
}
