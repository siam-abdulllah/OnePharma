var applg = angular.module("myApp", ['ngTouch','ngSanitize']);
applg.directive('loading', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };
            scope.$watch(scope.isLoading,
                function (value) {
                    if (value) {
                        element.removeClass('ng-hide');
                        //element.parent().addClass('blur');
                        $(".limiter").addClass('blur');
                    } else {
                        element.addClass('ng-hide');
                        $(".limiter").removeClass('blur');
                    }
                });
        }
    };
}]);
applg.directive('autocomplete', function () {

    return {

        restrict: 'A',
        link: function ($scope, el, attr) {

            el.bind('change',
                function (e) {
                    e.preventDefault();
                });
        }
    }

});
applg.controller("myCtrl", function ($scope, $http, $window) {

    $scope.LoginSystem = function () {

        if (($scope.Username === "" || $scope.Username === undefined) || ($scope.Password === undefined || $scope.Password === "")) {
            toastr.error("Invalid Credential!", '');
            return;
        } else {
            $scope.SaveDb = {};

            $scope.SaveDb.Username = $scope.Username;
            $scope.SaveDb.Password = $scope.Password;

            $http({
                method: "post",
                url: MyApp.rootPath + "Home/TryLogin",
                datatype: "json",
                data: JSON.stringify($scope.SaveDb)
            }).then(function (response) {
                if (response.data.Status === "Ok") {
                    //if (response.data.role_name === "FSM") {
                    //   $window.location.href = '/FSM/DashboardNationalPrescription/frmDashboardNationalPrescription';
                   //} else {
                       //$window.location.href = '/Dashboard/HomeDashboard/frmHomeDashboard';
                        $window.location.href = '/Home/Index';
                  // }

                } else {
                    toastr.error("Invalid Credential!", '');
                }
            });
        };
    }
});