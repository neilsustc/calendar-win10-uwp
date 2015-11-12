using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace HelloWorld.Data
{
    class TempData
    {
        static List<EventEntry> events = new List<EventEntry>();
        public static List<EventEntry> Events
        {
            get { return events; }
        }

        public static async Task<bool> LoadEvents()
        {
            string text = await StdIO.ReadText(Configuration.eventsFile);
            Regex regex = new Regex(Environment.NewLine);
            string[] strings = regex.Split(text);
            lock (events)
            {
                events.Clear();
                foreach (var item in strings)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        events.Add(new EventEntry(item));
                    }
                }
            }
            events.Sort();
            return true;
        }

        public static async Task<bool> SaveEvents()
        {
            string dataText = "";
            foreach (var item in events)
            {
                dataText += item.Wrap() + Environment.NewLine;
            }
            await StdIO.WriteText(Configuration.eventsFile, dataText);
            return true;
        }
    }
}
