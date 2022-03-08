app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(23);
    $scope.isDisabled = false;
    var columnNCurrentStockList = [
        { name: "SL_No", displayName: "SL.", width: 80 },
        //{ name: "DEPOT_CODE", displayName: "Depot Code", visible: false },
        //{ name: "DEPOT_NAME", displayName: "Depot ", width: 250 },
        { name: "PPM_CODE", displayName: "PPM Code", width: 150 },
        { name: "PPM_NAME", displayName: "PPM Name", width: 305 },
        { name: "PPM_TYPE", displayName: "PPM Type", visible: false },
        { name: "PPM_TYPE_NAME", displayName: "PPM Type Name", width: 150 },
        { name: "PACK_SIZE", displayName: "Pack Size", width: 200 },
        { name: "STOCK_QTY", displayName: "Stock QTY", width: 270, cellFilter: 'number:2', cellClass: 'grid-align' },
        //{ name: "DAMAGE_STOCK_QTY", displayName: "Damage Stock QTY", width: 160, cellFilter: 'number:2' },
        //{ name: "FRESH_STOCK_TP_VAL", displayName: "TP Value", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 150, footerCellFilter: 'number:2' },
        //{ name: "FRESH_STOCK_VAT_VAL", displayName: "VAT Value", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 150, footerCellFilter: 'number:2' },
        //{ name: "FRESH_STOCK_TP_VAT_VAL", displayName: "TP+ VAT Value", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 150, footerCellFilter: 'number:2' }





    ];
    $scope.gridNCurrentStockOptions = {
        //showGridFooter: true,
       // showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnNCurrentStockList,
        rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'National_PPM_Current_Stock.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
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


    };
    $scope.GetDashboardData = function () {
        $http({
            method: "post",
            url: MyApp.rootPath + "DepotCommCurrentStock/GetDashboardData",
            datatype: "json",
        }).then(function (response) {
            if (response.data.Status == 'Ok') {
                $scope.dashboardData = response.data.Data;
                $scope.dashboardData.Total_Stock_Valuation =
                    ((parseFloat(response.data.Data.Commercial_Stock_Valuation) || 0) +
                        (parseFloat(response.data.Data.Sample_Stock_Valuation) || 0) +
                        (parseFloat(response.data.Data.PPM_Stock_Valuation) || 0) +
                        (parseFloat(response.data.Data.Gift_Stock_Valuation) || 0)).toFixed(2);

            } else {
                toastr.warning("No Data Found!");
            }
        });
    };
    $scope.GetDashboardData();
    $scope.GetNationalPPMCurrentStock = function () {
        $scope.isDisabled = true;
        if (!DateCheck($scope.NationalDate, 'null')) {
            $scope.isDisabled = false;
            return false;
        }
        $http({
            method: "POST",
            url: MyApp.rootPath + "NationalPPMCurrentStock/GetNationalPPMCurrentStock",
            data: { dateParam: $scope.NationalDate }
        }).then(function (response) {
            if (response.data.Status == 'Ok') {
                $scope.gridNCurrentStockOptions.data = response.data.Data;

            }
            else {
                toastr.warning("No Data Found!", '');
                $scope.gridNCurrentStockOptions.data = [];
            }
            $scope.isDisabled = false;
        }, function () {
            alert("Error!");
            $scope.isDisabled = false;
        });
    };


    //
    $scope.Reset = function () {
        $scope.NationalDate = "";
        $scope.gridNCurrentStockOptions.data = [];
        $scope.isDisabled = false;
    };
});

