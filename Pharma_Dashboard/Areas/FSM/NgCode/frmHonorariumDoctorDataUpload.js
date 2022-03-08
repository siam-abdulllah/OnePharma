app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
   // $scope.EventPerm(39);
    $scope.EventPerm(27);
    var index = "";
    var maxValue = 6000;
    var i = 0;


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
            params: { DepotCode: $scope.frmHonorariumDoctorDataUpload.Depot }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Zones = response.data;
                    $scope.frmHonorariumDoctorDataUpload.Zone = "";
                    $scope.frmHonorariumDoctorDataUpload.Region = "";
                    $scope.frmHonorariumDoctorDataUpload.Area = "";
                    $scope.frmHonorariumDoctorDataUpload.Territory = "";
                    $scope.Regions = [];
                    $scope.Areas = [];
                    $scope.Territories = [];


                }
            },
            function() {
                toastr.warning("Error Occurred");
            });
    };
    $scope.GetRegionByZone = function () {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetRegionByZone",
            params: { DepotCode: $scope.frmHonorariumDoctorDataUpload.Depot,ZoneCode: $scope.frmHonorariumDoctorDataUpload.Zone }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Regions = response.data;

                $scope.frmHonorariumDoctorDataUpload.Region = "";
                $scope.frmHonorariumDoctorDataUpload.Area = "";
                $scope.frmHonorariumDoctorDataUpload.Territory = "";
                $scope.Areas = [];
                $scope.Territories = [];
            }
        },
            function () {
                toastr.warning("Error Occurred");
            });
    };
    $scope.GetAreaByRegion = function () {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetAreaByRegion",
            params: { DepotCode: $scope.frmHonorariumDoctorDataUpload.Depot, ZoneCode: $scope.frmHonorariumDoctorDataUpload.Zone,RegionCode: $scope.frmHonorariumDoctorDataUpload.Region }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Areas = response.data;

                $scope.frmHonorariumDoctorDataUpload.Area = "";
                $scope.frmHonorariumDoctorDataUpload.Territory = "";
                $scope.Territories = [];
            }
        },
            function () {
                toastr.warning("Error Occurred");
            });
    };
    $scope.GetTerritoryByArea = function () {

        $http({
            method: "POST",
            url: MyApp.rootPath + "Default/GetTerritoryByArea",
            params: { DepotCode: $scope.frmHonorariumDoctorDataUpload.Depot, ZoneCode: $scope.frmHonorariumDoctorDataUpload.Zone, RegionCode: $scope.frmHonorariumDoctorDataUpload.Region,AreaCode: $scope.frmHonorariumDoctorDataUpload.Area }
        }).then(function (response) {
            if (response.data !== '') {
                $scope.Territories = response.data;
                $scope.frmHonorariumDoctorDataUpload.Territory = "";
            }
        },
            function () {
                toastr.warning("Error Occurred");
            });
    };

   
    var columnHonorariumDoctorDataUploadList = [
        { name: "SL_NO", displayName: "Sln", width: 80, type: "number", pinnedLeft: true },
        { name: "DOCTOR_CODE", displayName: "Doctor Code", width: 120 ,pinnedLeft: true },
        { name: "DOCTOR_NAME", displayName: "Doctor Name", width: 150, pinnedLeft: true},
        { name: "PRACTICING_DAY", displayName: "Practicing Day", width: 150, enablePinning: true, hidePinLeft: false, hidePinRight: true},
        { name: "PRESCRIPTION_PER_DAY", displayName: "Prescription Per Day", width: 150},
        { name: "HONORARIUM_AMOUNT", displayName: "Honorarium Amount", width: 130 },
        { name: "TERRITORY_CODE_4P", displayName: "Territory Code 4P", width: 150 },
        {
            name: 'Action ', enableFiltering: false, enableSorting: false, width: "100", pinnedRight: true,
            cellTemplate: '<div style="text-align:center;position: relative;width:100%;padding:2px 2px 2px 6px;"><button  class="btn btn-sm btn-success " ng-click="grid.appScope.editItemGrid(row)"><i class="fa fa-edit"></i>&nbspEdit</button></div>'
        },
        {
            name: 'Delete Action', displayName: "Action", enableFiltering: false, width: "100", pinnedRight: true,
            cellTemplate: '<div style="text-align:center;position: relative;width:100%;padding:2px 2px 2px 5px;"><button class="btn btn-danger btn-sm" ng-click="grid.appScope.deleteItemGrid(row)"><i class="fa fa-remove"></i>&nbspDelete</button></div>'
        }

    ];
      
    $scope.gridHonorariumDoctorDataUploadOptionsValue = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnHonorariumDoctorDataUploadList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Honorarium_Doctor_info.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }

    };
    //delete row from grid
    $scope.deleteItemGrid = function (row) {
        var index = $scope.gridHonorariumDoctorDataUploadOptionsValue.data.indexOf(row.entity);
        $scope.gridHonorariumDoctorDataUploadOptionsValue.data.splice(index, 1);
    };
    //edit row from grid
    $scope.editItemGrid = function (row) {
        $('#honorariumDocModal').modal('show');
        $scope.SlNoModal = row.entity.SL_NO;
        $scope.DoctorCodeModal = row.entity.DOCTOR_CODE;
        $scope.DoctorNameModal = row.entity.DOCTOR_NAME;
        $scope.PracticingDayModal= row.entity.PRACTICING_DAY;
        $scope.PrescriptionPerDayModal = row.entity.PRESCRIPTION_PER_DAY;
        $scope.HonorariumAmountModal = row.entity.HONORARIUM_AMOUNT;
        $scope.TerritoryCode4PModal = row.entity.TERRITORY_CODE_4P;
        $scope.SetDateModal = row.entity.SET_DATE;
        index = $scope.gridHonorariumDoctorDataUploadOptionsValue.data.indexOf(row.entity);
    };
    //add item to grid
    $scope.addItemGrid = function () {
        if (index !== null) {
            $scope.gridHonorariumDoctorDataUploadOptionsValue.data.splice(index, 1);
        }
        $scope.gridHonorariumDoctorDataUploadOptionsValue.data.unshift({ SL_NO: $scope.SlNoModal,DOCTOR_CODE: $scope.DoctorCodeModal, DOCTOR_NAME: $scope.DoctorNameModal, PRACTICING_DAY: $scope.PracticingDayModal, PRESCRIPTION_PER_DAY: $scope.PrescriptionPerDayModal, HONORARIUM_AMOUNT: $scope.HonorariumAmountModal, TERRITORY_CODE_4P: $scope.TerritoryCode4PModal, SET_DATE: $scope.SetDateModal});
        $scope.SlNoModal = "";
        $scope.DoctorCodeModal = "";
        $scope.DoctorNameModal = "";
        $scope.PracticingDayModal = "";
        $scope.PrescriptionPerDayModal = "";
        $scope.HonorariumAmountModal = "";
        $scope.TerritoryCode4PModal = "";
        $scope.SetDateModal = "";
       
        index = null;
        $('#honorariumDocModal').modal('hide');
    };
    //upload doc to grid
    $scope.UploadDocument = function () {
        $scope.gridHonorariumDoctorDataUploadOptionsValue.data = [];
        var fileUpload = $("#DocFile").get(0);
        var files = fileUpload.files;
        if (files.length <= 0) {
            toastr.warning("Please Select a File !");
            return false;
        }

        if (files.length > 0) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                var name = files[0].name;
                var regex = new RegExp("(.*?)\.(xlsx|xls|csv)$");
                if (!(regex.test(name))) {
                    $('#DocFile').val('');
                    toastr.warning('Please select correct file format');
                    return false;
                }
                $.ajax({
                    type: "POST",
                    url: MyApp.rootPath +
                        'Default/UploadFile',
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (response) {
                        if (response.Status === "Ok") {
                            $scope.GetDocinfo(response.fileName, response.physicalPath);
                        } else {
                            toastr.warning(" Upload Failed!");
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = "Error " + " " + status + " " + p3 + " " + p4;
                        if (xhr.responseText && xhr.responseText[0] === "{")
                            err = JSON.parse(xhr.responseText).Message;
                        console.log(err);
                    }
                });
            }
        }

    };
    $scope.GetDocinfo = function(fileName, physicalPath) {

        $http({
            method: "POST",
            url: MyApp.rootPath + "HonorariumDoctorDataUpload/LoadExcelFile",
            params: { fileName: fileName, physicalPath: physicalPath }
        }).then(function(response) {
                if (response.data.Status === null || response.data.Status === undefined) {
                    if (response.data.length > 0) {
                        $scope.gridHonorariumDoctorDataUploadOptionsValue.data = response.data;
                        $scope.gridHonorariumDoctorDataUploadOptionsValue.columnDefs[8].visible = true;
                    } else {
                        toastr.warning("No Data Found!");

                    }
                } else {
                    toastr.warning(response.data.Status, { timeOut: 2000 });
                }
            },
            function(response) {
                console.log(response);
                toastr.warning("Error Occurred!", { timeOut: 2000 });
            });
    };
    $scope.SaveData = function () {
        var honorariumData = $scope.gridHonorariumDoctorDataUploadOptionsValue.data;
        if (!honorariumData.length > 0) {
            toastr.warning('Please Insert Data First');
            return false;
        }
        var honorariumDataLength = honorariumData.length - 1;
        //var n = parseInt(doctorDataLength / maxValue);
        $scope.SaveDataFunction(honorariumDataLength, honorariumData);
    };
    $scope.SaveDataFunction = function (honorariumDataLength, honorariumData) {

        var n = parseInt((honorariumData.length - 1) / maxValue);
        if (honorariumDataLength > maxValue) {
            var j = honorariumDataLength - maxValue;
        } else {
            var j = 0;
        }
        var honorariumDataK = honorariumData.slice(j, honorariumDataLength);
        //doctorDataLength = j;
        $http({
            method: "post",
            url: MyApp.rootPath + "HonorariumDoctorDataUpload/OperationsMode",
            datatype: "json",
            data: { model: honorariumDataK }
        }).then(function (response) {
                if (response.data.Status === 'Yes') {

                    if (i >= n) {
                        toastr.success('Updated Successfully', 'Success Alert', { timeOut: 2000 });
                    } else {
                        honorariumDataLength = j - 1;
                        $scope.SaveDataFunction(honorariumDataLength,honorariumData);
                    }
                    i++;
                } else {
                    toastr.error(response.data.Status, { timeOut: 2000 });
                }
            },
            function (response) {
                console.log(response);
                toastr.warning("Error Occurred!", { timeOut: 2000 });
            });

    };
    //GetDoctorGridData
    $scope.GetDoctorGridData = function () {
        $scope.gridHonorariumDoctorDataUploadOptionsValue.data = [];
        $http({
            method: "POST",
            url: MyApp.rootPath + "HonorariumDoctorDataUpload/GetDoctorGridData",
            params: { DepotCode: $scope.frmHonorariumDoctorDataUpload.Depot, ZoneCode: $scope.frmHonorariumDoctorDataUpload.Zone, RegionCode: $scope.frmHonorariumDoctorDataUpload.Region, AreaCode: $scope.frmHonorariumDoctorDataUpload.Area, TerritoryCode: $scope.frmHonorariumDoctorDataUpload.Territory }
        }).then(function (response) {
            if (response.data.Status === null || response.data.Status === undefined) {
            if (response.data.length > 0) {
                $scope.gridHonorariumDoctorDataUploadOptionsValue.data = response.data;
                $scope.gridHonorariumDoctorDataUploadOptionsValue.columnDefs[8].visible = false;
            } else {
                toastr.warning("No Data Found", { timeOut: 2000 });
            }
            } else {
                toastr.warning(response.data.Status, { timeOut: 2000 });
            }
        }, function (response) {
            console.log(response);
            toastr.warning("Error Occurred!", { timeOut: 2000 });
        });
    };

    //Reset
    $scope.Reset = function () {
        var fileElement = angular.element('#DocFile');
        angular.element(fileElement).val(null);
        $scope.gridHonorariumDoctorDataUploadOptionsValue.data = [];
        $scope.frmHonorariumDoctorDataUpload.Depot = "";
        $scope.frmHonorariumDoctorDataUpload.Zone = "";
        $scope.frmHonorariumDoctorDataUpload.Region = "";
        $scope.frmHonorariumDoctorDataUpload.Area = "";
        $scope.frmHonorariumDoctorDataUpload.Territory = "";
        $scope.Regions = [];
        $scope.Zones = [];
        $scope.Areas = [];
        $scope.Territories = [];

    };
});

