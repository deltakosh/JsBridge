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
    sendToHost(JSON.stringify(data), "People[]");
}

CDCService.connect(function (results) {
    if (results === false) {
        console.log("CDCService must first be successfully initialized");
    } else {
        console.log("CDCService is good to go!");
    }
}, dataContext, onUpdateDataContext, 3);

commitFunction = function () {
    CDCService.commit(function () {
        console.log('Commit successful');
    }, function (e) {
        console.log('Error during commit');
    });
}

rollbackFunction = function () {
    CDCService.rollback(function () {
        console.log('Rollback successful');
    }, function (e) {
        console.log('Error during rollback');
    });
}

deleteFunction = function (id) {
    console.log('Deleting ' + id);
    for (var index = 0; index < dataContext.people.length; index++) {
        var people = dataContext.people[index];

        if (people.id === id) {
            CDCService.remove("people", people);
            break;
        }
    }
}
