app.controller("IndexCtrl", function ($scope, $http, $filter) {
    $scope.ViewData = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "Home/ViewCompany"
        }).then(function (response) {
            if (response.data != "") {
                alert()
            } else {
                toastr.warning("No Data Found!");
            }
        },
            function () {
                toastr.warning("Error Occurred");
            });
    }
    //************************end of Docinfo*******************************************//
});