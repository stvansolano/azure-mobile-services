/* globals angular */
"use strict";

var angular = window.angular || {};
(function (angular, document) {
    'use strict';

    var app = angular.module('app', []);

    function readFile($q, filePath) {

        var deferred = $q.defer();

        var reader = new FileReader();

        // Closure to capture the file information.
        reader.onload = (function (theFile) {
            return function (e) {
                // Render thumbnail.
                var span = document.createElement('span');
                span.innerHTML = ['<img class="thumb" src="', e.target.result,
                                  '" title="', escape(theFile.name), '"/>'].join('');
                document.getElementById('preview').insertBefore(span, null);

                console.log('file ready');
                deferred.resolve(e.target.result);
            };
        })(filePath);

        // Read in the image file as a data URL.
        reader.readAsDataURL(filePath);

        return deferred.promise;
    }

    function postImage($q, $http, body) {

        var deferred = $q.defer();
        var cache = [];
        var content = JSON.stringify(body, function (key, value) {
            if (typeof value === 'object' && value !== null) {
                if (cache.indexOf(value) !== -1) {
                    // Circular reference found, discard key
                    return;
                }
                // Store value in our collection
                cache.push(value);
            }
            return value;
        });
        cache = null;

        console.log(body);

        $http.post('/moments/', content)
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (msg, code) {
                deferred.reject(msg);
                //$log.error(msg, code);
            });
        return deferred.promise;
    }
    
    app.controller('moments', ['$scope', '$q', '$http', 
        function ($scope, $q, $http) {
            $scope.send = function (model) {

                var fileDeferred = readFile($q, model.theFile);

                var buildMoment = function (from, to, fileContents) {
                    var recipients = [];

                    recipients.push(to);

                    return {
                        Recipients: recipients,
                        SenderId: from,
                        Attached: fileContents
                    };
                }

                fileDeferred.then(function (fileContents) {
                    var body = buildMoment(model.from, model.recipients, fileContents)

                    console.log(body);

                    return postImage($q, $http, body);
                });
            }
        }]);

    app.directive('file', function () {
        return {
            scope: {
                file: '='
            },
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var files = event.target.files;
                    var file = files[0];
                    scope.file = file || {};
                    scope.$apply();
                });
            }
        };
    });
})(angular, document);