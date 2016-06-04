using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Particle.Setup.Pages
{
    public partial class SetupPage : Page
    {
        internal void SetCustomization(Grid rootGrid)
        {
            List<TextBlock> textBlocks = FindTypeInContainer<TextBlock>(rootGrid);
            List<RichTextBlock> richTextBlocks = FindTypeInContainer<RichTextBlock>(rootGrid);

            foreach (TextBlock textblock in textBlocks)
            {
                textblock.Text = textblock.Text.Replace("{brand}", ParticleSetup.CurrentSetupSettings.BrandName);
                textblock.Text = textblock.Text.Replace("{device}", ParticleSetup.CurrentSetupSettings.DeviceName);
                textblock.Text = textblock.Text.Replace("{color}", ParticleSetup.CurrentSetupSettings.ListenModeLEDColorName);
                textblock.Text = textblock.Text.Replace("{mode button}", ParticleSetup.CurrentSetupSettings.ModeButtonText);
            }

            if (ParticleSetup.CurrentSetupSettings.TextForegroundColor != null)
            {
                foreach (TextBlock control in textBlocks)
                {
                    if (control.Text != "!")
                        control.Foreground = ParticleSetup.CurrentSetupSettings.TextForegroundColor;
                }
                foreach (RichTextBlock control in richTextBlocks)
                    control.Foreground = ParticleSetup.CurrentSetupSettings.TextForegroundColor;
            }

            if (ParticleSetup.CurrentSetupSettings.LinkForegroundColor != null)
            {
                List<HyperlinkButton> controls = FindTypeInContainer<HyperlinkButton>(rootGrid);
                foreach (HyperlinkButton control in controls)
                    control.Foreground = ParticleSetup.CurrentSetupSettings.LinkForegroundColor;

                foreach (RichTextBlock control in richTextBlocks)
                {
                    foreach (var block in control.Blocks)
                    {
                        if (block is Paragraph)
                        {
                            var paragraph = (Paragraph)block;
                            foreach (Inline inline in paragraph.Inlines)
                            {
                                var hyperlink = inline as Hyperlink;
                                if (hyperlink != null)
                                    hyperlink.Foreground = ParticleSetup.CurrentSetupSettings.LinkForegroundColor;
                            }
                        }
                    }
                }
            }

            if (ParticleSetup.CurrentSetupSettings.ElementForegroundColor != null || ParticleSetup.CurrentSetupSettings.ElementBackgroundColor != null)
            {
                List<Button> controls = FindTypeInContainer<Button>(rootGrid);
                foreach (Button control in controls)
                {
                    if (ParticleSetup.CurrentSetupSettings?.ElementForegroundColor != null)
                        control.Foreground = ParticleSetup.CurrentSetupSettings.ElementForegroundColor;

                    if (ParticleSetup.CurrentSetupSettings?.ElementBackgroundColor != null)
                        control.BorderBrush = ParticleSetup.CurrentSetupSettings.ElementBackgroundColor;
                }
            }

            if (ParticleSetup.CurrentSetupSettings.MaskSetupImages != null)
            {
                List<BitmapIcon> controls = FindTypeInContainer<BitmapIcon>(rootGrid);
                foreach (var control in controls)
                    control.Foreground = ParticleSetup.CurrentSetupSettings.MaskSetupImages;
            }
        }

        protected static List<T> FindTypeInContainer<T>(DependencyObject container)
        {
            List<T> foundChildren = new List<T>();

            if (container is ContentControl)
            {
                var contentConrol = (ContentControl)container;
                var newContainer = contentConrol.Content as DependencyObject;
                if (newContainer != null)
                    container = newContainer;
            }

            int count = VisualTreeHelper.GetChildrenCount(container);
            for (int i = 0; i < count; i++)
            {
                var child = (FrameworkElement)VisualTreeHelper.GetChild(container, i);

                if (child == null)
                    continue;

                if (child is T)
                    foundChildren.Add((T)(object)child);

                var subFoundChildren = FindTypeInContainer<T>(child);
                foundChildren.AddRange(subFoundChildren);
            }

            return foundChildren;

        }
    }
}