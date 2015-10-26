using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraBridge;

namespace JSBridge
{
    public static class LoaderTools
    {

        public static async Task ReadAndExecute(this ChakraHost host, string filename, string folder)
        {
            var script = await CoreTools.GetPackagedFileContentAsync(folder, filename);
            host.RunScript(script);
        }

        public static async Task DownloadAndExecute(this ChakraHost host, string url)
        {
            var script = await CoreTools.DownloadStringAsync(url);

            host.RunScript(script);
        }
    }
}
