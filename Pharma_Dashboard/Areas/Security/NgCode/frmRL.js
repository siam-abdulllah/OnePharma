app.controller("myCtrl", function ($scope, $http) {
    $scope.EventPerm(7);
    $scope.btnSaveValue = "Save";

    $scope.GetRoleInfoList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "RL/GetRoleList"
        }).then(function (response) {
            if (response.data.length > 0) {
                $scope.gridRoleOptions.data = response.data;
                $('#test').modal('show');
            } else {
                toastr.warning("No Data Found!");
            }
        }, function () {
            toastr.warning("No Data Found!");
        });
    };

    var columnRoleList = [
        { name: 'ID', displayName: "Role Code" },
        { name: 'Name', displayName: "Role Name" },
        { name: 'Code', visible: false }
    ];

    $scope.gridRoleOptions = {
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnRoleList,
        rowTemplate: rowTemplate(),
        onRegisterApi: function (gridApi) {
            $scope.gridRoleOptions = gridApi;
        }
    };

    function rowTemplate() {
        return '<div ng-dblclick="grid.appScope.rowDblClickComp(row)" >' +
            '  <div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader }"  ui-grid-cell></div></div>';
    }

    $scope.rowDblClickComp = function (row) {
        $scope.uiID = row.entity.ID;
        $scope.uiCode = row.entity.Code;
        $scope.RoleName = row.entity.Name;
        $scope.btnSaveValue = "Update";
        $('#test').modal('hide');
    };

    $scope.SaveData = function () {
        $scope.SaveDb = {};
        $scope.SaveDb.Name = $scope.RoleName;
        $scope.SaveDb.Code = $scope.uiCode;
        $scope.SaveDb.ID = $scope.uiID;

        $http({
            method: "post",
            url: MyApp.rootPath + "RL/OperationsMode",
            datatype: "json",
            data: JSON.stringify($scope.SaveDb)
        }).then(function (response) {
            if (response.data.Status === 'Yes') {
                $scope.uiCode = response.data.ID;
                $scope.uiID = response.data.ID;
                $scope.btnSaveValue = "Update";
                if (response.data.Mode === 'I') {

                    toastr.success('Inserted Successfully', 'Success Alert', { timeOut: 2000 });
                } else {
                    toastr.success('Updated Successfully', 'Success Alert', { timeOut: 2000 });
                }

            } else {
                toastr.error(response.data.Status, { timeOut: 2000 });
            }

        });
    };

    $scope.Reset = function () {
        $scope.uiCode = "";
        $scope.uiID = "";
        $scope.RoleName = "";
        $scope.btnSaveValue = "Save";
    };
});