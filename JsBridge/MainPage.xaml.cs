using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JSBridge
{
    public sealed partial class MainPage
    {
        readonly ChakraHost host = new ChakraHost();

        public MainPage()
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
            JSE.console.OnLog += Console_OnLog;
            ViewModels.peopleManager.OnPeopleReceived += PeopleManager_OnPeopleReceived;

            string msg = await host.InitAsync();
            if (msg != "NoError")
            {
                JsConsole.Text = msg;
            }

            try
            {
                await host.AddScriptReferenceAsync("sample.js");

            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void PeopleManager_OnPeopleReceived(object sender, Models.People[] people)
        {
            GridView.ItemsSource = people;
        }

        private void Console_OnLog(object sender, string text)
        {
            Log(text);
        }
    }
}
