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

        console.log("Getting list of people");
        var peopleList = [];
        for (var index = 0; index < results.table.length; index++) {
            var people = new Models.People();
            people.firstName = results.table[index].firstname;
            people.lastName = results.table[index].lastname;
            peopleList.push(people);
        }

        dataManager.raiseOnPeopleReceived(peopleList);
    }
}, dataContext, onUpdateDataContext, 3);

dataManager.commitFunction = function () {
    CDCService.commit(function () {
        console.log('Commit successful');
    }, function (e) {
        console.log('Error during commit');
    });
}
