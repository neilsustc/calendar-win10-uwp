using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelloWorld.Data
{
    class EventEntry : IComparable<EventEntry>
    {
        /// <summary>
        /// [date][time][state][description][tag]
        /// </summary>
        public ArrayList Content = new ArrayList();

        public EventEntry() { }

        public EventEntry(string s)
        {
            SetValue(Unwrap(s));
        }

        public EventEntry(ArrayList content)
        {
            this.Content = content;
        }

        public void SetValue(string[] ss)
        {
            Content.Clear();
            foreach (var item in ss)
            {
                Content.Add(item);
            }
        }

        public string Wrap()
        {
            string entry = "";
            foreach (var item in Content)
            {
                entry += "[" + item + "]";
            }
            return entry;
        }

        private string[] Unwrap(string entry)
        {
            entry = entry.Substring(1, entry.Length - 2);
            string[] strings = entry.Split(new string[] { "][" }, StringSplitOptions.None);
            return strings;
        }

        int IComparable<EventEntry>.CompareTo(EventEntry other)
        {
            if (other == null)
                return 1;
            else
                return GetDateTime().CompareTo(other.GetDateTime());
        }

        internal bool IsFinished()
        {
            return "True".Equals(Content[2]);
        }

        internal DateTime GetDateTime()
        {
            string[] dates = ((string)Content[0]).Split(new char[] { '/' });
            string[] hours = ((string)Content[1]).Split(new char[] { ':' });
            DateTime time = new DateTime(Int32.Parse(dates[0]), Int32.Parse(dates[1]), Int32.Parse(dates[2]), Int32.Parse(hours[0]), Int32.Parse(hours[1]),0);
            return time;
        }

        internal string GetTag()
        {
            return Content[4].ToString();
        }

        internal string GetDescription()
        {
            return Content[3].ToString();
        }
    }
}
