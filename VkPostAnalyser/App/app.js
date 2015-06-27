(function (angular, c3, d3) {
    'use strict';
    var postAnalyser = angular.module('postAnalyser', ["ngRoute"]);
    postAnalyser.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/:mineOnly?', {
            templateUrl: '/App/reportsList.html'
        });
    }]);
    postAnalyser.factory("dataContext", ["$http", "$q", function ($http, $q) {
        var retrieveReports = function (action, date, mineOnly) {
            var query = "/api/Reports/" + action + "?mineOnly=" + mineOnly;
            if (date) {
                query += "&date=" + date;
            }
            return $http.get(query, { cache: false }).then(function (response) {
                return response.data;
            }, function (response) {
                return $q.reject(response);
            });
        };
        return {
            nextPage: function (lastDate, mineOnly) {
                return retrieveReports("NextPage", lastDate, mineOnly);
            },
            newReports: function (firstDate, mineOnly) {
                return retrieveReports("NewReports", firstDate, mineOnly)
            },
            postReport: function (userAlias) {
                return $http.post("/api/Reports/Post", { UserAlias: userAlias }).then(function (response) {
                    return response.data;
                }, function (response) {
                    return $q.reject(response);
                });
            }
        };
    }]);
    postAnalyser.controller("reportsListController", ["$scope", "$routeParams", "dataContext", function ($scope, $routeParams, dataContext) {
        var vm = this, mineOnly = !!$routeParams.mineOnly;
        vm.loadMore = function () {
            vm.nextPageLoading = true;
            dataContext.nextPage(vm.lastDate, mineOnly).then(function (model) {
                if (!vm.firstDate) {
                    vm.firstDate = model.FirstDate;
                }
                vm.reports = vm.reports ? vm.reports.concat(model.Reports) : model.Reports;
                vm.hasMore = model.HasMore;
                vm.lastDate = model.LastDate;
                vm.nextPageLoading = false;
            }, function (response) {
                vm.nextPageLoading = false;
            });
        };
        vm.update = function () {
            if (!vm.reports || !vm.reports.length) {
                vm.loadMore();
                return;
            }
            vm.newReportsLoading = true;
            dataContext.newReports(vm.firstDate, mineOnly).then(function (model) {
                if (!vm.lastDate) {
                    vm.lastDate = model.LastDate;
                }
                if (vm.reports) {
                    vm.reports = vm.reports ? model.Reports.concat(vm.reports) : model.Reports;
                }
                if (model.FirstDate) {
                    vm.firstDate = model.FirstDate;
                }
                vm.newReportsLoading = false;
            }, function (response) {
                vm.newReportsLoading = false;
            });
        };
        vm.orderReport = function () {
            dataContext.postReport(vm.userAlias).then(function (report) {
                vm.reports.unshift(report);
            });
        };
        vm.loadMore();
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
