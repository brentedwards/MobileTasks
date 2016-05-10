
var app = angular.module('MobileTasks', ['ionic', 'ngCordova']);

app.run(function ($ionicPlatform, $state) {
    $ionicPlatform.ready(function () {

        // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
        // for form inputs)
        if (window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard) {
            cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
            cordova.plugins.Keyboard.disableScroll(true);

        }

        if (window.StatusBar) {
            StatusBar.styleLightContent();
        }
    });
});

app.config(function ($stateProvider, $urlRouterProvider) {

    // Ionic uses AngularUI Router which uses the concept of states
    // Learn more here: https://github.com/angular-ui/ui-router
    // Set up the various states which the app can be in.
    // Each state's controller can be found in controllers.js
    $stateProvider
        .state('login', {
            url: '/login',
            templateUrl: 'templates/login.html',
            controller: 'LoginController'
        });

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');

});

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}


//var onDeviceReady = function () {
//    console.log("Application start");
//    angular.bootstrap(document.body, ['MobileTasks']);
//}

//document.addEventListener('deviceready', onDeviceReady.bind(this), false);