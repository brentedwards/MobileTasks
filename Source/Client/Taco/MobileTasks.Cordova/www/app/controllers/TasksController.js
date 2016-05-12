app.controller('TasksController', function ($scope, $ionicPopup, $state, network, $ionicHistory, $filter) {

    $scope.isIOS = ionic.Platform.isIOS();
    $scope.isAndroid = ionic.Platform.isAndroid();

    $scope.getFormattedDateDue = function(taskDateDue) {
        if (taskDateDue == null) {
            return "No Due Date";
        } else {
            return $filter('date')(taskDateDue, "EEE, M/d/yy h:mm a");
        }
    }

    $scope.getIconForTask = function (taskComplete, taskDateDue) {
        if (taskComplete == null || taskDateDue == null) {
            return "images/icon-incomplete.png";
        } else if (taskComplete) {
            return "images/icon-completed.png";
        } else if (new Date(taskDateDue) < new Date()) {
            return "images/icon-pastdue.png";
        } else {
            return "images/icon-incomplete.png";
        }
    }

    $scope.switchIsChanged = function(taskId) {
        var returnValue = [];
        angular.forEach($scope.tasks, function (value, key) {
            if (value.id == taskId) {
                this.push(value);
            }
        }, returnValue);

        var task = returnValue[0];
        if (!task.isCompleted) {
            task.isCompleted = true;
            network.upsertTask(task).then(function (result) {
                if (result) {
                    task = result;
                }
            }).catch(function (error) {
                    alert("not good");
            });
        }
    }

    $scope.addTask = function () {
        $ionicHistory.nextViewOptions({
            historyRoot: false,
            disableAnimate: false,
            disableBack: false
        });
        $state.go('task');
    }

    $scope.logout = function() {
        network.logout().then(function (result) {
            $ionicHistory.nextViewOptions({
                historyRoot: true,
                disableAnimate: false,
                disableBack: true
            });
            $state.go('login');
        });
    }

    $scope.$on("$ionicView.beforeEnter", function (event, data) {
        $ionicHistory.clearHistory();
    });

    $scope.$on("$ionicView.enter", function (event, data) {
        network.getTasks().then(function (result) {
            if (result) {
                $scope.tasks = result;
            }
        }).catch(function(error) {
            if (error === "Unauthorized") {
                network.logout().then(function (result) {
                    $ionicHistory.nextViewOptions({
                        historyRoot: true,
                        disableAnimate: false,
                        disableBack: true
                    });
                    $state.go('login');
                });
            } else {
                alert("not good");
            }
        });
    });
});