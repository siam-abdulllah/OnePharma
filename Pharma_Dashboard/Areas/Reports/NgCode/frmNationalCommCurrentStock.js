app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(17);
    $scope.isDisabled = false;
    var columnNCurrentStockList = [
    { name: "SL_No", displayName: "SL.", width: 40},
    { name: "PRODUCT_CODE",displayName: "Product Code", width: 105},
    { name: "PRODUCT_NAME", displayName: "Product Name", width: 105},
    { name: "PACK_SIZE", displayName: "Pack Size", width: 80},
    { name: "UNIT_TP", displayName: "Unit Tp", width: 70},
    { name: "UNIT_VAT", displayName: "Unit vat", width: 70},
    { name: "FRESH_STOCK_QTY", displayName: "Stock QTY (Box)", width: 170, cellFilter: 'number:2' },
    { name: "DAMAGE_STOCK_QTY", displayName: "Damage Stock QTY", width: 160, cellFilter: 'number:2' },
    { name: "FRESH_STOCK_TP_VAL", displayName: "TP Value", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 150, footerCellFilter: 'number:2'},
    { name: "FRESH_STOCK_VAT_VAL", displayName: "VAT Value", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 150, footerCellFilter: 'number:2'},
    { name: "FRESH_STOCK_TP_VAT_VAL", displayName: "TP+ VAT Value", cellFilter: 'number:2' , aggregationType: uiGridConstants.aggregationTypes.sum, width: 150, footerCellFilter: 'number:2'}
    




    ];
    $scope.gridNCurrentStockOptions = {
        //showGridFooter: true,
        showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnNCurrentStockList,
        rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'National_Comm_Current_Stock.csv',
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
                    (parseFloat(response.data.Data.Sample_Stock_Valuation) || 0)).toFixed(2);

            } else {
                toastr.warning("No Data Found!");
            }
        });
    };
    $scope.GetDashboardData();
    $scope.GetNationalCommCurrentStock = function () {
        $scope.isDisabled = true;
        if (!DateCheck($scope.NationalDate, 'null')) {
            $scope.isDisabled = false;
            return false;
        }
        $http({
            method: "POST",
            url: MyApp.rootPath + "NationalCommCurrentStock/GetNationalCommCurrentStock",
            data: { dateParam: $scope.NationalDate}
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

