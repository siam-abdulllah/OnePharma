app.controller("myCtrl", function ($scope, $http, $interval, uiGridConstants) {
    $scope.EventPerm(38);
    $scope.ACCESS_LEVEL = '';
    //function GetUserName() {

    //var username = '<%= Session["ACCESS_LEVEL"] %>';
    ///var userID = <%: Session["ACCESS_LEVEL"] %>;
    //alert(userID);
    //} 
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    var zCounter = 0;
    var aCounter = 0;
    var rCounter = 0;
    var prodCounter = 0;

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    $scope.ToDate = dd + '/' + mm + '/' + yyyy;
    $scope.FromDate = '01' + '/' + mm + '/' + yyyy;
    var dashboardData = '';
    var barLevel = [];
    var barData = [];
    var barBackgroundColor = [];

    function dynamicColors() {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgba(" + r + "," + g + "," + b + ", 0.5)";
    }
    function poolColors(a) {
        var pool = [];
        for (i = 0; i < a; i++) {
            pool.push(dynamicColors());
        }
        barBackgroundColor = pool;
        return pool;
    }


    var chartBar = AmCharts.makeChart("chartBarDiv", {
        "theme": "light",
        "type": "serial",
        "startDuration": 1,
        //"legend": {
        //    "generateFromData": false //custom property for the plugin
        //},
        //"dataProvider": [{
        //    "name": "Deflazacort",
        //    "percent": 9.7,
        //    "color": "#931c76"
        //}, {
        //    "name": "Prednisolone",
        //    "percent": 21.4,
        //    "color": "#ea3a1d"
        //}
        //],
        "valueAxes": [{
            "position": "left",
            "title": "Up to Month Sales (Lac)",
            "minimum": 0
        }],
        "graphs": [{
            "balloonText": "[[BaloonText]]: <b>[[value]] Lac</b>",
            "fillColorsField": "Color",
            "fillAlphas": 1,
            "labelText": '[[value]]',
            "lineAlpha": 0.1,
            "type": "column",
            "valueField": "Data"
        }],
        "depth3D": 20,
        "angle": 30,
        "chartCursor": {
            "categoryBalloonEnabled": false,
            "cursorAlpha": 0,
            "zoomable": false
        },
        "categoryField": "Level",
        "categoryAxis": {
            "gridPosition": "start",
            "labelRotation": 45,
            "gridCount": 5
        },
        "export": {
            "enabled": true
        }
    });
    function customAchAggregate(upToSalesValue, target) {

        //var v = $scope.ZWiseGridApi.grid.columns[7].getAggregationValue();
        //var v = $scope.AWiseGridApi.grid.columns[7].getAggregationValue();
        var result = (upToSalesValue * 100) / target;
        if (!isFinite(result)) {
            result = "total: 0";
        } else {
            result = "total: " + result;
        }
        return result;

    }
    //----------------------------------------------------------------------------
    var columnZWiseSalesAchValueList = [
        //{ name: "STOCK_DATE", displayName: "Stock Date", cellFilter: "FullDateTime", width: '13%'},
        { name: "ZONE_CODE", displayName: "Zone Code", visible: false },
        { name: "DSM_CODE", displayName: "DSM CODE", visible: false },
        { name: "SL_No", displayName: "SL. ", width: 37 },
        { name: "ZONE_NAME", displayName: "Zone Name ", width: 120 },
        { name: "DSM_NAME", displayName: "DSM Name ", width: 200 },
        { name: "TARGET_AMT", displayName: "Target", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 100, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "TO_DAY_SALES", displayName: "Today Sales", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 105, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "UPTO_SALES", displayName: "Up To Sales", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 115, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "TO_DAY_COLLECTION", displayName: " Today Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "UPTO_COLLECTION", displayName: "Up To Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        {
            name: "ACH", displayName: "Ach", width: 70, cellClass: 'grid-align', aggregationType: function () {
                var upToSalesValue = parseFloat($scope.ZWiseGridApi.grid.columns[8].getAggregationValue());
                var target = parseFloat($scope.ZWiseGridApi.grid.columns[6].getAggregationValue());
                return customAchAggregate(upToSalesValue, target);
            }
        },
        { name: "LM_UPTO_SALES", displayName: "LMUS", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "GROWTH", displayName: " Growth", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "CM_MPO", displayName: "CM MIO", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 65, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "LM_MPO", displayName: "LM MIO", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 63, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "CM_CUST", displayName: "CM Customer", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 70, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "LM_CUST", displayName: "LM Customer", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 70, cellClass: 'grid-align', footerCellFilter: 'number' }


    ];
    $scope.gridZWiseSalesAchOptionsValue = {
        //showGridFooter: true,
        showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnZWiseSalesAchValueList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Zone_Wise_Sales_Achievement_Value.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApiValue) {
            $scope.ZWiseGridApi = gridApiValue;
        }

    };
    //----------------------------------------------------------------------------
    var columnRWiseSalesAchValueList = [
        { name: "REGION_CODE", displayName: "REGION Code", visible: false },

        { name: "RSM_CODE", displayName: "RSM Code", visible: false },
        { name: "ZONE_CODE", displayName: "Zone Code", visible: false },
        { name: "DSM_CODE", displayName: "DSM CODE", visible: false },
        { name: "SL_No", displayName: "SL. ", width: 37 },
        { name: "REGION_NAME", displayName: "Region Name ", width: 120 },
        { name: "RSM_NAME", displayName: "RSM Name ", width: 200 },
        { name: "DSM_NAME", displayName: "DSM Name ", visible: false },
        { name: "TARGET_AMT", displayName: "Target", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 100, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "TO_DAY_SALES", displayName: "Today Sales", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 105, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "UPTO_SALES", displayName: "Up To Sales", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 115, cellClass: 'grid-align', footerCellFilter: 'number' },
        { name: "TO_DAY_COLLECTION", displayName: " Today Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "UPTO_COLLECTION", displayName: "Up To Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        {
            name: "ACH", displayName: "Ach", width: 70, cellFilter: 'number', cellClass: 'grid-align', aggregationType: function () {
                var upToSalesValue = parseFloat($scope.RWiseGridApi.grid.columns[11].getAggregationValue());
                var target = parseFloat($scope.RWiseGridApi.grid.columns[9].getAggregationValue());
                return customAchAggregate(upToSalesValue, target);
            }
        },
        { name: "LM_UPTO_SALES", displayName: "LMUS", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'fractionFilter' },
        { name: "GROWTH", displayName: " Growth", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'fractionFilter' },
        { name: "CM_MPO", displayName: "CM MIO", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 65, cellClass: 'grid-align', footerCellFilter: 'fractionFilter' },
        { name: "LM_MPO", displayName: "LM MIO", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 63, cellClass: 'grid-align', footerCellFilter: 'fractionFilter' },
        { name: "CM_CUST", displayName: "CM Customer", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 70, cellClass: 'grid-align', footerCellFilter: 'fractionFilter' },
        { name: "LM_CUST", displayName: "LM Customer", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 70, cellClass: 'grid-align', footerCellFilter: 'fractionFilter' }
    ];
    $scope.gridRWiseSalesAchOptionsValue = {
        //showGridFooter: true,
        showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnRWiseSalesAchValueList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'RSM_Wise_Sales_Achievement_Value.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApiValue) {
            $scope.RWiseGridApi = gridApiValue;
        }

    };

    //----------------------------------------------------------------------------

    var columnProdWiseSalesAchList = [
        { name: "SL_No", displayName: "SL. ", width: 37 },
        { name: "PRODUCT_CODE", displayName: "Product Code", visible: false },
        { name: "PRODUCT_NAME", displayName: "Product", width: 145 },
        { name: "PACK_SIZE", displayName: "Pack Size", width: 80 },
        { name: "TO_DAY_SALES_BOX", displayName: "To Day Sales (Box)", cellFilter: 'number', width: 125, cellClass: 'grid-align' },
        { name: "TO_DAY_SALES_VALUE", displayName: "To Day Sales (Value)", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, cellClass: 'grid-align', footerCellClass: 'grid-align', width: 140, footerCellFilter: 'number' },
        { name: "CM_UPTO_SALES_BOX", displayName: "UpTo Sales (Box)", cellFilter: 'number', cellClass: 'grid-align', width: 118 },
        { name: "CM_UPTO_SALES_VALUE", displayName: "UpTo Sales (Value)", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, cellClass: 'grid-align', footerCellClass: 'grid-align', width: 118, footerCellFilter: 'number' },
        { name: "LM_UPTO_SALES_BOX", displayName: "LMUS (Box)", cellFilter: 'number', cellClass: 'grid-align', width: 115 },
        { name: "LM_UPTO_SALES_VALUE", displayName: "LMUS (Value)", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, cellClass: 'grid-align', footerCellClass: 'grid-align', width: 115, footerCellFilter: 'number' },
        { name: "GROWTH_BOX", displayName: "Growth (Box)", cellFilter: 'number', cellClass: 'grid-align', width: 100 },
        { name: "GROWTH_VALUE", displayName: "Growth (Value)", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, cellClass: 'grid-align', footerCellClass: 'grid-align', width: 102, footerCellFilter: 'number' }
    ];
    $scope.gridProdWiseSalesAchOptionsValue = {
        //showGridFooter: true,
        showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnProdWiseSalesAchList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Sales_Achievement_Value.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApiValue) {
            $scope.ProdGridApi = gridApiValue;
        }
    };
    //----------------------------------------------------------------------------
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
        { name: "ZONE_NAME", displayName: "ZONE Name ", visible: false },
        { name: "REGION_NAME", displayName: "REGION Name ", visible: false },
        { name: "DEPOT_NAME", displayName: "DEPOT Name ", visible: false },
        { name: "RSM_NAME", displayName: "RSM Name ", visible: false },
        { name: "AREA_NAME", displayName: "Area Name ", width: 110 },
        { name: "AM_NAME", displayName: "AM Name ", width: 150 },
        //{ name: "ZONE_NAME", displayName: "Zone Name ", width: 120},
        { name: "DSM_NAME", displayName: "DSM Name ", visible: false },
        { name: "TARGET_AMT", displayName: "Target", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 120, cellClass: 'grid-align' },

        { name: "TO_DAY_SALES", displayName: "Today Sales", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "UPTO_SALES", displayName: "Up To Sales", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 120, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "TO_DAY_COLLECTION", displayName: " Today Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "UPTO_COLLECTION", displayName: "Up To Collection", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        {
            name: "ACH", displayName: "Achievement", width: 100, cellClass: 'grid-align', aggregationType: function () {
                var upToSalesValue = parseFloat($scope.gridApi.grid.columns[18].getAggregationValue());
                var target = parseFloat($scope.gridApi.grid.columns[16].getAggregationValue());
                return customAchAggregate(upToSalesValue, target);
            }
        },
        { name: "LM_UPTO_SALES", displayName: "LMUS", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "GROWTH", displayName: " Growth", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 110, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "CM_MPO", displayName: "CM MIO", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "LM_MPO", displayName: "LM MIO", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 80, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "CM_CUST", displayName: "CM Customer", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 90, cellClass: 'grid-align', footerCellFilter: 'number:2' },
        { name: "LM_CUST", displayName: "LM Customer", cellFilter: 'number:2', aggregationType: uiGridConstants.aggregationTypes.sum, width: 90, cellClass: 'grid-align', footerCellFilter: 'number:2' }
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
        exporterCsvFilename: 'Area_Manager_Wise_Sales_Achievement_Human.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }

    };

    $scope.GetAMWiseSalesValueAchievement = function () {
        $scope.isDisabled = true;
        $scope.FromDate = $scope.ToDate;
        if (zCounter === 0) {
            $http({
                method: "POST",
                url: MyApp.rootPath + "AMWiseSalesAchievement/GetAMWiseSalesValueAchievement",
                data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
            }).then(function (response) {
                if (response.data.Status === 'Ok') {
                    $scope.gridAWiseSalesAchOptionsValue.data = response.data.Data;
                    aCounter = 1;

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
        }
    };

    //----------------------------------------------------------------------------

    $scope.GetDashboardData = function () {
        $http({
            method: "post",
            url: MyApp.rootPath + "DashboardAnimal/GetDashboardData",
            data: { toDate: $scope.ToDate },
            datatype: "json"
        }).then(function (response) {
            if (response.data.Status === 'Ok') {
                $scope.dashboardData = response.data.Data;
                $scope.dashboardData.Total_Stock_Valuation = ((parseFloat(response.data.Data.Commercial_Stock_Valuation) || 0) + (parseFloat(response.data.Data.Sample_Stock_Valuation) || 0) + (parseFloat(response.data.Data.PPM_Stock_Valuation) || 0) + (parseFloat(response.data.Data.Gift_Stock_Valuation) || 0)).toFixed(2);
                if (response.data.Data.Target.Target_Current_Month === null ||
                    response.data.Data.Target.Target_Current_Month === "") {
                    $scope.dashboardData.Achievement = "";
                } else {
                    $scope.dashboardData.Achievement = (((parseFloat(response.data.Data.UpToMonthSale.UpTo_Month_Total_Sales) || 0) * 100) / (parseFloat(response.data.Data.Target.Target_Current_Month) || 0)).toFixed(2);

                }
                //$scope.dashboardData.Growth = ((parseFloat(response.data.Data.UpToMonthSale.UpTo_Month_Total_Sales) || 0) - (parseFloat(response.data.Data.LMUpToDate.LM_UP_ToDate_Total_Sales) || 0)).toFixed(2);
                $scope.dashboardData.Growth =
                    ((parseFloat(response.data.Data.UpToMonthSale.UpTo_Month_Total_Sales)) -
                        (parseFloat(response.data.Data.LMUpToDate.LM_UP_ToDate_Total_Sales))).toFixed(2);
                $scope.dashboardData.CollectionGrowth =
                    (((parseFloat(response.data.Data.UpTo_Month_Collection)) - (parseFloat(response.data.Data.LM_UpTo_Month_Collection))) / (parseFloat(response.data.Data.LM_UpTo_Month_Collection) * 100)).toFixed(2);
                var a = parseFloat(response.data.Data.UpTo_Month_Collection) - parseFloat(response.data.Data.LM_UpTo_Month_Collection);
                var b = parseFloat(response.data.Data.LM_UpTo_Month_Collection) * 100;
                $scope.dashboardData.CollectionGrowth = (a / b).toFixed(0);
                //$scope.dashboardData.Total_Dues = ((parseFloat(response.data.Data.MaturedDue.Matured_Dues) || 0) + (parseFloat(response.data.Data.ImmaturedDue.Immatured_Dues) || 0)).toFixed(2);

                //$scope.dashboardData.Total_Dues_CA = ((parseFloat(response.data.Data.MaturedDue.Matured_Dues_CA) || 0) + (parseFloat(response.data.Data.ImmaturedDue.Immatured_Dues_CA) || 0)).toFixed(2);

                //$scope.dashboardData.Total_Dues_CR = ((parseFloat(response.data.Data.MaturedDue.Matured_Dues_CR) || 0) + (parseFloat(response.data.Data.ImmaturedDue.Immatured_Dues_CR) || 0)).toFixed(2);

                //if (response.data.Data.ACCESS_LEVEL === "National" || response.data.Data.ACCESS_LEVEL === "Z" || response.data.Data.ACCESS_LEVEL === "R") {
                //    $scope.ACCESS_LEVEL = response.data.Data.ACCESS_LEVEL;

                //var today = new Date();
                //var dd = today.getDate();
                //var mm = today.getMonth() + 1; //January is 0!
                //var yyyy = today.getFullYear();

                //if (dd < 10) {
                //    dd = '0' + dd;
                //}

                //if (mm < 10) {
                //    mm = '0' + mm;
                //}

                //$scope.ToDate = dd + '/' + mm + '/' + yyyy;
                //$scope.FromDate = '01' + '/' + mm + '/' + yyyy;

                //$http({
                //    method: "POST",
                //    url: MyApp.rootPath + "ZoneWiseSalesAchievement/GetZoneWiseSalesValueAchievement",
                //    data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
                //}).then(function (response) {
                //    if (response.data.Status == 'Ok') {
                //        $scope.gridZWiseSalesAchOptionsValue.data = response.data.Data;
                //    }
                //});

                //$http({
                //    method: "POST",
                //    url: MyApp.rootPath + "RSMWiseSalesAchievement/GetRSMWiseSalesValueAchievement",
                //    data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
                //}).then(function (response) {
                //    if (response.data.Status == 'Ok') {
                //        $scope.gridRWiseSalesAchOptionsValue.data = response.data.Data;
                //    }
                //    });

                //$http({
                //    method: "POST",
                //    url: MyApp.rootPath + "ProdWiseSalesAchievement/GetProdWiseSalesAchievement",
                //    data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
                //}).then(function (response) {
                //    if (response.data.Status == 'Ok') {
                //        $scope.gridProdWiseSalesAchOptionsValue.data = response.data.Data;
                //    }
                //});
                //}
                // else {
                $scope.ACCESS_LEVEL = response.data.Data.ACCESS_LEVEL;
                //     alert($scope.ACCESS_LEVEL);
                //}
                chartBar.dataProvider = response.data.BarChartData;
                var date = new Date();
                $scope.CurrentMonthName = date.toLocaleString('en-us', { month: 'long' });
                //$('#homeBarChart_modal').modal('show');


            } else {
                toastr.error("No Data Found!", '');
            }
        });
    };
    $scope.GetDashboardData();
    $scope.GetZWiseSalesAchValue = function () {
        $scope.FromDate = $scope.ToDate;
        if (zCounter === 0) {
            $http({
                method: "POST",
                url: MyApp.rootPath + "ZoneWiseSalesAchievementAnimal/GetZoneWiseSalesValueAchievementAnimal",
                data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
            }).then(function (response) {
                if (response.data.Status === 'Ok') {
                    $scope.gridZWiseSalesAchOptionsValue.data = response.data.Data;
                    zCounter = 1;
                }
            });
        }
    };
    $scope.GetRWiseSalesAchValue = function () {
        $scope.FromDate = $scope.ToDate;
        if (rCounter === 0) {
            $http({
                method: "POST",
                url: MyApp.rootPath + "RSMWiseSalesAchievementAnimal/GetRSMWiseSalesValueAchievementAnimal",
                data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
            }).then(function (response) {
                if (response.data.Status === 'Ok') {
                    $scope.gridRWiseSalesAchOptionsValue.data = response.data.Data;
                    rCounter = 1;
                }
            });
        }
    };
    $scope.GetProdWiseSalesAchValue = function () {
        $scope.FromDate = $scope.ToDate;
        if (prodCounter === 0) {
            $http({
                method: "POST",
                url: MyApp.rootPath + "ProdWiseSalesAchievementAnimal/GetProdWiseSalesAchievementAnimal",
                data: { fromDate: $scope.FromDate, toDate: $scope.ToDate }
            }).then(function (response) {
                if (response.data.Status === 'Ok') {
                    $scope.gridProdWiseSalesAchOptionsValue.data = response.data.Data;
                    prodCounter = 1;
                }
            });
        }
    };

    $('#homeBarChart_modal').on('shown.bs.modal', function (event) {
        chartBar.startEffect = 'easeInSine';
        chartBar.startDuration = 1;
        chartBar.animateAgain();
    });
    $('#chartButton').click(function () {
        $('#homeBarChart_modal').modal('show');
    });
    //------------------------------------------------------------------------------------
});