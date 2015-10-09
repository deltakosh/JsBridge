using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Storage;

namespace ChakraBridge
{
    public static class CoreTools
    {
        public static IAsyncOperation<StorageFile> GetPackagedFileAsync(string folderName, string fileName)
        {
            return Task.Run(async () =>
            {
                StorageFolder installFolder = Package.Current.InstalledLocation;

                if (folderName != null)
                {
                    StorageFolder subFolder = await installFolder.GetFolderAsync(folderName);
                    return await subFolder.GetFileAsync(fileName);
                }

                return await installFolder.GetFileAsync(fileName);
            }).AsAsyncOperation();
        }

        public static IAsyncOperation<string> GetPackagedFileContentAsync(string folderName, string fileName)
        {
            return Task.Run(async () =>
            {
                var file = await GetPackagedFileAsync(folderName, fileName);
                return await FileIO.ReadTextAsync(file);
            }).AsAsyncOperation();
        }

        public static IAsyncOperation<string> DownloadStringAsync(string url)
        {
            return Task.Run(async () =>
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(new Uri(url)))
                    {
                        using (var content = response.Content)
                        {
                            return await content.ReadAsStringAsync();
                        }
                    }
                }
            }).AsAsyncOperation();
        }
    }
}
