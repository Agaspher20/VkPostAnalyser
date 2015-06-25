(function (angular, c3, d3) {
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
                chartView: "=",
                chartWidth: "=",
                chartHeight: "="
            },
            link: function (scope, element) {
                var chartData = scope.chartView,
                    xAxisData,
                    yAxisData,
                    chart;
                if (!chartData) {
                    return;
                }
                xAxisData = chartData.map(function (pi) {
                    return pi.SignsCount;
                });
                yAxisData = chartData.map(function (pi) {
                    return pi.LikesCount;
                });
                xAxisData.unshift("x");
                yAxisData.unshift("Likes Count");
                chart = c3.generate({
                    bindto: element.get(0),
                    data: {
                        x: "x",
                        columns: [
                            xAxisData,
                            yAxisData
                        ]
                    }
                });
            }
        };
    });
}(angular, c3, d3));
