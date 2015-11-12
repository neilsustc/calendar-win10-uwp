using HelloWorld.Data;
using HelloWorld.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HelloWorld.CustomControls
{
    class EntryPanel : StackPanel
    {
        bool isFinished;

        bool toBeDeleted;
        public bool ToBeDelete
        {
            get { return toBeDeleted; }
        }

        bool modified;
        public bool Modified
        {
            get { return modified; }
        }

        public CheckBox ckbxFinished = new CheckBox();
        CalendarDatePicker datePicker = new CalendarDatePicker();
        TimePicker timePicker = new TimePicker();
        TextBox tbxDescription = new TextBox();
        ComboBox cbbxTags = new ComboBox();
        public Button btnDelete = new Button();

        public EntryPanel(EventEntry e = null)
        {
            // Layout
            this.Orientation = Orientation.Horizontal;
            this.Padding = new Thickness(12, 6, 12, 6);

            ckbxFinished.Margin = new Thickness(0, 0, 18, 0);
            datePicker.Margin = new Thickness(0, 0, 18, 0);
            timePicker.Margin = new Thickness(0, 0, 18, 0);
            tbxDescription.Margin = new Thickness(0, 0, 18, 0);
            cbbxTags.Margin = new Thickness(0, 0, 18, 0);

            tbxDescription.Padding = new Thickness(6, 4, 6, 4);
            btnDelete.Padding = new Thickness(8, 1, 8, 3);

            ckbxFinished.MinWidth = 32;
            timePicker.MinWidth = 80;
            timePicker.MinHeight = 32;
            tbxDescription.MinWidth = 300;
            cbbxTags.MinWidth = 120;

            ckbxFinished.Content = "done";
            datePicker.PlaceholderText = "Choose a date";
            ObservableCollection<string> tags = new ObservableCollection<string>();
            tags.Add("Deadline");
            tags.Add("Lecture");
            tags.Add("Exam/Test");
            tags.Add("Others");
            cbbxTags.ItemsSource = tags;
            btnDelete.Content = "Delete";

            if (e != null)
            {
                SetInfo(e);
            }

            //
            BasicEventHandler();

            //
            Children.Add(ckbxFinished);
            Children.Add(datePicker);
            Children.Add(timePicker);
            Children.Add(tbxDescription);
            Children.Add(cbbxTags);
            Children.Add(btnDelete);
        }

        private void BasicEventHandler()
        {
            ckbxFinished.Click += OnClick;
            btnDelete.Click += OnClick;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                isFinished = !isFinished;
            }
            else
            {
                toBeDeleted = !toBeDeleted;
                if (toBeDeleted)
                {
                    this.Background = new SolidColorBrush(Colors.LightGray);
                    SetAllEnabled(false);
                }
                else
                {
                    this.Background = new SolidColorBrush(Colors.White);
                    SetAllEnabled(true);
                }
            }
        }

        private void SetAllEnabled(bool en)
        {
            ckbxFinished.IsEnabled = en;
            datePicker.IsEnabled = en;
            timePicker.IsEnabled = en;
            tbxDescription.IsEnabled = en;
            cbbxTags.IsEnabled = en;
            if (en)
            {
                btnDelete.Content = "Delete";
            }
            else
            {
                btnDelete.Content = "Undo";
            }
        }

        public void HandleOnModified()
        {
            ckbxFinished.Click += OnModified;
            datePicker.DateChanged += OnModified;
            timePicker.TimeChanged += OnModified;
            tbxDescription.TextChanged += OnModified;
            cbbxTags.SelectionChanged += OnModified;
            btnDelete.Click += OnModified;
        }

        private void OnModified(object sender, TimePickerValueChangedEventArgs e)
        {
            CalendarPage.Current.OnModified();
        }

        private void OnModified(object sender, RoutedEventArgs e)
        {
            CalendarPage.Current.OnModified();
        }

        private void OnModified(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            CalendarPage.Current.OnModified();
        }

        public void SetInfo(EventEntry e)
        {
            datePicker.Date = e.GetDateTime();
            timePicker.Time = new TimeSpan(datePicker.Date.Value.Hour, datePicker.Date.Value.Minute, 0);

            if ("True".Equals(e.Content[2]))
            {
                isFinished = true;
            }
            else
            {
                isFinished = false;
            }
            ckbxFinished.IsChecked = isFinished;

            tbxDescription.Text = e.Content[3].ToString();

            cbbxTags.SelectedItem = e.Content[4];
        }

        public EventEntry GetInfo()
        {
            ArrayList list = new ArrayList();
            DateTimeOffset date = datePicker.Date.Value;
            list.Add(date.Year.ToString() + "/" + date.Month + "/" + date.Day);
            list.Add(timePicker.Time.Hours + ":" + timePicker.Time.Minutes);
            list.Add(isFinished);
            string des = tbxDescription.Text.Trim();
            list.Add(des.Substring(0, 1).ToUpper() + des.Substring(1));
            list.Add(cbbxTags.SelectedItem);
            return new EventEntry(list);
        }

        public bool ValidateInput()
        {
            if (datePicker.Date == null || "".Equals(tbxDescription.Text.Trim()))
            {
                return false;
            }
            return true;
        }
    }

    class EntryInfoPanel : StackPanel
    {
        Border tagBorder = new Border();
        TextBlock tagText = new TextBlock();
        TextBlock tbkTimeOffset = new TextBlock();
        TextBlock tbkDescription = new TextBlock();

        public EntryInfoPanel(EventEntry e, TimeSpan span)
        {
            tagBorder.Padding = new Thickness(6, 3, 6, 3);
            tagBorder.Margin = new Thickness(18, 3, 6, 3);
            tbkTimeOffset.Margin = new Thickness(12, 6, 0, 6);
            tbkDescription.Margin = new Thickness(18, 6, 0, 6);

            switch (e.GetTag())
            {
                case "Deadline":
                    tagBorder.Background = new SolidColorBrush(Colors.OrangeRed);
                    if (span.Days < 4)
                    {
                        tagBorder.Background = new SolidColorBrush(Colors.Red);
                    }
                    break;
                case "Exam/Test":
                    tagBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 201, 14));
                    break;
                default:
                    tagBorder.Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
                    break;
            }
            tagText.Foreground = new SolidColorBrush(Colors.White);

            tagText.Text = e.GetTag();
            DateTime date = e.GetDateTime();
            if (span.Days == 0)
            {
                tbkTimeOffset.Text = (span.Hours != 0 ? (span.Hours + " hours ") : "") + span.Minutes + " minutes left";
            }
            else
            {
                tbkTimeOffset.Text = span.Days + " days " + (span.Hours != 0 ? (span.Hours + " hours ") : "") + "left";
            }
            tbkDescription.Text = "[ " + e.GetDescription() + " ]";

            tagBorder.Child = tagText;

            Orientation = Orientation.Horizontal;
            //
            Children.Add(tagBorder);
            Children.Add(tbkTimeOffset);
            Children.Add(tbkDescription);
        }
    }
}
