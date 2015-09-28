using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Chakra;
using Entities;

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

        async Task ReadAndExecute(string filename)
        {
            var script = await CoreTools.GetPackagedFileContentAsync("refs", filename);
            host.RunScript(script);
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            WaitGrid.Visibility = Visibility.Visible;
            Console.OnLog += Console_OnLog;

            CommunicationManager.RegisterType(typeof(People[]));
            CommunicationManager.OnObjectReceived = (type, data) =>
            {
                switch (type)
                {
                    case "People[]":
                        var peopleList = (People[]) data;
                        peopleCollection = new ObservableCollection<People>(peopleList);

                        peopleCollection.CollectionChanged += PeopleCollection_CollectionChanged;

                        GridView.ItemsSource = peopleCollection;
                        break;
                }
                WaitGrid.Visibility = Visibility.Collapsed;
            };


            string msg = host.Init();
            if (msg != "NoError")
            {
                JsConsole.Text = msg;
            }

            //host.ProjectObjectToGlobal(DataManager.Current, "dataManager");     
            //host.ProjectNamespace("Entities");
            //DataManager.Current.OnPeopleReceived += PeopleManager_OnPeopleReceived;

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

        //private void PeopleManager_OnPeopleReceived(object sender, IEnumerable<People> peopleArray)
        //{
        //    peopleCollection = new ObservableCollection<People>(peopleArray);

        //    peopleCollection.CollectionChanged += PeopleCollection_CollectionChanged;

        //    GridView.ItemsSource = peopleCollection;
        //}

        private void PeopleCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (People people in e.OldItems)
                {
                    host.CallFunction("deleteFunction", people.Id);
                    //DataManager.Current.Delete(people);
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
            DataManager.Current.Commit();
        }

        private void RollbackButton_OnClick(object sender, RoutedEventArgs e)
        {
            WaitGrid.Visibility = Visibility.Visible;
            host.CallFunction("rollbackFunction");
            //DataManager.Current.Rollback();
        }
    }
}
