using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace HelloWorld.Data
{
    class StdIO
    {
        public static async Task<string> ReadText(StorageFile file)
        {
            string fileContent = "";
            fileContent = await FileIO.ReadTextAsync(file);
            return fileContent;
        }

        public static async Task<bool> WriteText(StorageFile file, string text)
        {
            Configuration.eventsFile = await Configuration.library.CreateFileAsync(Configuration.eventsDataFilePath, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, text);
            return true;
        }
    }
}
