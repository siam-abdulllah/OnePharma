app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    //$scope.EventPerm(18);


    
    var columnZWiseSalesAchProductList = [
        //{ name: "STOCK_DATE", displayName: "Stock Date", cellFilter: "FullDateTime", width: '13%'},
        { name: "DEPOT_CODE", displayName: "Product Code", visible: false },
        { name: "SL_No", displayName: "SL. ", width: 40 },
        { name: "DEPOT_NAME", displayName: "Product Name ", width: 250},
        { name: "PRODUCT_CODE", displayName: "Pack Size", width: 150},
        { name: "PRODUCT_NAME", displayName: "Today Sales (Box)", width: 105},
        { name: "PRODUCT_NAME", displayName: "Today Sales ", width: 105},
        { name: "PACK_SIZE", displayName: "Up To Sales (Box)", width: 130},
        { name: "PACK_SIZE", displayName: "Up To Sales", width: 130},
        //{ name: "UNIT_TP", displayName: "Achievement", width: 120},
        { name: "UNIT_VAT", displayName: "LMSD (Box)", width: 70},
        { name: "UNIT_VAT", displayName: "LMUS", width: 70},
        { name: "FRESH_STOCK_QTY", displayName: " Growth", width: 170},
        { name: "DAMAGE_STOCK_QTY", displayName: "Opening MPO", width: 160}
        //{ name: "FRESH_STOCK_TP_VAL", displayName: "TP Value", aggregationType: uiGridConstants.aggregationTypes.sum, width: 130 },
        //{ name: "FRESH_STOCK_VAT_VAL", displayName: "VAT Value", aggregationType: uiGridConstants.aggregationTypes.sum, width: 130 },
        //{ name: "FRESH_STOCK_TP_VAT_VAL", displayName: "TP+ VAT Value", aggregationType: uiGridConstants.aggregationTypes.sum, width: 130 }

    ];

    $scope.gridZWiseSalesAchOptionsProduct = {
        //showGridFooter: true,
        showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnZWiseSalesAchProductList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Zone_Wise_Sales_Achievement_Product.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApiProduct) {
            $scope.gridApi = gridApiProduct;
        }

    };
    //$scope.getFooterValue = function () {
    //    console.log($scope.gridApi);
    //    alert($scope.gridApi.grid.columns[2].getAggregationValue());
    //}
   

   
    $scope.GetDashboardData = function () {
        $http({
            method: "post",
            url: MyApp.rootPath + "ZoneWiseSalesAchievement/GetDashboardData",
            datatype: "json",
        }).then(function (response) {
            if (response.data.Status == 'Ok') {
                $scope.dashboardData = response.data.Data;
                $scope.dashboardData.Total_Stock_Valuation = ((parseFloat(response.data.Data.Commercial_Stock_Valuation) || 0) + (parseFloat(response.data.Data.Sample_Stock_Valuation) || 0) + (parseFloat(response.data.Data.PPM_Stock_Valuation) || 0) + (parseFloat(response.data.Data.Gift_Stock_Valuation) || 0)).toFixed(2);

            }
            else {
                toastr.warning("No Data Found!");
            }
        });
    };
    $scope.GetDashboardData();

    $scope.GetZoneWiseSalesProductAchievement = function () {
        $http({
            method: "POST",
            url: MyApp.rootPath + "ZoneWiseSalesAchievement/GetZoneWiseSalesProductAchievement",
            data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
        }).then(function (response) {
            if (response.data.Status == 'Ok') {
                $scope.gridZWiseSalesAchOptions.data = response.data.Data;

            }
            else {
                toastr.warning("No Data Found!", '');
                $scope.gridZWiseSalesAchOptions.data = [];
            }
        }, function () {
            toastr.error("Error!");
        });
    };


    //
    $scope.Reset = function () {
        $scope.FromDate = "";
        $scope.ToDate = "";
        $scope.gridZWiseSalesAchOptionsProduct.data = [];
    };
});
//Date picker
