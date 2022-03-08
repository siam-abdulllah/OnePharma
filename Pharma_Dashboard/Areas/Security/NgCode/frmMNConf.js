app.controller("myCtrl", function ($scope, $http) {
    $scope.btnSaveValue = "Save";
    $scope.EventPerm(8);

    $scope.GetRoleList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "RL/GetRoleList"
        }).then(function (response) {
            $scope.RLListCombo = response.data;
        }, function () {
            toastr.error("Error Loading Role");
        });
    };

    $scope.GetRoleList();

    $scope.GetHeadMenuList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "MH/GetHeadMenuList"
        }).then(function (response) {
            $scope.MHListCombo = response.data;
        }, function () {
            toastr.error("Error Loading Menu Head");
        });
    };

    $scope.GetHeadMenuList();

    $scope.GetSubMenuList = function () {
        if ($scope.MH_ID === null || $scope.MH_ID === "") {
            toastr.warning("Please Select Menu Head!");
            return false;
        }
        else {
            $http({
                method: "POST",
                url: MyApp.rootPath + "SM/GetSubMenuListNyMHID",
                data: { RL_ID: $scope.RL_ID, MH_ID: $scope.MH_ID }
            }).then(function (response) {
                $scope.SMListCombo = response.data;
                if ($scope.RL_ID == null || $scope.RL_ID == "") {
                    toastr.warning("Please Select Role Name!");
                    return false;
                }
                else {
                    $scope.GetMNConfListByMHRL();
                }
            }, function () {
                alert("Error Loading Sub Menu");
            });
        }
    };

    var columnRLConfList = [
        { name: 'ID', displayName: "ID", visible: false },
        { name: 'RL_ID', displayName: "Name", visible: false },
        { name: 'RL_NM', displayName: "Role" },
        { name: 'MH_ID', displayName: "Department", visible: false },
        { name: 'MH_NM', displayName: "Menu Head" },
        { name: 'SM_ID', displayName: "Sequence", visible: false },
        { name: 'SM_NM', displayName: "Sub Menu" },
        { name: 'Sv', displayName: 'Save Permission' },
        //{ name: 'Vw', displayName: 'View Permission' },
        { name: 'Dl', displayName: 'Delete Permission' },
        { name: 'btnDelete', enableFiltering: false, enableSorting: false, displayName: "Action", cellTemplate: '<button type="button" ng-click="grid.appScope.DeleteAccess(row.entity)">Delete</button>' },
    ];

    $scope.gridMNConfOptions = {
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnRLConfList,
        rowTemplate: rowTemplate(),
        onRegisterApi: function (gridApi) {
            $scope.gridMNConfOptions = gridApi;
        }
    };

    $scope.GetMNConfListByMHRL = function () {
        $scope.SM_ID = "";
        $scope.btnSaveValue = "Save";
        $scope.gridMNConfOptions.data = [];
        if ($scope.MH_ID == null || $scope.MH_ID == "") {
            toastr.warning("Please Select Menu Head!");
            return false;
        }
        else {
            $http({
                method: "POST",
                url: MyApp.rootPath + "MNConf/GetMNConfListByMHRL",
                data: { RL_ID: $scope.RL_ID, MH_ID: $scope.MH_ID }
            }).then(function (response) {
                if (response.data.length > 0) {
                    $scope.gridMNConfOptions.data = response.data;
                }
                else {
                    toastr.warning("No Data Found!");
                }
            },
                function () {
                    toastr.error("Error Loading Menu Grid");
                });
        }
    };
    $scope.OnRoleClick = function () {
        $scope.MH_ID = "";
        $scope.MH_ID = "";
        $scope.gridMNConfOptions.data = [];
    };

    $scope.SaveData = function () {
        $scope.SaveDb = {};

        $scope.SaveDb.SM_ID = $scope.SM_ID;
        $scope.SaveDb.MH_ID = $scope.MH_ID;
        $scope.SaveDb.RL_ID = $scope.RL_ID;
        $scope.SaveDb.Sv = $scope.Sv;
        $scope.SaveDb.Dl = $scope.Dl;
        $scope.SaveDb.ID = $scope.uiID;
        $http({
            method: "post",
            url: MyApp.rootPath + "MNConf/OperationsMode",
            datatype: "json",
            data: JSON.stringify($scope.SaveDb)
        }).then(function (response) {
            if (response.data.Status === 'Yes') {
               // $scope.uiID = response.data.ID;
                $scope.btnSaveValue = "Update";
                $scope.GetMNConfListByMHRL();
                $scope.GetSubMenuList();
                if (response.data.Mode === 'I') {
                    toastr.success('Inserted Successfully', 'Success Alert', { timeOut: 2000 });
                } else {
                    toastr.success('Updated Successfully', 'Success Alert', { timeOut: 2000 });
                }

            } else {
                toastr.error(response.data.Status, { timeOut: 2000 });
            }

        });
    }

    $scope.DeleteAccess = function (row) {
        $scope.ID = row.ID;

        if ($scope.ID !== '') {
            $http({
                method: "post",
                url: MyApp.rootPath + "MNConf/DeleteMNConf",
                datatype: "json",
                data: { ID: $scope.ID }
            }).then(function (response) {
                $scope.GetMNConfListByMHRL();
                $scope.GetSubMenuList();
                OperationMsg(response.data.Mode);
            });
        }
    };

    function rowTemplate() {
        return '<div ng-dblclick="grid.appScope.rowDblClickComp(row)" >' +
            '  <div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader }"  ui-grid-cell></div></div>';
    }
    $scope.SubMenuChange = function () {
        $scope.uiID = "";
    };
    $scope.rowDblClickComp = function (row) {
        var d = [{
            ID: row.entity.SM_ID,
            Name: row.entity.SM_NM
        }];
        $scope.SMListCombo = d;
        $scope.RL_ID = row.entity.RL_ID;
        $scope.MH_ID = row.entity.MH_ID;
        $scope.SM_ID = row.entity.SM_ID;
        $scope.uiID = row.entity.ID;
        $scope.Sv = row.entity.Sv;
        $scope.Dl = row.entity.Dl;
      
        $scope.btnSaveValue = "Update";
    };

    $scope.Reset = function () {
        $scope.btnSaveValue = "Save";
        $scope.uiID = "";
        $scope.RL_ID = "";
        $scope.MH_ID = "";
        $scope.SM_ID = "";
        $scope.Sv = "No";
        $scope.Dl = "No";
        $scope.gridMNConfOptions.data = [];
        $scope.SMListCombo = [];
    };
});