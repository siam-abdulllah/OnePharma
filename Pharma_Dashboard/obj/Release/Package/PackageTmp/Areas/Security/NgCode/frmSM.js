app.controller("myCtrl", function ($scope, $http) {
    $scope.btnSaveValue = "Save";
    $scope.EventPerm(6);

    $scope.GetHeadMenuList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "MH/GetHeadMenuList"
        }).then(function (response) {
            $scope.MhListCombo = response.data;
        }, function () {
            alert("Error Loading Category");
        });
    };

    $scope.GetHeadMenuList();
    $scope.GetSubMenuList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "SM/GetSubMenuList"
        }).then(function (response) {
            $scope.gridSMOptions.data = response.data;
            $('#test').modal('show');
        }, function () {
            alert("Error Loading Category");
        });
    };

    var columnSMList = [
        { name: 'ID', displayName: "ID", visible: false },
        { name: 'MH_ID', displayName: "MH_ID", visible: false },
        { name: 'Name', displayName: "Menu Head" },
        { name: 'SubName', displayName: "Sub Menu Name" },
        { name: 'Sequence', displayName: "Sequence" },
        { name: 'Url', displayName: "Url" }
        //{ name: 'btnUpdate', enableFiltering: false, enableSorting: false, displayName: "Update", cellTemplate: '<div><button type="button" ng-click="grid.appScope.UpdateDept(row.entity)">Update</button></div>' },
    ];

    $scope.gridSMOptions = {
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnSMList,
        rowTemplate: rowTemplate(),
        onRegisterApi: function (gridApi) {
            $scope.gridSMOptions = gridApi;
        }
    };

    function rowTemplate() {
        return '<div ng-dblclick="grid.appScope.rowDblClickComp(row)" >' +
            '  <div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader }"  ui-grid-cell></div></div>';
    }

    $scope.rowDblClickComp = function (row) {

        $scope.MH_ID = row.entity.MH_ID;
        $scope.Name = row.entity.SubName;
        $scope.Sequence = row.entity.Sequence;
        $scope.Url = row.entity.Url;
        $scope.uiID = row.entity.ID;

        $('#modalClose').click();
    };

    $scope.SaveData = function () {

        $scope.SaveDb = {};

        $scope.SaveDb.Name = $scope.Name;
        $scope.SaveDb.Sequence = $scope.Sequence;
        $scope.SaveDb.ID = $scope.uiID;
        $scope.SaveDb.Url = $scope.Url;
        $scope.SaveDb.MH_ID = $scope.MH_ID;

        $http({
            method: "post",
            url: MyApp.rootPath + "SM/OperationsMode",
            datatype: "json",
            data: JSON.stringify($scope.SaveDb)
        }).then(function (response) {
            if (response.data.Status === 'Yes') {
                //$scope.GetCompanyList();
                if (response.data.Mode === 'I') {
                    $scope.uiID = response.data.ID;
                    $scope.uiCode = response.data.ID;
                    toastr.success('Inserted Successfully', 'Success Alert', { timeOut: 2000 });
                } else {
                    toastr.success('Updated Successfully', 'Success Alert', { timeOut: 2000 });
                }

            } else {
                toastr.error(response.data.Status, { timeOut: 2000 });
            }

        });
    }

    $scope.Reset = function () {
        $scope.MH_ID = "";
        $scope.Name = "";
        $scope.Sequence = "";
        $scope.uiID = "";
        $scope.Url = "";
    };
});