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
    public sealed partial class PaperJS
    {
        private ChakraHost host;
        private DispatcherTimer timer;

        public PaperJS()
        {
            InitializeComponent();

            this.timer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(1000d / 60)
            };
            this.timer.Tick += (o, e) => {
                host.CallFunction("drawScene");
                this.canvasCtrl.Invalidate();
            };
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
                //await ReadAndExecute("sample.js");

                // animating rect
                //await ReadAndExecute("paper-full.js");
                //await ReadAndExecute("papersample.js");

                // tadpoles
                await host.ReadAndExecute("paper-full.js", "paperjs-refs");
                await host.ReadAndExecute("tadpoles.js", "paperjs-refs");

                this.timer.Start();
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
