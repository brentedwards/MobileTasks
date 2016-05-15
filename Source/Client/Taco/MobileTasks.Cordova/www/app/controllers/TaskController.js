app.controller('TaskController', function($scope, $ionicPopup, $state, network, $ionicHistory, $filter) {
    $scope.isIOS = ionic.Platform.isIOS();
    $scope.isAndroid = ionic.Platform.isAndroid();
    $scope.task = { sid : "", id: "", description: "", isCompleted: false, dateCompleted: null, dateCreated: null, dateDue: null};

    $scope.specifyDueDate = false;

    $scope.close = function() {
        $ionicHistory.goBack();
    }

    $scope.save = function () {
        network.upsertTask($scope.task).then(function (result) {
            if (result) {
                $ionicHistory.goBack();
            }
        }).catch(function (error) {
            alert("not good");
        });
    }
});