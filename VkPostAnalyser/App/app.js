(function (angular, c3, d3) {
    'use strict';
    var postAnalyser = angular.module('postAnalyser', ["ngRoute"]);
    postAnalyser.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/:myReports?', {
            templateUrl: '/App/reportsList.html'
        });
    }]);
    postAnalyser.controller("reportsListController", ["$scope", "$http", "$routeParams", function ($scope, $http, $routeParams) {
        var vm = this, myReports = !!$routeParams.myReports, retrieveReports = function (lastDate, firstDate) {
            var query = "/api/Reports?mineOnly=" + myReports;
            if (firstDate) {
                query += "&firstDate=" + firstDate;
            }
            if (lastDate) {
                query += "&lastDate=" + lastDate;
            }
            vm.dataLoading = true;
            $http.get(query).then(function (response) {
                if (!vm.firstDate) {
                    vm.firstDate = response.data.FirstDate;
                }
                if (!firstDate) {
                    vm.hasMore = response.data.HasMore;
                    vm.reports = vm.reports ? vm.reports.concat(response.data.Reports) : response.data.Reports;
                    vm.lastDate = response.data.LastDate;
                } else if(response.data.Reports.length > 0) {
                    vm.reports = vm.reports ? response.data.Reports.concat(vm.reports) : response.data.Reports;
                }
                vm.dataLoading = false;
            });
        };
        retrieveReports();
        vm.orderReport = function () {
            $http.post("/api/Reports", { UserAlias: vm.userAlias }).then(function (response) {
                vm.reports.unshift(response.data);
            });
        }
        vm.update = function () {
            retrieveReports(null, vm.firstDate);
        };
        vm.loadMore = function () {
            retrieveReports(vm.lastDate);
        };
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
