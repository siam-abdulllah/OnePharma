app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(24);
    $scope.isDisabled = false;

    //$scope.GetHeadMenuList();
    //
    var columnNCurrentStockList = [
        //{ name: "STOCK_DATE", displayName: "Stock Date", cellFilter: "FullDateTime", width: '13%'},
        { name: "SL_No", displayName: "SL.", width: 80 },
        { name: "DEPOT_CODE", displayName: "Depot Code", visible: false },
        { name: "DEPOT_NAME", displayName: "Depot ", width: 250 },
        { name: "PPM_CODE", displayName: "PPM Code", width: 110 },
        { name: "PPM_NAME", displayName: "PPM Name", width: 305 },
        { name: "PPM_TYPE", displayName: "PPM Type", visible: false  },
        { name: "PPM_TYPE_NAME", displayName: "PPM Type Name", width: 100 },
        { name: "PACK_SIZE", displayName: "Pack Size", width: 100 },
        { name: "STOCK_QTY", displayName: "Stock QTY", width: 250, cellFilter: 'number:2',cellClass: 'grid-align' },
    ];

    $scope.gridNCurrentStockOptions = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnNCurrentStockList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Depot_Comm_Current_Stock.csv',
        exporterMenuPdf: false,
        //exporterPdfDefaultStyle: { fontSize: 9, overflow: 'wordbreak' },
        //exporterPdfTableStyle: { margin: [0, 0, 0, 0] },
        //exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: false, color: 'black' },
        //exporterPdfHeader: { text: "Brand Name Approval Report", style: 'headerStyle' },
        //exporterPdfFooter: function (currentPage, pageCount) {
        //    return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        //},
        //exporterPdfCustomFormatter: function (docDefinition) {
        //    docDefinition.styles.headerStyle = { fontSize: 22, bold: true, alignment: 'center' };
        //    docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
        //    return docDefinition;
        //},
        //exporterFieldCallback: function (grid, row, col, input) {
        //    if (col.cellFilter) { // check if any filter is applied on the column
        //        var filters = col.cellFilter.split('|'); // get all the filters applied
        //        angular.forEach(filters,
        //            function (filter) {
        //                var filterName = filter.split(':')[0]; // fetch filter name
        //                var filterParams = filter.split(':').splice(1); //fetch all the filter parameters
        //                filterParams.unshift(input); // insert the input element to the filter parameters list
        //                var filterFn = $filter(filterName); // filter function
        //                // call the filter, with multiple filter parameters.
        //                //'Apply' will call the function and pass the array elements as individual parameters to that function.
        //                input = filterFn.apply(this, filterParams);
        //            });
        //        return input;
        //    }
        //    else
        //        return input;
        //},

        //exporterPdfOrientation: 'landscape',
        //exporterPdfPageSize: 'a4',
        //exporterPdfMaxGridWidth: 680,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }

    };
    //$scope.getFooterValue = function () {
    //    console.log($scope.gridApi);
    //    alert($scope.gridApi.grid.columns[2].getAggregationValue());
    //}
   
    //
    $scope.GetDashboardData = function () {
        $http({
            method: "post",
            url: MyApp.rootPath + "DepotPPMCurrentStock/GetDashboardData",
            datatype: "json",
        }).then(function (response) {
            if (response.data.Status == 'Ok') {
                $scope.dashboardData = response.data.Data;
                $scope.dashboardData.Total_Stock_Valuation = ((parseFloat(response.data.Data.Commercial_Stock_Valuation) || 0) + (parseFloat(response.data.Data.Sample_Stock_Valuation) || 0)).toFixed(2);

            }
            else {
                toastr.warning("No Data Found!");
            }
        });
    };
    $scope.GetDashboardData();

    $scope.GetDepotPPMCurrentStock = function () {
        $scope.isDisabled = true;
        if (!DateCheck($scope.NationalDate, 'null')) {
            $scope.isDisabled = false;
            return false;
        }
        $http({
            method: "POST",
            url: MyApp.rootPath + "DepotPPMCurrentStock/GetDepotPPMCurrentStock",
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
            toastr.error("Error!");
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
//Date picker
