using HelloWorld.CustomControls;
using HelloWorld.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HelloWorld.Views
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage.Current.ShowStatusInfo("Loading...", NotifyType.Normal);

            DateTime now = DateTime.Now;
            await TempData.LoadEvents();

            foreach (var item in TempData.Events)
            {
                if (Configuration.hideWhatIsDone || item.IsFinished())
                    continue;

                TimeSpan span = item.GetDateTime().Subtract(now);
                if (span.Days < 4)
                {
                    importantEvents.Children.Add(new EntryInfoPanel(item, span));
                }
                else if (span.Days < 15)
                {
                    moreEvents.Children.Add(new EntryInfoPanel(item, span));
                }
                else
                {
                    break;
                }
            }

            if (importantEvents.Children.Count == 0)
            {
                importantEvents.Children.Add(new TextBlock() { Text = "Nothing", Margin = new Thickness(18, 6, 0, 6) });
            }
            if (moreEvents.Children.Count == 0)
            {
                moreEvents.Children.Add(new TextBlock() { Text = "Nothing", Margin = new Thickness(18, 6, 0, 6) });
            }

            this.progressBar.Visibility = Visibility.Collapsed;
            this.placeHolder.Margin = new Thickness(0, 4, 0, 0);

            MainPage.Current.ResetStatusBar();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.progressBar.Visibility = Visibility.Visible;
            this.placeHolder.Margin = new Thickness(0);
        }
    }
}
