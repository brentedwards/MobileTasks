app.controller('LoginController', function ($scope, $ionicPopup, $state, network) {
    
    $scope.processLogin = function(provider) {
        network.login(provider).then(function(result) {
            if (result) {
                $state.go('tasks');
            }
        });
    };

    $scope.$on("$ionicView.beforeEnter", function(event, data){
        network.hasPreviousAuthentication().then(function(result) {
            if (result) {
                $state.go('tasks');
            }
        });
    });
});