app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(36);
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
            params: { DepotCode: $scope.frmReportRegionWiseDoctorIncentive.Depot }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Zones = response.data;
                $scope.frmReportRegionWiseDoctorIncentive.Zone = "";
                $scope.frmReportRegionWiseDoctorIncentive.Region = "";
                $scope.frmReportRegionWiseDoctorIncentive.Area = "";
                $scope.frmReportRegionWiseDoctorIncentive.Territory = "";
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
            params: { ZoneCode: $scope.frmReportRegionWiseDoctorIncentive.Zone }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Regions = response.data;

                $scope.frmReportRegionWiseDoctorIncentive.Region = "";
                $scope.frmReportRegionWiseDoctorIncentive.Area = "";
                $scope.frmReportRegionWiseDoctorIncentive.Territory = "";
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
            params: { RegionCode: $scope.frmReportRegionWiseDoctorIncentive.Region }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Areas = response.data;

                $scope.frmReportRegionWiseDoctorIncentive.Area = "";
                $scope.frmReportRegionWiseDoctorIncentive.Territory = "";
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
            params: { AreaCode: $scope.frmReportRegionWiseDoctorIncentive.Area }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Territories = response.data;
                $scope.frmReportRegionWiseDoctorIncentive.Territory = "";
            }
        }, function () {
            toastr.warning("Error Occurred");
        });
    }

    //$scope.GetHeadMenuList();
    //
    var columnRegionWiseDoctorIncentiveList = [
        { name: "SL_NO", displayName: "Sln", width: 50, pinnedLeft: true},
        { name: "DOCTOR_NAME", displayName: "Doctor Name", width: 185, pinnedLeft: true},
        { name: "DEGREES", displayName: "Degree", width: 80, pinnedLeft: true},
        { name: "MIO_NAME", displayName: "MPO Name", width: 135, enablePinning: true, hidePinLeft: false, hidePinRight: true },
        { name: "MIO_DESIGNATION_NAME", displayName: "Desig.", width: 80 },
        { name: "ADDRESS", displayName: "Address", width: 150 },
        { name: "TERRITORY_NAME", displayName: "Territory Name ", width: 130 },
        { name: "AREA_NAME", displayName: "Area Name", width: 95 },
        { name: "REGION_NAME", displayName: "Region Name", width: 115 },
        { name: "TOTAL_PRESCRIPTION", displayName: "Total Prescription", width: 120, pinnedRight: true},
        { name: "MIO_INCENTIVE", displayName: "MPO Incentive(TK)", width: 130, pinnedRight: true},
        { name: "TYPE", displayName: "Hon/Non Hon", width: 150, pinnedRight: true}
    ];


    $scope.gridReportRegionWiseDoctorIncentive = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnRegionWiseDoctorIncentiveList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'RegionWiseDoctorIncentive.csv',

        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        exporterExcelFilename: 'RegionWiseDoctorIncentive.xlsx',
        exporterExcelSheetName: 'Sheet1',
        exporterColumnScaleFactor: 4.5,
        exporterFieldApplyFilters: true,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }
    };



    $scope.GetRegionWiseDoctorIncentiveData = function () {
        $scope.gridReportRegionWiseDoctorIncentive.data = [];
        $http({
            method: "POST",
            url: MyApp.rootPath + "ReportRegionWiseDoctorIncentive/GetRegionWiseDoctorIncentiveData",
            params: { FromDate: $scope.FromDate, ToDate: $scope.ToDate }
        }).then(function (response) {
            if (response.data.Status === null || response.data.Status === undefined) {
                if (response.data.length > 0) {
                    $scope.gridReportRegionWiseDoctorIncentive.data = response.data;
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
        $scope.frmReportRegionWiseDoctorIncentive.Depot = "";
        $scope.frmReportRegionWiseDoctorIncentive.Zone = "";
        $scope.frmReportRegionWiseDoctorIncentive.Region = "";
        $scope.frmReportRegionWiseDoctorIncentive.Area = "";
        $scope.frmReportRegionWiseDoctorIncentive.Territory = "";

        $scope.FromDate = "";
        $scope.ToDate = "";
        $scope.Regions = [];
        $scope.Areas = [];
        $scope.Territories = [];
        $scope.gridReportRegionWiseDoctorIncentive.data = [];
        $scope.FromDate = firstDay;
        $scope.ToDate = toDay;

    };
});

