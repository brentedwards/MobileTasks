app.factory('network', function ($q) {

    var azureUrl = "<enter URL here>";

    var mobileService = {};
    var azureService = new WindowsAzure.MobileServiceClient(azureUrl);

    var lastUsedProvider = "LastUsedProvider";
    var userId = "|UserId";
    var token = "|Token";

    // Login
    mobileService.login = function (provider) {
        var deferred = $q.defer();

        azureService.login(provider).done(function(results) {

            var prefs = plugins.appPreferences;
            prefs.store(function() {
                prefs.store(function() {
                    prefs.store(function() {
                        deferred.resolve(true);
                    }, function () { deferred.resolve(false); }, provider + token, azureService.currentUser.mobileServiceAuthenticationToken);
                }, function (error) { deferred.resolve(false); }, provider + userId, results.userId);
            }, function (error) { deferred.resolve(false); }, lastUsedProvider, provider);
        }, function (err) {
            deferred.resolve(false);
        });

        return deferred.promise;
    };

    mobileService.hasPreviousAuthentication = function() {
        var deferred = $q.defer();

        var prefs = plugins.appPreferences;
        prefs.fetch(function(provider) {
            prefs.fetch(function(uid) {
                prefs.fetch(function (uToken) {
                    azureService.currentUser = { "userId": uid, "mobileServiceAuthenticationToken": uToken };

                    deferred.resolve(true);
                }, function () { deferred.resolve(false); }, provider + token);
            }, function (error) { deferred.resolve(false); }, provider + userId);
        }, function (error) { deferred.resolve(false); }, lastUsedProvider);

        return deferred.promise;
    }

    // Get Status List
    //azureService.getIncidentList = function (assignedToUserId) {

    //    var deferred = $q.defer();

    //    var incidentTable = mobileService.getTable('Incident')
    //      .take(1000)
    //      .where({

    //          assignedToId: assignedToUserId

    //      })
    //      .read().done(function (incidentTable) {

    //          deferred.resolve(incidentTable);

    //      }, function (err) {

    //          alert("Error: " + err);

    //      });

    //    return deferred.promise;

    //};

    return mobileService;

});