app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(33);
   // $scope.GetDepot();
    //$scope.isDisabled = false;
    var index = "";


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
            params: { DepotCode: $scope.frmReportMPOWiseTopPrescription.Depot }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Zones = response.data;
                $scope.frmReportMPOWiseTopPrescription.Zone = "";
                $scope.frmReportMPOWiseTopPrescription.Region = "";
                $scope.frmReportMPOWiseTopPrescription.Area = "";
                $scope.frmReportMPOWiseTopPrescription.Territory = "";
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
            params: { ZoneCode: $scope.frmReportMPOWiseTopPrescription.Zone }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Regions = response.data;

                $scope.frmReportMPOWiseTopPrescription.Region = "";
                $scope.frmReportMPOWiseTopPrescription.Area = "";
                $scope.frmReportMPOWiseTopPrescription.Territory = "";
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
            params: { RegionCode: $scope.frmReportMPOWiseTopPrescription.Region }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Areas = response.data;

                $scope.frmReportMPOWiseTopPrescription.Area = "";
                $scope.frmReportMPOWiseTopPrescription.Territory = "";
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
            params: { AreaCode: $scope.frmReportMPOWiseTopPrescription.Area }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Territories = response.data;
                $scope.frmReportMPOWiseTopPrescription.Territory = "";
            }
        }, function () {
            toastr.warning("Error Occurred");
        });
    }

    //$scope.GetHeadMenuList();
    //
    var columnReportMPOWiseTopPrescriptionList = [
        { name: "SL_NO", displayName: "Sln", width: 80 },
        //{ name: "MST_SL", displayName: "ID", width: 180 },
        { name: "USER_ID", displayName: "MPO Code", width: 120 },
        { name: "MIO_NAME", displayName: "MPO Name", width:250 },
        { name: "DESIG_NAME", displayName: "Designation", width: 150 },
        { name: "TERRITORY_NAME", displayName: "Territory", width: 180 },
        { name: "PRESCRIPTION_QTY", displayName: "Prescription Qty", width: 150 }
    ];


    $scope.gridReportMPOWiseTopPrescription = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnReportMPOWiseTopPrescriptionList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'MPOWiseTopPrescription.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }
    };
    $scope.showImage = function (row) {
        $('#showImageModal').modal('show');
        $("#prescriptionImg").html(" ");
        //$scope.prescriptionImageSRC = row.entity.PRESCRIPTION_URL.replace("~", "");
        // $scope.prescriptionImageSRC = row.entity.PRESCRIPTION_URL;
        var curect_file_path = row.entity.PRESCRIPTION_URL.replace("~", "");
        $("#prescriptionImg").verySimpleImageViewer({
            imageSource: curect_file_path,
            frame: ['100%', '100%'],
            maxZoom: '900%',
            zoomFactor: '10%',
            mouse: true,
            keyboard: true,
            toolbar: true,
            rotateToolbar: true
        });

    };

    //GetDoctorGridData
    $scope.GetMPOWiseTopPrescriptionData = function () {
        //if ($scope.FromDate && $scope.ToDate) {
        //    if (!DateCheck($scope.FromDate, $scope.ToDate)) {
        //        $scope.isDisabled = false;
        //        return false;
        //    }
        //}
        $scope.gridReportMPOWiseTopPrescription.data = [];
        $http({
            method: "POST",
            url: MyApp.rootPath + "ReportMPOWiseTopPrescription/GetMPOWiseTopPrescriptionData",
            params: { DepotCode: $scope.frmReportMPOWiseTopPrescription.Depot, ZoneCode: $scope.frmReportMPOWiseTopPrescription.Zone, RegionCode: $scope.frmReportMPOWiseTopPrescription.Region, AreaCode: $scope.frmReportMPOWiseTopPrescription.Area, TerritoryCode: $scope.frmReportMPOWiseTopPrescription.Territory, FromDate: $scope.FromDate, ToDate: $scope.ToDate }
        }).then(function (response) {
            if (response.data.length>0) {
                $scope.gridReportMPOWiseTopPrescription.data = response.data;
            } else {
                toastr.warning("No Data Found", { timeOut: 2000 });
            }
        }, function () {
            alert("Error Loading Category");
        });
    };

    //Reset
    $scope.Reset = function () {
        $scope.frmReportMPOWiseTopPrescription.Depot = "";
        $scope.frmReportMPOWiseTopPrescription.Zone = "";
        $scope.frmReportMPOWiseTopPrescription.Region = "";
        $scope.frmReportMPOWiseTopPrescription.Area = "";
        $scope.frmReportMPOWiseTopPrescription.Territory = "";
        
        $scope.FromDate ="";
        $scope.ToDate = "";
        $scope.Regions = [];
        $scope.Areas = [];
        $scope.Territories = [];
        $scope.gridReportMPOWiseTopPrescription.data = [];

    };
});

