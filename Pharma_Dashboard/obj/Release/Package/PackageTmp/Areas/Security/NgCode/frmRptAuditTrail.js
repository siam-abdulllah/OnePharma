app.controller("myCtrl", function ($scope, $http, $filter) {
    $scope.EventPerm(25);
    //$scope.GetEmployeeList = function () {
    //    $http({
    //        method: "GET",
    //        url: MyApp.rootPath + "EmployeeInfo/GetActiveEmployeeInfoList"
    //    }).then(function (response) {
    //        if (response.data != "") {
    //            $scope.Employees = response.data;
    //        } else {
    //            toastr.warning("No Data Found!");
    //        }
    //    }, function (response) {
    //        toastr.warning("Error Occurred!");
    //    });
    //};

    //$scope.GetEmployeeList();
    $scope.GetEmployeeList = function () {
        $http({
            method: "GET",
            url: MyApp.rootPath + "RoleConf/GetActiveEmployeeInfoList"
        }).then(function (response) {
            if (response.data !== "") {
                $scope.Employees = response.data;
            } else {
                toastr.warning("No Data Found!");
            }
        }, function () {
            toastr.warning("Error Occurred!");
        });
    };

    $scope.GetEmployeeList();

    $scope.GetAuditTrail = function () {
        $http({
            method: "POST",
            url: MyApp.rootPath + "AuditTrail/GetAuditTrail",
            data: { FromDate: $scope.FromDate, ToDate: $scope.ToDate, Action_By: $scope.frmRptAuditTrail.EmployeeCode, Action_Type: $scope.ActionType }
        }).then(function (response) {
            if (response.data !== "") {
                $scope.gridAuditTrailOptions.data = response.data;
            } else {
                toastr.warning("No Data Found!");
            }
        }, function (response) {
            toastr.warning("Error Occurred!");
        });
    }


    var columnAuditTrail = [
        { name: 'EmployeeName', displayName: "Action By", width: "250" },
        //{ name: 'ProposalID', displayName: "ProposalID", visible: false },
       // { name: 'DepartmentName', displayName: "Department", width: "200" },
        { name: 'DesignationName', displayName: "Designation", width: "250" },
        { name: 'Terminal', displayName: "Terminal", width: "200" },
        { name: 'Action_Date', displayName: "Action Date", width: "150" },
        { name: 'Activity_Type', displayName: "Action Type", width: "150"},
        { name: 'Action_Form', displayName: "Form", width: "230" },
        { name: 'Action_Table', displayName: "Action table", width: "120", visible: false },
        { name: 'Transaction_ID', displayName: "Tracking No", visible: false }

    ];

    $scope.gridAuditTrailOptions = {
        enableFiltering: true,
        enableSorting: true,
        enableColumnResizing: true,
        paginationPageSizes: [10, 50, 100],
        paginationPageSize: 10,
        columnDefs: columnAuditTrail,

        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Audit_Trail.csv',
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterPdfTableStyle: { margin: [0, 10, 10, 0] },
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: false, color: 'black' },
        exporterPdfHeader: { text: "Audit Trail Report", style: 'headerStyle' },
        exporterPdfFooter: function (currentPage, pageCount) {
            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        },
        exporterPdfCustomFormatter: function (docDefinition) {
            docDefinition.styles.headerStyle = { fontSize: 22, bold: true, alignment: 'center' };
            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
            return docDefinition;
        },
        exporterFieldCallback: function (grid, row, col, input) {
            if (col.cellFilter) { // check if any filter is applied on the column
                var filters = col.cellFilter.split('|'); // get all the filters applied
                angular.forEach(filters,
                    function (filter) {
                        var filterName = filter.split(':')[0]; // fetch filter name
                        var filterParams = filter.split(':').splice(1); //fetch all the filter parameters
                        filterParams.unshift(input); // insert the input element to the filter parameters list
                        var filterFn = $filter(filterName); // filter function
                        // call the filter, with multiple filter parameters.
                        //'Apply' will call the function and pass the array elements as individual parameters to that function.
                        input = filterFn.apply(this, filterParams);
                    });
                return input;
            }
            else
                return input;
        },

        exporterPdfOrientation: 'portrait',
        exporterPdfPageSize: 'A4',
        exporterPdfMaxGridWidth: 470,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
    };

    $scope.Reset = function () {
        $scope.FromDate = "";
        $scope.ToDate = "";
        $scope.ActionType = "";

        $scope.gridAuditTrailOptions.data = [];
    };
});