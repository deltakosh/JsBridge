console.log("Go....!");

var CDCAzureMobileService = new CloudDataConnector.AzureDataService();

var CDCService = new CloudDataConnector.DataService(new CloudDataConnector.OfflineService(), new CloudDataConnector.ConnectivityService());
CDCAzureMobileService.addSource('https://angularpeoplev2.azure-mobile.net/', 'DDJpBYxoQEUznagCnyYNRYfkDxpYyz90', ['people']);

CDCService.addSource(CDCAzureMobileService);

var dataContext = {};

var onUpdateDataContext = function (data) {
    if (data && data.length) {
        syncPeople(data);
    }
}

var syncPeople = function (data) {
    var peopleList = [];
    for (var index = 0; index < data.length; index++) {
        var people = new Entities.People();
        people.firstName = data[index].firstname;
        people.lastName = data[index].lastname;
        people.id = index;
        peopleList.push(people);
    }

    dataManager.raiseOnPeopleReceived(peopleList);
}

CDCService.connect(function (results) {
    if (results === false) {
        console.log("CDCService must first be successfully initialized");
    } else {
        console.log("CDCService is good to go!");

        syncPeople(results.table);
    }
}, dataContext, onUpdateDataContext, 3);

dataManager.commitFunction = function () {
    CDCService.commit(function () {
        console.log('Commit successful');
    }, function (e) {
        console.log('Error during commit');
    });
}

dataManager.rollbackFunction = function () {
    CDCService.rollback(function () {
        console.log('Rollback successful');
    }, function (e) {
        console.log('Error during rollback');
    });
}

dataManager.deleteFunction = function (people) {
    CDCService.remove("people", dataContext.people[people.index]);
}
