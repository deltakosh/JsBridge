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
    public sealed partial class PaperJS
    {
        private ChakraHost host;
        private DateTime start;
        private int drawCount;
        private bool initialized = false;

        public PaperJS()
        {
            InitializeComponent();
        }

        private void Log(string text)
        {
            JsConsole.Text += text + "\n";
            JsOutputScroll.ChangeView(null, double.MaxValue, null);
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
                //await host.ReadAndExecute("sample.js", "paperjs-refs");

                // animating rect
                //await host.ReadAndExecute("paper-core.js", "paperjs-refs");
                //await host.ReadAndExecute("papersample.js", "paperjs-refs");

                // tadpoles
                await host.ReadAndExecute("paper-core.js", "paperjs-refs");
                await host.ReadAndExecute("tadpoles.js", "paperjs-refs");

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
