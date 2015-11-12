using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using HelloWorld.Views;
using HelloWorld.Data;
using Windows.Storage;

namespace HelloWorld
{
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;

            listBox.SelectedIndex = 0;
        }
        
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBox)sender).SelectedIndex)
            {
                case 0:
                    contentPageFrame.Navigate(typeof(HomePage));
                    break;
                case 1:
                    contentPageFrame.Navigate(typeof(CalendarPage));
                    break;
                case 2:
                    contentPageFrame.Navigate(typeof(SakaiPage));
                    break;
                case 3:
                    contentPageFrame.Navigate(typeof(AboutPage));
                    break;
                default:
                    break;
            }
            ResetStatusBar();
        }

        public void ShowStatusInfo(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.Normal:
                    statusBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
                    break;
                case NotifyType.Success:
                    statusBorder.Background = new SolidColorBrush(Colors.Green);
                    break;
                case NotifyType.Warning:
                    statusBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 140, 64));
                    break;
                case NotifyType.Error:
                    statusBorder.Background = new SolidColorBrush(Colors.Red);
                    break;
            }
            statusBlock.Text = strMessage;
        }

        public void ResetStatusBar()
        {
            ShowStatusInfo("Ready", NotifyType.Normal);
        }
    }

    public enum NotifyType
    {
        Normal,
        Success,
        Warning,
        Error
    };
}
