using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace JSBridge
{
    public static class CoreTools
    {
        public async static Task<StorageFile> GetPackagedFileAsync(string folderName, string fileName)
        {
            StorageFolder installFolder = Package.Current.InstalledLocation;

            if (folderName != null)
            {
                StorageFolder subFolder = await installFolder.GetFolderAsync(folderName);
                return await subFolder.GetFileAsync(fileName);
            }

            return await installFolder.GetFileAsync(fileName);
        }

        public async static Task<string> DownloadStringAsync(string url)
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
        }
    }
}
