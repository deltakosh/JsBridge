/* Copyright (c) Microsoft Open Technologies, Inc.  All rights reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information. */
/// <reference path="../../../lib/jquery/jquery.d.ts" />
/// <reference path="../../../dist/cdc.d.ts" />
var CloudDataConnector;
(function (CloudDataConnector) {
    var AzureDataService = (function () {
        function AzureDataService() {
            this.tableNames = new Array();
        }
        AzureDataService.prototype.addSource = function (urlOrClient, secretKey, tableNames) {
            var client = (urlOrClient.substring) ? new WindowsAzure.MobileServiceClient(urlOrClient, secretKey) : urlOrClient;
            this.azureClient = client;
            this.tableNames = tableNames;
        };
        // the callback is called with an array of objects { tableName: <tableName>, table: <array> }
        AzureDataService.prototype.get = function (updateCallback, lastSyncDates) {
            this.dataAvailableCallback = updateCallback;
            var count = 0;
            var total = this.tableNames.length;
            var result = [];
            var tableName;
            for (var x = 0; x < total; x++) {
                tableName = this.tableNames[x];
                var lastSyncDate = lastSyncDates[tableName];
                this._getTable(tableName, function (resultElement) {
                    count++;
                    updateCallback([resultElement]);
                    if (count == total) {
                    } //!+ request is finished.  Might be interesting to have a callback to top level code called at this point.
                }, lastSyncDate);
            }
        };
        AzureDataService.prototype._getTable = function (tableName, callback, lastDate) {
            var Table = this.azureClient.getTable(tableName);
            var firstCall = false;
            if (!lastDate) {
                lastDate = new Date(null);
                firstCall = true;
            }
            // Since the server sets the updateData and we are doiug a sort on date we assume we will never miss an item as long as we query from our latest update date.  
            Table.where(function (lastDateParam, firstCallParam) {
                return (firstCallParam && !this.sync_deleted) || (!firstCallParam && this.sync_updated > lastDateParam);
            }, lastDate, firstCall).orderBy("sync_updated").take(100).read().done(function (table) {
                //!!! Bug - need logic to send the query again until no more read.  Right now we only read 100 entries in our solution.
                var result = { 'tableName': tableName, 'table': table };
                callback(result);
            }, function (err) {
                console.log(err);
                callback(null);
            });
        };
        AzureDataService.prototype.remove = function (tableName, entity, onsuccess, onerror) {
            var Table = this.azureClient.getTable(tableName);
            Table.del({ id: entity.id }).then(onsuccess, onerror);
        };
        AzureDataService.prototype.update = function (tableName, entity, onsuccess, onerror) {
            var Table = this.azureClient.getTable(tableName);
            delete entity.$$hashKey;
            Table.update(entity).then(function (newProperty) {
                onsuccess(newProperty);
            }, onerror);
        };
        AzureDataService.prototype.add = function (tableName, entity, onsuccess, onerror) {
            var Table = this.azureClient.getTable(tableName);
            delete entity.$$hashKey;
            Table.insert(entity).then(function (newProperty) {
                onsuccess(newProperty);
            }, onerror);
        };
        return AzureDataService;
    })();
    CloudDataConnector.AzureDataService = AzureDataService;
})(CloudDataConnector || (CloudDataConnector = {}));
