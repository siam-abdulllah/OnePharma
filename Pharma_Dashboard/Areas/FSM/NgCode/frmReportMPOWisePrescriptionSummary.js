app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(34);
    // $scope.GetDepot();
    //$scope.isDisabled = false;
    var index = "";
    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    firstDay = ('0' + firstDay.getDate()).slice(-2) + '-' + ('0' + (firstDay.getMonth() + 1)).slice(-2) + '-' + firstDay.getFullYear();
    var toDay = new Date();
    toDay = ('0' + toDay.getDate()).slice(-2) + '-' + ('0' + (toDay.getMonth() + 1)).slice(-2) + '-' + toDay.getFullYear();
    $scope.FromDate = firstDay;
    $scope.ToDate = toDay;

    $http({
        method: "POST",
        url: MyApp.rootPath + "Default/GetDepot"
        //params: { DepotCode: "" }
    }).then(function (response) {
        if (response.data !== '') {
            $scope.Depots = response.data;
        }
    }, function () {
        toastr.warning("Error Occurred");
    });

    $scope.GetZoneByDepot = function() {
        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetZoneByDepot",
            params: { DepotCode: $scope.frmReportMPOWisePrescriptionSummary.Depot }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Zones = response.data;
                    $scope.frmReportMPOWisePrescriptionSummary.Zone = "";
                    $scope.frmReportMPOWisePrescriptionSummary.Region = "";
                    $scope.frmReportMPOWisePrescriptionSummary.Area = "";
                    $scope.frmReportMPOWisePrescriptionSummary.Territory = "";
                    $scope.Regions = [];
                    $scope.Areas = [];
                    $scope.Territories = [];


                }
            },
            function() {
                toastr.warning("Error Occurred");
            });
    };
    $scope.GetRegionByZone = function() {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetRegionByZone",
            params: { DepotCode: $scope.frmReportMPOWisePrescriptionSummary.Depot,ZoneCode: $scope.frmReportMPOWisePrescriptionSummary.Zone }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Regions = response.data;

                    $scope.frmReportMPOWisePrescriptionSummary.Region = "";
                    $scope.frmReportMPOWisePrescriptionSummary.Area = "";
                    $scope.frmReportMPOWisePrescriptionSummary.Territory = "";
                    $scope.Areas = [];
                    $scope.Territories = [];
                }
            },
            function() {
                toastr.warning("Error Occurred");
            });
    };
    $scope.GetAreaByRegion = function() {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetAreaByRegion",
            params: { DepotCode: $scope.frmReportMPOWisePrescriptionSummary.Depot, ZoneCode: $scope.frmReportMPOWisePrescriptionSummary.Zone,RegionCode: $scope.frmReportMPOWisePrescriptionSummary.Region }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Areas = response.data;

                    $scope.frmReportMPOWisePrescriptionSummary.Area = "";
                    $scope.frmReportMPOWisePrescriptionSummary.Territory = "";
                    $scope.Territories = [];
                }
            },
            function() {
                toastr.warning("Error Occurred");
            });
    };
    $scope.GetTerritoryByArea = function() {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetTerritoryByArea",
            params: { DepotCode: $scope.frmReportMPOWisePrescriptionSummary.Depot, ZoneCode: $scope.frmReportMPOWisePrescriptionSummary.Zone, RegionCode: $scope.frmReportMPOWisePrescriptionSummary.Region,AreaCode: $scope.frmReportMPOWisePrescriptionSummary.Area }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Territories = response.data;
                    $scope.frmReportMPOWisePrescriptionSummary.Territory = "";
                }
            },
            function() {
                toastr.warning("Error Occurred");
            });
    };

    //$scope.GetHeadMenuList();
    //
    var columnMPOWisePrescriptionSummaryList = [
        { name: "SL_NO", displayName: "Sln", width: 80 },
        { name: "USER_NAME", displayName: "User Name", width: 120 },
        { name: "REGION_NAME", displayName: "Region.", width: 110 },
        { name: "AREA_NAME", displayName: "Area", width: 120 },
        { name: "TERRITORY_NAME", displayName: "Territory", width: 150 },
        { name: "NO_OF_OTHER_PRES", displayName: "Total OPD Pres", width: 150 },
        { name: "TOTAL_PRES", displayName: "Total Pres", width: 150 },
        { name: "TOTAL_PRODUCT", displayName: "Total Product", width: 150 },
        { name: "TOTAL_XELPRO", displayName: "Total Xelpro", width: 150 },
        { name: "TOTAL_XELPRO_MUPS", displayName: "Total Xelpro MUPS", width: 150 },
        { name: "TOTAL_CARDOTEL", displayName: "Total Cardotel", width: 150 },
        { name: "TOTAL_FUXTIL", displayName: "Total  Fuxtil", width: 150 },
        { name: "TOTAL_OTHERS", displayName: "Total Others Product", width: 150 }
    ];


    $scope.gridReportMPOWisePrescriptionSummary = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnMPOWisePrescriptionSummaryList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'MPOWisePrescriptionSummary.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        exporterExcelFilename: 'MPOWisePrescriptionSummary.xlsx',
        exporterExcelSheetName: 'Sheet1',
        exporterColumnScaleFactor: 4.5,
        exporterFieldApplyFilters: true,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }
    };



    $scope.GetMPOWisePrescriptionSummaryData = function () {
        $scope.gridReportMPOWisePrescriptionSummary.data = [];
        $http({
            method: "POST",
            url: MyApp.rootPath + "ReportMPOWisePrescriptionSummary/GetMPOWisePrescriptionSummaryData",
            params: { DepotCode: $scope.frmReportMPOWisePrescriptionSummary.Depot, ZoneCode: $scope.frmReportMPOWisePrescriptionSummary.Zone, RegionCode: $scope.frmReportMPOWisePrescriptionSummary.Region, AreaCode: $scope.frmReportMPOWisePrescriptionSummary.Area, TerritoryCode: $scope.frmReportMPOWisePrescriptionSummary.Territory, FromDate: $scope.FromDate, ToDate: $scope.ToDate }
        }).then(function (response) {
            if (response.data.Status === null || response.data.Status === undefined) {
                if (response.data.length > 0) {
                    $scope.gridReportMPOWisePrescriptionSummary.data = response.data;
                } else {
                    toastr.warning("No Data Found", { timeOut: 2000 });
                }
            } else {
                toastr.warning(response.data.Status, { timeOut: 2000 });
            }
        }, function (response) {
            console.log(response);
            alert("Error Loading Category");
        });
    };

    //Reset
    $scope.Reset = function () {
        $scope.frmReportMPOWisePrescriptionSummary.Depot = "";
        $scope.frmReportMPOWisePrescriptionSummary.Zone = "";
        $scope.frmReportMPOWisePrescriptionSummary.Region = "";
        $scope.frmReportMPOWisePrescriptionSummary.Area = "";
        $scope.frmReportMPOWisePrescriptionSummary.Territory = "";

        $scope.FromDate = "";
        $scope.ToDate = "";
        $scope.Regions = [];
        $scope.Areas = [];
        $scope.Territories = [];
        $scope.gridReportMPOWisePrescriptionSummary.data = [];
        $scope.FromDate = firstDay;
        $scope.ToDate = toDay;
    };
});

