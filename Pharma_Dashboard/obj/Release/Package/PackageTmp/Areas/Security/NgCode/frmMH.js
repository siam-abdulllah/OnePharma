app.controller("myCtrl", function ($scope, $http) {
    $scope.btnSaveValue = "Save";
    $scope.EventPerm(5);

   
    //$scope.GetHeadMenuList();
//
    var columnMHList = [
        { name: 'ID', displayName: "ID", visible: false },
        { name: 'Name', displayName: "Name" },
        { name: 'Seq', displayName: "Sequence" }
    ];

    $scope.gridMHOptions = {
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnMHList,
        rowTemplate: rowTemplate(),
        onRegisterApi: function (gridApi) {
            $scope.gridMHOptions = gridApi;
        }
    };

    function rowTemplate() {
        return '<div ng-dblclick="grid.appScope.rowDblClickComp(row)" >' +
            '  <div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader }"  ui-grid-cell></div></div>';
    }

    $scope.rowDblClickComp = function (row) {
        $scope.Name = row.entity.Name;
        $scope.Sequence = row.entity.Seq;
        $scope.uiID = row.entity.ID;
        $scope.btnSaveValue = "Update";
        $('#test').modal('hide');
    };
    //
    $scope.SaveData = function () {
        $scope.SaveDb = {};

        $scope.SaveDb.Name = $scope.Name;
        $scope.SaveDb.Seq = $scope.Sequence;
        $scope.SaveDb.ID = $scope.uiID;

        $http({
            method: "post",
            url: MyApp.rootPath + "MH/OperationsMode",
            datatype: "json",
            data: JSON.stringify($scope.SaveDb)
        }).then(function (response) {
            //$scope.GetHeadMenuList();
            if (response.data.Status === 'Yes') {
                //$scope.GetCompanyList();
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
    //
    $scope.GetHeadMenuList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "MH/GetHeadMenuList"
        }).then(function (response) {
            $scope.gridMHOptions.data = response.data;
            $('#test').modal('show');
        }, function () {
            alert("Error Loading Category");
        });
    };


    //
    $scope.Reset = function () {
        $scope.btnSaveValue = "Save";
        $scope.Name = "";
        $scope.Sequence = "";
        $scope.CssClass = "";
        $scope.uiID = "";
    };
});