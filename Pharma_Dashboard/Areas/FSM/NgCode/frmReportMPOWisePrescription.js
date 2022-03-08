app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(32);
    //$scope.GetDepot();
    //$scope.isDisabled = false;
    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    firstDay = ('0' + firstDay.getDate()).slice(-2) +'-' + ('0' + (firstDay.getMonth() + 1)).slice(-2) + '-' + firstDay.getFullYear();
    var toDay = new Date();
    toDay = ('0' + toDay.getDate()).slice(-2) + '-' + ('0' + (toDay.getMonth() + 1)).slice(-2) + '-' + toDay.getFullYear();
    $scope.FromDate = firstDay;
    $scope.ToDate = toDay;
    var index = "";
    //var serverIPAdd = 'http://172.16.128.118:132';
    //var serverIPAdd = 'http://10.12.6.176:8999';
    var serverIPAdd = 'http://http://202.84.32.118:8999';

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
            params: { DepotCode: $scope.frmReportMPOWisePrescription.Depot }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Zones = response.data;
                    $scope.frmReportMPOWisePrescription.Zone = "";
                    $scope.frmReportMPOWisePrescription.Region = "";
                    $scope.frmReportMPOWisePrescription.Area = "";
                    $scope.frmReportMPOWisePrescription.Territory = "";
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
            params: {
                DepotCode: $scope.frmReportMPOWisePrescription.Depot,
                ZoneCode: $scope.frmReportMPOWisePrescription.Zone
            }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Regions = response.data;

                    $scope.frmReportMPOWisePrescription.Region = "";
                    $scope.frmReportMPOWisePrescription.Area = "";
                    $scope.frmReportMPOWisePrescription.Territory = "";
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
            params: {
                DepotCode: $scope.frmReportMPOWisePrescription.Depot,
                ZoneCode: $scope.frmReportMPOWisePrescription.Zone,
                RegionCode: $scope.frmReportMPOWisePrescription.Region
            }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Areas = response.data;

                    $scope.frmReportMPOWisePrescription.Area = "";
                    $scope.frmReportMPOWisePrescription.Territory = "";
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
            params: {
                DepotCode: $scope.frmReportMPOWisePrescription.Depot,
                ZoneCode: $scope.frmReportMPOWisePrescription.Zone,
                RegionCode: $scope.frmReportMPOWisePrescription.Region,
                AreaCode: $scope.frmReportMPOWisePrescription.Area
            }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Territories = response.data;
                    $scope.frmReportMPOWisePrescription.Territory = "";
                }
            },
            function() {
                toastr.warning("Error Occurred");
            });
    };

    //$scope.GetHeadMenuList();
    //
    var columnReportMPOWisePrescriptionList = [
        { name: "SL_NO", displayName: "Sln", width: 80 },
        { name: "USER_ID", displayName: "User ID", visible: false },
        { name: "EMPLOYEE_NAME", displayName: "Employee Name", width: 140 },
        { name: "CAPTURE_TIME", displayName: "Capture Date & Time", width: 140 },
        { name: "PRESCRIPTION_TYPE", displayName: "Pescription Type", width: 120 },
        { field: 'PRESCRIPTION_URL', displayName: "Image", width: 120, cellTemplate: '<div style="text-align:center;position: relative;width:100%;padding:2px 2px 2px 6px;"><img width="50px" ng-src="' + serverIPAdd + '{{grid.getCellValue(row, col)}}" lazy-src ng-click="grid.appScope.showImage(row)"></div>' },
        { name: "DOCTOR_CODE", displayName: "Doctor Code", visible: false },
        { name: "DOCTOR_NAME", displayName: "Doctor Name", width: 140 },
        { name: "Chemist Name", displayName: "Chemist Name", width: 130 },
        { name: "TOTAL_PROD", displayName: "Total Product", width: 110 }
    ];


    $scope.gridReportMPOWisePrescription = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnReportMPOWisePrescriptionList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'MPOWisePrescription.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
            $scope.gridApi.grid.registerDataChangeCallback(function () {
                $scope.gridApi.treeBase.expandAllRows();
            });
        }
    };

    $scope.showImage = function (row) {
       // $('#showImageModal').modal('show');
        $("#prescriptionImg").html(" ");
        //$scope.prescriptionImageSRC = row.entity.PRESCRIPTION_URL.replace("~", "");
        // $scope.prescriptionImageSRC = row.entity.PRESCRIPTION_URL;
        var curect_file_path = serverIPAdd + row.entity.PRESCRIPTION_URL.replace("~", "");
        $("#prescriptionImg").verySimpleImageViewer({
            imageSource: curect_file_path,
            frame: ['100%', '100%'],
            maxZoom: '900%',
            zoomFactor: '10%',
            saveZoomPos: false,
            mouse: true,
            keyboard: true,
            toolbar: true,
            rotateToolbar: true
        });
        $('#showImageModal').modal('show');
        //$('#prescriptionImg img').width(300); 
        //$('#prescriptionImg img').height(400);
        $('.jqvsiv_main_image_content>img').attr('id','image');
        //$('.jqvsiv_main_image_content>img').attr({
        //    width: 600,
        //    height: 400
        //});

    };
    $('#showImageModal').on('shown.bs.modal', function (event) {
        //var h = $('.jqvsiv_main_image_content img').attr('height', $('.jqvsiv_main_image_content img').height());
        //var w = $('.jqvsiv_main_image_content img').attr('width', $('.jqvsiv_main_image_content img').width());


        //var myImg = document.querySelector("#image");
        //var w = myImg.naturalWidth;
        //var h = myImg.naturalHeight;

        //alert(w + " " + h);
        $('#image').attr({
            width: 330,
            height: 440
        });
        $('#image').css("left", "135px");
    });
    //GetDoctorGridData
    $scope.GetMPOWisePrescriptionData = function () {
        //    if ($scope.FromDate && $scope.ToDate) {
        //        if (!DateCheck($scope.FromDate, $scope.ToDate)) {
        //            $scope.isDisabled = false;
        //            return false;
        //        }
        //    }
        $scope.gridReportMPOWisePrescription.data = [];
        $http({
            method: "POST",
            url: MyApp.rootPath + "ReportMPOWisePrescription/GetMPOWisePrescriptionData",
            params: { DepotCode: $scope.frmReportMPOWisePrescription.Depot, ZoneCode: $scope.frmReportMPOWisePrescription.Zone, RegionCode: $scope.frmReportMPOWisePrescription.Region, AreaCode: $scope.frmReportMPOWisePrescription.Area, TerritoryCode: $scope.frmReportMPOWisePrescription.Territory, FromDate: $scope.FromDate, ToDate: $scope.ToDate }
        }).then(function (response) {
            if (response.data.length > 0) {
                $scope.gridReportMPOWisePrescription.data = response.data;
            } else {
                toastr.warning("No Data Found", { timeOut: 2000 });
            }
        }, function () {
            alert("Error Loading Category");
        });
    };

    //Reset
    $scope.Reset = function () {
        $scope.frmReportMPOWisePrescription.Depot = "";
        $scope.frmReportMPOWisePrescription.Zone = "";
        $scope.frmReportMPOWisePrescription.Region = "";
        $scope.frmReportMPOWisePrescription.Area = "";
        $scope.frmReportMPOWisePrescription.Territory = "";
        $scope.FromDate = "";
        $scope.ToDate = "";
        $scope.Regions = [];
        $scope.Areas = [];
        $scope.Territories = [];
        $scope.gridReportMPOWisePrescription.data = [];
        $scope.FromDate = firstDay;
        $scope.ToDate = toDay;

    };
});

