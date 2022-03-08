app.controller("myCtrl", function ($scope, $http) {
    $scope.btnSaveValue = "Save";
    $scope.passwordDisabled = true;
    $scope.CrntPassChq = false;
    $scope.validateCurrentPassword = false;
    $scope.passwordValidation = true;
    $scope.CheckCurrentPassword = function () {
        $http({
            method: "post",
            url: MyApp.rootPath + "ChangPass/CheckCurrentPassword",
            datatype: "json",
            data: { currentPassword: $scope.CurrentPassword }
        }).then(function (response) {
            if (response.data.Stat === true) {
                $scope.passwordDisabled = false;
                $scope.CrntPassChq = false;
                $scope.validateCurrentPassword = true;
            } else {
                $scope.passwordDisabled = true;
                $scope.CrntPassChq = true;
                $scope.validateCurrentPassword = false;
                $scope.passwordValidation = true;
                $scope.Password = "";
                $scope.RePassword = "";
            }
        });
    };
    $scope.CheckPasswordValidity = function () {
        if ($scope.Password === $scope.RePassword) {
            $scope.passwordValidation = false;
        } else {
            $scope.passwordValidation = true;
        }
    };
    $scope.SaveData = function () {
        $http({
            method: "post",
            url: MyApp.rootPath + "ChangPass/UpdatePassword",
            datatype: "json",
            data: { Password: $scope.Password }
        }).then(function (response) {
            if (response.data.Status === "Yes") {
                OperationMsg(response.data.Mode);
                $scope.Reset();
            } else {
                toastr.error("Failed!");
            }
        });
    };
    //reset
    $scope.Reset = function () {
        $scope.passwordDisabled = true;
        $scope.CrntPassChq = false;
        $scope.validateCurrentPassword = false;
        $scope.passwordValidation = true;
        $scope.CurrentPassword = "";
        $scope.Password = "";
        $scope.RePassword = "";
    };
});