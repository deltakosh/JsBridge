using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JSBridge
{
    public sealed partial class MainPage
    {
        readonly ChakraHost.ChakraHost host = new ChakraHost.ChakraHost();

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string result = host.RunScript(JsInput.Text);
            Log(result);
        }

        private void Log(string text)
        {
            JsConsole.Text += text + "\n";
            JsOutputScroll.ChangeView(null, double.MaxValue, null);
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            string msg = await host.InitAsync();
            if (msg != "NoError")
            {
                JsConsole.Text = msg;
            }

            JSE.console.OnLog += Console_OnLog;

            var codeFile = await CoreTools.GetPackagedFileAsync("sampleCode", "sample.js");
            JsInput.Text = await FileIO.ReadTextAsync(codeFile);
        }

        private void Console_OnLog(object sender, string text)
        {
            Log(text);
        }
    }
}
