using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ChakraBridge;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;

namespace JSBridge
{
    public sealed partial class MainPage
    {
        private ChakraHost host;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Log(string text)
        {
            JsConsole.Text += text + "\n";
            JsOutputScroll.ChangeView(null, double.MaxValue, null);
        }

        async Task ReadAndExecute(string filename)
        {
            var script = await CoreTools.GetPackagedFileContentAsync("refs", filename);
            host.RunScript(script);
        }

        async Task DownloadAndExecute(string url)
        {
            var script = await CoreTools.DownloadStringAsync(url);

            try
            {
                host.RunScript(script);
            }
            catch (Exception ex)
            {
                JsConsole.Text = ex.Message;
            }
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Console.OnLog += Console_OnLog;

            try
            {
                host = new ChakraHost();
            }
            catch (Exception ex)
            {
                JsConsole.Text = ex.Message;
            }

            try
            {
                await ReadAndExecute("sample.js");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private async void Console_OnLog(object sender, string text)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Log(text);
            });
        }

        private void canvasCtrl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var target = (CanvasRenderTarget)this.host.Window.Render();

            args.DrawingSession.DrawImage(target);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.canvasCtrl.RemoveFromVisualTree();
            this.canvasCtrl = null;
        }
    }
}
