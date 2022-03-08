app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(35);
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

    $scope.GetZoneByDepot = function () {
        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetZoneByDepot",
            params: { DepotCode: $scope.frmReportQualifiedMPO.Depot }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Zones = response.data;
                $scope.frmReportQualifiedMPO.Zone = "";
                $scope.frmReportQualifiedMPO.Region = "";
                $scope.frmReportQualifiedMPO.Area = "";
                $scope.frmReportQualifiedMPO.Territory = "";
                $scope.Regions = [];
                $scope.Areas = [];
                $scope.Territories = [];


            }
        },
            function () {
                toastr.warning("Error Occurred");
            });
    }
    $scope.GetRegionByZone = function () {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetRegionByZone",
            params: { ZoneCode: $scope.frmReportQualifiedMPO.Zone }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Regions = response.data;

                $scope.frmReportQualifiedMPO.Region = "";
                $scope.frmReportQualifiedMPO.Area = "";
                $scope.frmReportQualifiedMPO.Territory = "";
                $scope.Areas = [];
                $scope.Territories = [];
            }
        }, function () {
            toastr.warning("Error Occurred");
        });
    }
    $scope.GetAreaByRegion = function () {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetAreaByRegion",
            params: { RegionCode: $scope.frmReportQualifiedMPO.Region }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Areas = response.data;

                $scope.frmReportQualifiedMPO.Area = "";
                $scope.frmReportQualifiedMPO.Territory = "";
                $scope.Territories = [];
            }
        }, function () {
            toastr.warning("Error Occurred");
        });
    }
    $scope.GetTerritoryByArea = function () {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetTerritoryByArea",
            params: { AreaCode: $scope.frmReportQualifiedMPO.Area }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Territories = response.data;
                $scope.frmReportQualifiedMPO.Territory = "";
            }
        }, function () {
            toastr.warning("Error Occurred");
        });
    }

    //$scope.GetHeadMenuList();
    //
    var columnReportQualifiedMPOList = [
        { name: "SL_NO", displayName: "Sln", width: 50, pinnedLeft: true },
    //{ name: "MST_SL", displayName: "ID", width: 180 },
    //{ name: "USER_ID", displayName: "MPO Code", width: 120 },
    { name: "USER_NAME", displayName: "User Name", width: 120, pinnedLeft: true },
    { name: "REGION_NAME", displayName: "Region", width: 90, enablePinning: true, hidePinLeft: false, hidePinRight: true },
    { name: "DEPOT_NAME", displayName: "Depot", width: 90 },
    { name: "AREA_NAME", displayName: "Area", width: 90 },
    { name: "TERRITORY_NAME", displayName: "Territory", width: 90 },
    { name: "TOTAL_XELPRO", displayName: "Xelpro", width: 80, pinnedRight: true },
    { name: "TOTAL_CARDOTEL", displayName: "Cardotel", width: 80, pinnedRight: true },
    { name: "TOTAL_FUXTIL", displayName: "Fuxtil", width: 80, pinnedRight: true },
    { name: "TOTAL_OTHERS", displayName: "Others (Pres. Base)", width: 80, pinnedRight: true },
    { name: "TOTAL_PRES", displayName: "Total Pres", width: 80, pinnedRight: true },
    { name: "AWARDED_AMOUNT", displayName: "Awarded Amt(TK)", cellFilter: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, width: 100, footerCellFilter: 'number:2', pinnedRight: true }
    ];


$scope.gridReportQualifiedMPO = {
    showGridFooter: true,
    showColumnFooter: true,
    enableFiltering: true,
    enableSorting: true,
    columnDefs: columnReportQualifiedMPOList,
    //rowTemplate: rowTemplate(),
    enableGridMenu: true,
    enableSelectAll: true,
    exporterCsvFilename: 'QualifiedMPO.csv',
    exporterMenuPdf: false,
    exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
    onRegisterApi: function (gridApi) {
        $scope.gridApi = gridApi;
    }
};
$scope.GetQualifiedMPOData = function () {
    $scope.gridReportQualifiedMPO.data = [];
    $http({
        method: "POST",
        url: MyApp.rootPath + "ReportQualifiedMPO/GetQualifiedMPOData",
        params: { FromDate: $scope.FromDate, ToDate: $scope.ToDate }
    }).then(function (response) {
        if (response.data.Status === null || response.data.Status === undefined) {
            if (response.data.length > 0) {
                $scope.gridReportQualifiedMPO.data = response.data;
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
    $scope.frmReportQualifiedMPO.Depot = "";
    $scope.frmReportQualifiedMPO.Zone = "";
    $scope.frmReportQualifiedMPO.Region = "";
    $scope.frmReportQualifiedMPO.Area = "";
    $scope.frmReportQualifiedMPO.Territory = "";

    $scope.FromDate = "";
    $scope.ToDate = "";
    $scope.Regions = [];
    $scope.Areas = [];
    $scope.Territories = [];
    $scope.gridReportQualifiedMPO.data = [];
    $scope.FromDate = firstDay;
    $scope.ToDate = toDay;

};
});

