app.controller('LoginController', function ($scope, $ionicPopup, $state, network, $ionicHistory) {
    $scope.processLogin = function(provider) {
        network.login(provider).then(function(result) {
            if (result) {
                $state.go('tasks');
            }
        });
    };

    $scope.$on("$ionicView.beforeEnter", function(event, data){
        $ionicHistory.clearHistory();
        $ionicHistory.nextViewOptions({
            historyRoot: true,
            disableAnimate: false,
            disableBack: true
        });

        network.hasPreviousAuthentication().then(function (result) {
            if (result) {
                $state.go('tasks');
            }
        });
    });
});