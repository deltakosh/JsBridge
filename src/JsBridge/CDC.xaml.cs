using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ChakraBridge;

namespace JSBridge
{
    public sealed partial class CDC
    {
        private ChakraHost host;
        ObservableCollection<People> peopleCollection;

        public CDC()
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

        private async void CDC_OnLoaded(object sender, RoutedEventArgs e)
        {
            WaitGrid.Visibility = Visibility.Visible;
            Console.OnLog += Console_OnLog;

            CommunicationManager.RegisterType(typeof(People[]));
            CommunicationManager.OnObjectReceived = (data) =>
            {
                var peopleList = (People[])data;
                peopleCollection = new ObservableCollection<People>(peopleList);

                peopleCollection.CollectionChanged += PeopleCollection_CollectionChanged;

                GridView.ItemsSource = peopleCollection;
                WaitGrid.Visibility = Visibility.Collapsed;
            };

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
                await ReadAndExecute("cdc.js");
                await ReadAndExecute("azuremobileservices.js");
                await ReadAndExecute("cdc-azuremobileservices.js");
                await ReadAndExecute("sample.js");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void PeopleCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (People people in e.OldItems)
                {
                    host.CallFunction("deleteFunction", people.Id);
                }
            }

            if (e.NewItems != null && e.NewItems.Count > 0)
            {

            }
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
            WaitGrid.Visibility = Visibility.Visible;
            host.CallFunction("commitFunction");
        }

        private void RollbackButton_OnClick(object sender, RoutedEventArgs e)
        {
            WaitGrid.Visibility = Visibility.Visible;
            host.CallFunction("rollbackFunction");
        }
    }
}
