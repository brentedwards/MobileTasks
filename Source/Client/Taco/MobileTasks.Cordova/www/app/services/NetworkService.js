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
            deferred.reject(err.message);
        });
        return deferred.promise;
    };

    mobileService.hasPreviousAuthentication = function() {
        var deferred = $q.defer();

        var prefs = plugins.appPreferences;
        prefs.fetch(function (provider) {
            if (provider != null) {
                prefs.fetch(function(uid) {
                    prefs.fetch(function(uToken) {
                        azureService.currentUser = { "userId": uid, "mobileServiceAuthenticationToken": uToken };

                        deferred.resolve(true);
                    }, function() { deferred.resolve(false); }, provider + token);
                }, function(error) { deferred.resolve(false); }, provider + userId);
            } else { deferred.resolve(false); }
        }, function (error) { deferred.resolve(false); }, lastUsedProvider);

        return deferred.promise;
    }

    mobileService.getTasks = function() {
        var deferred = $q.defer();
        azureService.invokeApi("task", { method: "Get" }).done(function (results) {
            deferred.resolve(JSON.parse(results.response));
        }, function (err) {
            deferred.reject(err.message);
        });

        return deferred.promise;
    }

    mobileService.logout = function() {
        var deferred = $q.defer();
        azureService.logout().done(function(error, result) {
            var prefs = plugins.appPreferences;
            prefs.remove(function () {
                deferred.resolve(true);
            }, function () { deferred.resolve(true); }, lastUsedProvider);
        }, function (err) {
            deferred.reject(err.message);
        });
        return deferred.promise;
    }

    mobileService.upsertTask = function(task) {
        var deferred = $q.defer();
        var stringTask = JSON.stringify(task);
        azureService.invokeApi("task", { body: stringTask }).done(function (results) {
            deferred.resolve(JSON.parse(results.response));
        }, function (err) {
            deferred.reject(err.message);
        });
        return deferred.promise;
    }

    return mobileService;
});