module.exports = function (grunt) {
    grunt.initConfig({
        bowercopy: {
            options: {
                clean: false
            },
            ionic: {
                files: {
                    'www/libs/ionic/css': 'ionic/release/css/*.*',
                    'www/libs/ionic/fonts': 'ionic/release/fonts/*.*',
                    'www/libs/ionic/js': 'ionic/release/js/*.*'
                }
            },
            angular: {
                files: {
                    'www/libs/angular': 'angular/angular.min.js'
                }
            },
            angularanimate: {
                files: {
                    'www/libs/angular-animate': 'angular-animate/angular-animate.min.js'
                }
            },
            angularsanitize: {
                files: {
                    'www/libs/angular-sanitize': 'angular-sanitize/angular-sanitize.min.js'
                }
            },
            angularuirouter: {
                files: {
                    'www/libs/angular-ui-router': 'angular-ui-router/release/angular-ui-router.min.js'
                }
            }
        }
    });

    grunt.registerTask("default", ["bowercopy"]);
    grunt.loadNpmTasks("grunt-bowercopy");
};