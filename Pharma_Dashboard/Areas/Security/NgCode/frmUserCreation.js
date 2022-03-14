app.controller("myCtrl", function ($scope, $http) {
    $scope.btnSaveValue = "Save";
    $scope.EventPerm(43);

    $scope.GetUserList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "UserCreation/GetUserList"
        }).then(function (response) {
            $scope.gridUserCreationOptions.data = response.data;
            $('#test').modal('show');
        }, function () {
            toastr.warning("Error loading UserList");
        });
    };
    $scope.Search = function () {

    };
    var columnUserList = [
        { name: 'EmployeeName', displayName: "EmployeeName" },
        { name: 'EmployeeCode', displayName: "EmployeeCode" },
        { name: 'UserId', displayName: "UserId", visible: false },
        { name: 'DesignationDetail', displayName: "DesignationDetail", visible: false },
        { name: 'PostingLocation', displayName: "PostingLocation", visible: false },
        { name: 'DepotCode', displayName: "DepotCode", visible: false },
        { name: 'Password', displayName: "Password", visible: false },
        { name: 'Status', displayName: "Status" },
        { name: 'GroupCode', displayName: "GroupCode" },
        { name: 'AccessLevel', displayName: "AccessLevel" }
        // { name: 'Code', displayName: "Code" }
    ];

    $scope.gridUserCreationOptions = {
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnUserList,
        rowTemplate: rowTemplate(),
        onRegisterApi: function (gridApi) {
            $scope.gridUserCreationOptions = gridApi;
        }
    };

    function rowTemplate() {
        return '<div ng-dblclick="grid.appScope.rowDblClickComp(row)" >' +
            '  <div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader }"  ui-grid-cell></div></div>';
    }

    $scope.rowDblClickComp = function (row) {
        $scope.frmUserCreation.Employee = row.entity;
        $scope.UserId = row.entity.UserId;
        $scope.Password = row.entity.Password;
        //$scope.conPassword = row.entity.Password;
        $scope.Status = row.entity.Status;
        $scope.GroupCode = row.entity.GroupCode;
        $scope.AccessLevel = row.entity.AccessLevel;
        $scope.EmployeeCode = row.entity.EmployeeCode;
        // $scope.Code = row.entity.Code;
        $scope.btnSaveValue = "Update";
        $('#test').modal('hide');
    };
    $scope.compare = function () {
        if ($scope.Password === $scope.conPassword) {
            $scope.myStyle = { color: 'green' };
            $scope.myText = "Password matched";
            console.log("equal");
        }
        if ($scope.Password !== $scope.conPassword) {
            $scope.myStyle = { color: 'red' };
            $scope.myText = "Confirm Password must match Password";
            console.log("password mismatch");
        }
        //else {
        //    $scope.myText = '<span style="color: red;">Confirm Password is required and must match Password</span>';
        //    console.log("Confirm Password is required");

        //}
    };
    $scope.requiredMsg = function () {
        if (!$scope.Password) {
            //  $scope.myStyle = { color: 'yellow' };
            $scope.myText1 = 'minimum 6 charchter Password is required';
            //console.log("not empty");
        }
        else {
            console.log("empty");
        }
    }

    //
    $scope.SaveData = function () {
        $scope.SaveDb = {};
        $scope.SaveDb.Username = $scope.frmUserCreation.Employee.EmployeeCode;
        $scope.SaveData.UserId = $scope.UserId;
        $scope.SaveDb.Password = $scope.Password;
        $scope.SaveDb.Status = $scope.Status;
        $scope.SaveDb.GroupCode = $scope.GroupCode;
        $scope.SaveDb.AccessLevel = $scope.AccessLevel;
        $scope.SaveDb.PostingLocation = $scope.frmUserCreation.Employee.PostingLocation;
        $scope.SaveDb.DepotCode = $scope.frmUserCreation.Employee.DepotCode;
        $scope.SaveDb.EmployeeCode = $scope.frmUserCreation.Employee.EmployeeCode;

        //
        $http({
            method: "post",
            url: MyApp.rootPath + "UserCreation/OperationsMode",
            datatype: "json",
            data: { userLogin: $scope.SaveDb, userId: $scope.UserId }
        }).then(function (response) {
            if (response.data.Status === 'Yes') {
                if (response.data.Mode === 'I') {
                    $scope.UserId = response.data.ID;
                    // $scope.uiCode = response.data.ID;
                    toastr.success('Inserted Successfully', 'Success Alert', { timeOut: 2000 });
                } else {
                    toastr.success('Updated Successfully', 'Success Alert', { timeOut: 2000 });
                }

            } else {
                toastr.error(response.data.Status, { timeOut: 2000 });
            }

        });
    }
    //
    $scope.GetEmployeeList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "UserCreation/GetActiveEmployeeInfoList"
        }).then(function (response) {
            if (response.data !== "") {
                //console.log(response.data);
                $scope.Employees = response.data;
            } else {
                toastr.warning("No Data Found!");
            }
        }, function () {
            toastr.warning("Error Occurred!");
        });
    };
    $scope.GetEmployeeList();
    //
    $scope.Reset = function () {
        $scope.btnSaveValue = "Save";
        $scope.UserId = "";
        $scope.frmUserCreation.Employee = "";
        $scope.Password = "";
        $scope.conPassword = "";
        $scope.Status = "";
        $scope.AccessLevel = "";
        $scope.CssClass = "";
        $scope.GroupCode = "";
        $scope.gridUserCreationOptions.data = [];

    };
});