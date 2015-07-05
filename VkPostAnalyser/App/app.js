(function (angular, c3, toastr, window) {
    'use strict';
    var postAnalyser = angular.module('postAnalyser', ["ngRoute", "ngCookies"], ['$httpProvider', '$provide', function ($httpProvider, $provide) {
        $provide.factory('responseInterceptor', ['$q', function ($q) {
            return {
                request: function (config) { return config; },
                requestError: function (rejection) { return $q.reject(rejection); },
                response: function (response) {
                    return response;
                },
                responseError: function (response) {
                    if (response.status === 401) {
                        window.location = "/Account/Login";
                    }
                    var message = "Error " + response.status;
                    if (response.data) {
                        message += ": " + (response.data.Message || response.data);
                    }
                    toastr.error(message);
                    return $q.reject(response);
                }
            };
        }]);
        $httpProvider.interceptors.push('responseInterceptor');
    }]);
    postAnalyser.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/:mineOnly?', {
            templateUrl: '/App/reportsList.html'
        });
    }]);
    postAnalyser.factory("dataContext", ["$http", function ($http) {
        var retrieveReports = function (action, date, mineOnly, skipIds) {
            var query = "/api/Reports/" + action + "?mineOnly=" + !!mineOnly, i, skipIdsLength;
            if (date) {
                query += "&date=" + date;
            }
            if (skipIds && skipIds.length) {
                skipIdsLength = skipIds.length;
                for (i = 0; i < skipIdsLength; ++i) {
                    if (query.length > 2000) {
                        break;
                    }
                    query += "&skipIds=" + skipIds[i];
                }
            }
            return $http.get(query, { cache: false });
        };
        return {
            nextPage: function (lastDate, mineOnly) {
                return retrieveReports("NextPage", lastDate, mineOnly);
            },
            newReports: function (firstDate, skipReports, mineOnly) {
                return retrieveReports("NewReports", firstDate, mineOnly, skipReports);
            },
            postReport: function (userId) {
                return $http.post("/api/Reports/OrderReport", { UserId: userId });
            }
        };
    }]);
    postAnalyser.factory("membershipProvider", ["$cookies", function ($cookies) {
        return {
            isAuthenticated: function () {
                console.log($cookies.get(".AspNet.ApplicationCookie"));
                return !!$cookies.get(".AspNet.ApplicationCookie");
            }
        };
    }]);
    postAnalyser.controller("reportsListController", ["$routeParams", "dataContext", "membershipProvider", function ($routeParams, dataContext, membershipProvider) {
        var vm = this, mineOnly = $routeParams.mineOnly && ($routeParams.mineOnly === "myReports"),
            skipReports,
            addToSkipList = function (reportId) {
                if (skipReports) {
                    skipReports.push(reportId);
                }
                else {
                    skipReports = [reportId];
                }
            },
            onReportCreated = function (viewModel, reportPromise) {
                reportPromise.then(function (response) {
                    if (!viewModel.reports) {
                        viewModel.reports = [response.data];
                    }
                    else {
                        viewModel.reports.unshift(response.data);
                    }
                    viewModel.reportCreation = false;
                    addToSkipList(response.data.Id);
                    toastr.success("Success");
                }, function () {
                    viewModel.reportCreation = false;
                });
            };
        vm.isAuthenticated = membershipProvider.isAuthenticated();
        vm.loadMore = function () {
            vm.nextPageLoading = true;
            dataContext.nextPage(vm.lastDate, mineOnly).then(function (response) {
                var model = response.data;
                vm.nextPageLoading = false;
                if (response.status === 204) {
                    vm.hasMore = false;
                    toastr.warning("All pages were loaded");
                    return;
                }
                if (!vm.firstDate) {
                    vm.firstDate = model.FirstDate;
                }
                vm.reports = vm.reports ? vm.reports.concat(model.Reports) : model.Reports;
                vm.hasMore = true;
                vm.lastDate = model.LastDate;
            }, function () {
                vm.nextPageLoading = false;
            });
        };
        vm.update = function () {
            if (!vm.reports || !vm.reports.length) {
                vm.loadMore();
                return;
            }
            vm.newReportsLoading = true;
            dataContext.newReports(vm.firstDate, skipReports, mineOnly).then(function (response) {
                var model = response.data;
                vm.newReportsLoading = false;
                if (response.status === 204) {
                    toastr.success("No Updates");
                    return;
                }
                if (!vm.lastDate) {
                    vm.lastDate = model.LastDate;
                }
                if (vm.reports) {
                    vm.reports = vm.reports ? model.Reports.concat(vm.reports) : model.Reports;
                }
                if (model.FirstDate) {
                    vm.firstDate = model.FirstDate;
                }
                skipReports = null;
            }, function () {
                vm.newReportsLoading = false;
            });
        };
        vm.onKeyboardSubmit = function ($event) {
            if ($event.which === 13) {
                vm.orderReport();
            }
        };
        vm.orderReport = function () {
            if (vm.reportCreation) {
                return;
            }
            vm.reportCreation = true;
            onReportCreated(vm, dataContext.postReport(vm.userId));
        };
        vm.myReport = function () {
            if (vm.reportCreation) {
                return;
            }
            vm.reportCreation = true;
            onReportCreated(vm, dataContext.myReport());
        }
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
                    yAxisData;
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
                c3.generate({
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
    postAnalyser.directive("naturalInt", function () {
        return {
            replace: false,
            restrict: 'A',
            require: 'ngModel',            
            link: function(scope, element, attributes, ngModel) {
                ngModel.$validators.naturalInt = function (modelValue) {
                    var intValue = parseInt(modelValue, 10);
                    return modelValue === intValue && intValue > 0;
                };
            }
        };
    });
}(angular, c3, toastr, window));
