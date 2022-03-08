app.controller("myCtrl", function ($scope, $http) {
    $scope.EventPerm(15);
    $scope.btnSaveValue = "Save";

    $scope.GetRoleList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "RL/GetRoleList"
        }).then(function (response) {
            $scope.RLListCombo = response.data;
        }, function () {
            alert("Error Loading Role");
        });
    };

    $scope.GetRoleList();

    $scope.GetEmployeeList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "RoleConf/GetActiveEmployeeInfoList"
        }).then(function (response) {
            if (response.data !== "") {
                $scope.Employees = response.data;
            } else {
                toastr.warning("No Data Found!");
            }
        }, function () {
            toastr.warning("Error Occurred!");
        });
    };

    $scope.GetEmployeeList();

    $scope.GetEmployeeByRoleList = function () {
        $http({
            method: "POST",
            url: MyApp.rootPath + "RoleConf/GetEmployeeByRoleList",
            data: { roleId: $scope.RL_ID },
        }).then(function (response) {
            $scope.gridRLConfOptions.data = response.data;
        }, function () {
            toastr.warning("Error Occurred!");
        });
    };

    var columnRLConfList = [
        { name: 'UserID', displayName: "UserID", visible: false },
        { name: 'EmployeeCode', displayName: "Employee Code",width:'15%' },
        { name: 'EmployeeName', displayName: "Employee Name",width:'30%' },
        { name: 'DesignationCode', displayName: " Designation",width:'20%' },
        { name: 'DesignationDetail', displayName: " Designation Detail",width:'35%'}
    ];
    
    $scope.gridRLConfOptions = {
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnRLConfList,
        rowTemplate: rowTemplate(),
        onRegisterApi: onRegisterApi
        
    };
    var onRegisterApi = function (gridApi) {
        $scope.gridRLConfOptions = gridApi;
    };
    function rowTemplate() {
        return '<div ng-dblclick="grid.appScope.rowDblClickComp(row)" >' +
            '  <div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader }"  ui-grid-cell></div></div>';
    }

    $scope.rowDblClickComp = function (row) {
        $scope.frmRoleConf.USER_ID = row.entity.UserID;
        $scope.btnSaveValue = "Update";
    };

    $scope.SaveData = function () {
        $scope.SaveDb = {};
        $scope.SaveDb.RL_ID = $scope.RL_ID;
        $scope.SaveDb.USER_ID = $scope.frmRoleConf.USER_ID;

        $http({
            method: "post",
            url: MyApp.rootPath + "RoleConf/SaveRLConf",
            datatype: "json",
            data: JSON.stringify($scope.SaveDb)
        }).then(function (response) {
            if (response.data.Status == "Yes") {
                OperationMsg(response.data.Mode);
                $scope.GetEmployeeByRoleList();
                //$scope.uiCode = response.data.Code;
            } else {
                toastr.error("Failed!");
            }
        });
    };

    $scope.Reset = function () {
        $scope.frmRoleConf.USER_ID = "";
        $scope.RL_ID = "";
        $scope.gridRLConfOptions.data = [];
        $scope.btnSaveValue = "Save";
    };
});