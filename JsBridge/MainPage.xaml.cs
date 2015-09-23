using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Models;
using ViewModels;

namespace JSBridge
{
    public sealed partial class MainPage
    {
        readonly ChakraHost host = new ChakraHost();
        ObservableCollection<People> peopleCollection; 

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
            DataManager.Current.OnPeopleReceived += PeopleManager_OnPeopleReceived;

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

        private void PeopleManager_OnPeopleReceived(object sender, Models.People[] peopleArray)
        {
            peopleCollection = new ObservableCollection<People>(peopleArray);
            GridView.ItemsSource = peopleCollection;
        }

        private async void Console_OnLog(object sender, string text)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Log(text);                 
            });
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var people = button.DataContext as People;

                peopleCollection.Remove(people);
            }
        }

        private void CommitButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataManager.Current.Commit();
        }
    }
}
