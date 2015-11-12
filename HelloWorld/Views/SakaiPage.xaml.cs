using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HelloWorld.Views
{
    public sealed partial class SakaiPage : Page
    {
        public SakaiPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage.Current.ShowStatusInfo("Loading...", NotifyType.Normal);
            
            MainPage.Current.ResetStatusBar();

            this.progressBar.Visibility = Visibility.Collapsed;
            this.placeHolder.Margin = new Thickness(0, 4, 0, 0);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.progressBar.Visibility = Visibility.Visible;
            this.placeHolder.Margin = new Thickness(0);
        }
    }
}
