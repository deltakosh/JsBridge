using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ChakraBridge;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using Windows.UI;

namespace JSBridge
{
    public sealed partial class MainPage
    {
        private ChakraHost host;
        private DateTime start;
        private int drawCount;
        private bool initialized = false;

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
                // simple palette
                //await ReadAndExecute("sample.js");

                // animating rect
                //await ReadAndExecute("paper-full.js");
                //await ReadAndExecute("papersample.js");

                // tadpoles
                await ReadAndExecute("paper-full.js");
                await ReadAndExecute("tadpoles.js");

                this.initialized = true;
                this.canvasCtrl.Invalidate();
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
            if (!this.initialized) {
                return;
            }
            this.host.CallFunction("drawScene");
            var target = (CanvasRenderTarget)this.host.Window.Render();

            args.DrawingSession.DrawImage(target);


            if (drawCount == 0) {
                this.start = DateTime.Now;
            }
            drawCount++;
            var seconds = (DateTime.Now - this.start).TotalSeconds;
            if (seconds > 0) {
                var fps = drawCount / seconds;

                args.DrawingSession.DrawText(fps.ToString("0.#") + "fps", 10, 10, Colors.Red);
            }

            // triggers next Draw event at max 60fps
            this.canvasCtrl.Invalidate();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.canvasCtrl.RemoveFromVisualTree();
            this.canvasCtrl = null;
        }
    }
}
