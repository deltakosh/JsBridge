console.log("Go....!");

var CDCAzureMobileService = new CloudDataConnector.AzureDataService();

var CDCService = new CloudDataConnector.DataService(new CloudDataConnector.OfflineService(), new CloudDataConnector.ConnectivityService());
CDCAzureMobileService.addSource('https://angularpeoplev2.azure-mobile.net/', 'DDJpBYxoQEUznagCnyYNRYfkDxpYyz90', ['people']);

CDCService.addSource(CDCAzureMobileService);

var dataContext = {};

var onUpdateDataContext = function (data) {
}

CDCService.connect(function (results) {
    if (results === false) {
        console.log("CDCService must first be successfully initialized");
    } else {
        console.log("CDCService is good to go!");

        var models = [];
        for (var index = 0; index < results.table.length; index++) {
            var people = new Models.People();
            people.firstname = results.table[0].firstname;
            people.lastname = results.table[0].lastname;
            models.push(people);
        }

        peopleManager.raiseOnPeopleReceived(models);
    }
}, dataContext, onUpdateDataContext, 3);
