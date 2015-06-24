(function (angular) {
    'use strict';
    var postAnalyser = angular.module('postAnalyser', ["ngRoute"]);
    postAnalyser.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/:myReports?', {
            templateUrl: '/App/reportsList.html'
        });
    }]);
    postAnalyser.controller("reportsListController", ["$scope", "$http", "$routeParams", function ($scope, $http, $routeParams) {
        var vm = this, myReports = !!$routeParams.myReports;
        $http.get("/api/Reports?mineOnly=" + myReports).then(function (response) {
            vm.reports = response.data.Reports;
        });
        vm.orderReport = function () {
            $http.post("/api/Reports", { userId: vm.userId }).then(function (response) {
                vm.reports.unshift(response.data);
            });
        }
        return vm;
    }]);
    postAnalyser.directive("chartView", function () {
        return {
            replace: false,
            restrict: 'A',
            scope: {
                chartView: "="
            },
            link: function (scope, element) {
                var report = scope.chartView;
                var p = $("<p>");
                p.append(report.UserId);
                element.append(p);
            }
        };
    });
}(angular));
