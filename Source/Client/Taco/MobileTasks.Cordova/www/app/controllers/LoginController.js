app.controller('LoginController', function ($scope, $ionicPopup, $state, network, $ionicHistory) {
    $scope.processLogin = function (provider) {
        alert('sdfsd');
        network.login(provider).then(function(result) {
            if (result) {
                $ionicHistory.nextViewOptions({
                    historyRoot: true,
                    disableAnimate: false,
                    disableBack: true
                });
                $state.go('tasks');
            }
        });
    };

    $scope.$on("$ionicView.beforeEnter", function(event, data){
        $ionicHistory.clearHistory();

        network.hasPreviousAuthentication().then(function (result) {
            if (result) {
                $ionicHistory.nextViewOptions({
                    historyRoot: true,
                    disableAnimate: false,
                    disableBack: true
                });
                $state.go('tasks');
            }
        });
    });
});