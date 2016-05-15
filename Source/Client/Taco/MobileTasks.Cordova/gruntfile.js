module.exports = function (grunt) {
    grunt.initConfig({
        bowercopy: {
            options: {
                clean: false
            },
            ionic: {
                files: {
                    'www/lib/ionic/css': 'ionic/release/css/*.*',
                    'www/lib/ionic/fonts': 'ionic/release/fonts/*.*',
                    'www/lib/ionic/js': 'ionic/release/js/*.*'
                }
            },
            angular: {
                files: {
                    'www/lib/angular': 'angular/angular.min.js'
                }
            },
            angularanimate: {
                files: {
                    'www/lib/angular-animate': 'angular-animate/angular-animate.min.js'
                }
            },
            angularsanitize: {
                files: {
                    'www/lib/angular-sanitize': 'angular-sanitize/angular-sanitize.min.js'
                }
            },
            angularuirouter: {
                files: {
                    'www/lib/angular-ui-router': 'angular-ui-router/release/angular-ui-router.min.js'
                }
            },
            ngCordova: {
                files: {
                    'www/lib/ngcordova': 'ngCordova/dist/ng-Cordova.min.js'
                }
            }
        }
    });

    grunt.registerTask("default", ["bowercopy"]);
    grunt.loadNpmTasks("grunt-bowercopy");
};