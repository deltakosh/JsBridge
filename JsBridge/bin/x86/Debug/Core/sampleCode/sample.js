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
        console.log("CDCService is good to go! : " + results.table[0].lastname);
    }
}, dataContext, onUpdateDataContext, 3);

//setTimeout(function() {
//    console.log("GG!");
//}, 2000);

//var print = function () {
//    console.log(this.name);
//}

//var bind = function (func, target) {
//    return function () {
//        func.apply(target, arguments);
//    };
//}

//var foo = {
//    name: "toto",
//    execute: function () {
//        setTimeout(bind(print, this), 2000);
//    }
//}

//foo.execute();
