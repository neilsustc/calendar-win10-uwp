using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace HelloWorld
{
    class Configuration
    {
        public static string eventsDataFilePath = "eventsData.txt";
        public static StorageFolder library = KnownFolders.PicturesLibrary;
        public static StorageFile eventsFile = null;
        public static string mruToken = null;
        public static string falToken = null;

        public static bool hideWhatIsDone = false;
        
        internal static async Task<bool> ConfigDataFile()
        {
            try
            {
                eventsFile = await library.GetFileAsync(Configuration.eventsDataFilePath);
            }
            catch (FileNotFoundException)
            {
                await CreateDataFile();
            }
            return true;
        }

        private static async Task CreateDataFile()
        {
            eventsFile = await library.CreateFileAsync(Configuration.eventsDataFilePath, CreationCollisionOption.ReplaceExisting);
        }
    }
}
