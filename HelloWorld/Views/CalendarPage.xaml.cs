using HelloWorld.CustomControls;
using HelloWorld.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HelloWorld.Views
{
    public sealed partial class CalendarPage : Page
    {
        public static CalendarPage Current;

        public CalendarPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage.Current.ShowStatusInfo("Loading...", NotifyType.Normal);

            await TempData.LoadEvents();
            foreach (var item in TempData.Events)
            {
                if (Configuration.hideWhatIsDone && item.IsFinished())
                    continue;
                EntryPanel panel = new EntryPanel(item);
                panel.HandleOnModified();
                this.eventList.Children.Add(panel);
            }

            this.progressBar.Visibility = Visibility.Collapsed;
            calendarGrid.Margin = new Thickness(24, 4, 24, 24);

            MainPage.Current.ResetStatusBar();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.eventList.Children.Clear();

            this.progressBar.Visibility = Visibility.Visible;
            calendarGrid.Margin = new Thickness(24, 0, 24, 24);
        }
        
        public void OnModified()
        {
            MainPage.Current.ShowStatusInfo("* Not saved", NotifyType.Warning);

            btnCancel.IsEnabled = true;
            btnSave.IsEnabled = true;
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Equals((Button)sender))
            {
                if (toBeAddedPanel.Children.Count > 3)
                    return;
                EntryPanel entry = new EntryPanel();
                entry.ckbxFinished.IsEnabled = false;
                entry.btnDelete.IsEnabled = false;
                toBeAddedPanel.Children.Add(entry);

                btnCancel.IsEnabled = true;
                btnSave.IsEnabled = true;
            }
            else
            {
                bool isSave = btnSave.Equals((Button)sender);
                if (isSave)
                {
                    // Validate
                    foreach (var item in eventList.Children.Union(toBeAddedPanel.Children))
                    {
                        if (!((EntryPanel)item).ValidateInput())
                        {
                            MainPage.Current.ShowStatusInfo("Required fields", NotifyType.Error);
                            return;
                        }
                    }

                    // Write into file
                    TempData.Events.Clear();
                    foreach (var item in eventList.Children.Union(toBeAddedPanel.Children))
                    {
                        if (((EntryPanel)item).ToBeDelete)
                            continue;
                        TempData.Events.Add(((EntryPanel)item).GetInfo());
                    }
                    await TempData.SaveEvents();
                    MainPage.Current.ShowStatusInfo("Saved", NotifyType.Success);
                }
                toBeAddedPanel.Children.Clear();
                Page_Unloaded(null, null);
                /* didn't use Page_loaded method because of wanting to show "Saved" message. */
                /* */
                await TempData.LoadEvents(); // Not nessesary if(!isSave), it was placed here to ensure animation work well while cancelling.
                foreach (var item in TempData.Events)
                {
                    if (Configuration.hideWhatIsDone && item.IsFinished())
                        continue;
                    EntryPanel panel = new EntryPanel(item);
                    panel.HandleOnModified();
                    this.eventList.Children.Add(panel);
                }
                /* */

                this.progressBar.Visibility = Visibility.Collapsed;
                calendarGrid.Margin = new Thickness(24, 4, 24, 24);

                if (!isSave)
                {
                    MainPage.Current.ShowStatusInfo("Cancelled", NotifyType.Normal);
                }
            }
        }
    }
}
