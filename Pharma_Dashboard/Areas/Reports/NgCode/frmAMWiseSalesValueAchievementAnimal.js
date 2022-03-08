app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(41);
    $scope.isDisabled = false;


    //$scope.GetHeadMenuList();
    //
    var columnAWiseSalesAchValueList = [
        //{ name: "STOCK_DATE", displayName: "Stock Date", cellFilter: "FullDateTime", width: '13%'},
        { name: "DEPOT_CODE", displayName: "DEPOT Code", visible: false },
        { name: "DSM_CODE", displayName: "DSM CODE", visible: false },
        { name: "ZONE_CODE", displayName: "Zone Code", visible: false },
        { name: "RSM_CODE", displayName: "RSM Code", visible: false },
        { name: "REGION_CODE", displayName: "REGION Code", visible: false },
        { name: "AM_CODE", displayName: "AM Code", visible: false },
        { name: "AREA_CODE", displayName: "AREA Code", visible: false },
        { name: "SL_No", displayName: "SL. ", width: 40 },
        { name: "ZONE_NAME", displayName: "ZONE Name ", visible: false},
        { name: "REGION_NAME", displayName: "REGION Name ", visible: false},
        { name: "DEPOT_NAME", displayName: "DEPOT Name ", visible: false},
        { name: "RSM_NAME", displayName: "RSM Name ", visible: false},
        { name: "AREA_NAME", displayName: "Area Name ", width: 110},
        { name: "AM_NAME", displayName: "AM Name ", width: 150},
        //{ name: "ZONE_NAME", displayName: "Zone Name ", width: 120},
        { name: "DSM_NAME", displayName: "DSM Name ", visible: false },
        { name: "TARGET_AMT", displayName: "Target", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 120, cellClass: 'grid-align'},
        
        { name: "TO_DAY_SALES", displayName: "Today Sales", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "UPTO_SALES", displayName: "Up To Sales", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 120, cellClass: 'grid-align', footerCellFilter: 'number:2'},
        { name: "TO_DAY_COLLECTION", displayName: " Today Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "UPTO_COLLECTION", displayName: "Up To Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        {
            name: "ACH", displayName: "Achievement", width: 100, cellClass: 'grid-align', aggregationType: function () {
                var upToSalesValue = parseFloat($scope.gridApi.grid.columns[18].getAggregationValue());
                var target = parseFloat($scope.gridApi.grid.columns[16].getAggregationValue());
                return customAchAggregate(upToSalesValue, target);
            }},
        { name: "LM_UPTO_SALES", displayName: "LMUS", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2'},
        { name: "GROWTH", displayName: " Growth", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2'},
        { name: "CM_MPO", displayName: "CM MIO", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2'},
        { name: "LM_MPO", displayName: "LM MIO", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2'},
        { name: "CM_CUST", displayName: "CM Customer", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 90, cellClass: 'grid-align', footerCellFilter: 'number:2'},
        { name: "LM_CUST", displayName: "LM Customer", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 90, cellClass: 'grid-align', footerCellFilter: 'number:2'}
        //{ name: "FRESH_STOCK_TP_VAL", displayName: "TP Value", aggregationType: uiGridConstants.aggregationTypes.sum, width: 130 },
        //{ name: "FRESH_STOCK_VAT_VAL", displayName: "VAT Value", aggregationType: uiGridConstants.aggregationTypes.sum, width: 130 },
        //{ name: "FRESH_STOCK_TP_VAT_VAL", displayName: "TP+ VAT Value", aggregationType: uiGridConstants.aggregationTypes.sum, width: 130 }

    ];

    $scope.gridAWiseSalesAchOptionsValue = {
        //showGridFooter: true,
        showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnAWiseSalesAchValueList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Area_Manager_Wise_Sales_Achievement_Animal.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }

    };

    function customAchAggregate(upToSalesValue, target){
    var result = (upToSalesValue * 100) / target;
    if (!isFinite(result)) {
       result = "total: 0.00";
    } else {
       result = "total: " + result.toFixed(2);
    }
    return result;

    }
    
    $scope.GetAMWiseSalesValueAchievementAnimal = function () {
        $scope.isDisabled = true;
        if (!DateCheck($scope.FromDate, $scope.ToDate)) {
            $scope.isDisabled = false;
            return false;
        }
        $http({
            method: "POST",
            url: MyApp.rootPath + "AMWiseSalesAchievementAnimal/GetAMWiseSalesValueAchievementAnimal",
            data: { fromDate: $scope.FromDate, toDate:$scope.ToDate}
        }).then(function (response) {
            if (response.data.Status === 'Ok') {
                $scope.gridAWiseSalesAchOptionsValue.data = response.data.Data;
                

            }
            else {
                toastr.warning("No Data Found!", '');
                $scope.gridAWiseSalesAchOptionsValue.data = [];
            }
            $scope.isDisabled = false;

            }, function (response) {
            toastr.error("Error!");
            $scope.isDisabled = false;

        });
    };


    //
    $scope.Reset = function () {
        $scope.FromDate = "";
        $scope.ToDate = "";
        $scope.gridAWiseSalesAchOptionsValue.data = [];
        $scope.isDisabled = false;

    };
});
//Date picker
