app.controller("myCtrl", function ($scope, $http, uiGridConstants, $q) {
    $scope.EventPerm(26);
    $scope.isDisabled = false;
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
            params: { DepotCode: $scope.frmDoctorDataUpload.Depot }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Zones = response.data;
                    $scope.frmDoctorDataUpload.Zone = "";
                    $scope.frmDoctorDataUpload.Region = "";
                    $scope.frmDoctorDataUpload.Area = "";
                    $scope.frmDoctorDataUpload.Territory = "";
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
            params: { DepotCode: $scope.frmDoctorDataUpload.Depot,ZoneCode: $scope.frmDoctorDataUpload.Zone }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Regions = response.data;

                    $scope.frmDoctorDataUpload.Region = "";
                    $scope.frmDoctorDataUpload.Area = "";
                    $scope.frmDoctorDataUpload.Territory = "";
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
            params: { DepotCode: $scope.frmDoctorDataUpload.Depot, ZoneCode: $scope.frmDoctorDataUpload.Zone,RegionCode: $scope.frmDoctorDataUpload.Region }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Areas = response.data;

                    $scope.frmDoctorDataUpload.Area = "";
                    $scope.frmDoctorDataUpload.Territory = "";
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
            params: { DepotCode: $scope.frmDoctorDataUpload.Depot, ZoneCode: $scope.frmDoctorDataUpload.Zone, RegionCode: $scope.frmDoctorDataUpload.Region,AreaCode: $scope.frmDoctorDataUpload.Area }
        }).then(function(response) {
                if (response.data !== '') {
                    $scope.Territories = response.data;
                    $scope.frmDoctorDataUpload.Territory = "";
                }
            },
            function() {
                toastr.warning("Error Occurred");
            });
    };

    var columnDoctorDataUploadValueList = [
        { name: "SL_NO", displayName: "Sln", width: 80, type: "number", pinnedLeft: true},
        { name: "DOCTOR_CODE", displayName: "Doctor Code", width: 80, pinnedLeft: true},
        { name: "DOCTOR_NAME", displayName: "Doctor Name", width: 110, pinnedLeft: true},
        //{ name: "DOCTOR_CODE_4P", displayName: "Doctor Code 4P", width: 110 },
        { name: "CLASS_GROUP", displayName: "Class", width: 110, enablePinning: true, hidePinLeft: false, hidePinRight: true },
        { name: "ADDRESS", displayName: "Address", width: 110 },
        { name: "DEGREES", displayName: "Degrees", width: 110 },
        { name: "DESIGNATION", displayName: "Designation", width: 110 },
        { name: "CONTRACT_NO", displayName: "Contract No", width: 110 },
        { name: "EMAIL", displayName: "Email", width: 110 },
        { name: "TERRITORY_CODE_4P", displayName: "Territory Code 4P", width: 110 },
        { name: "SPECIALTY", displayName: "Specialty", width: 110 },
        {
            name: 'Action ', enableFiltering: false, enableSorting: false, width: "100", pinnedRight: true  ,
            cellTemplate: '<div style="text-align:center;position: relative;width:100%;padding:2px 2px 2px 6px;"><button  class="btn btn-sm btn-success " ng-click="grid.appScope.editItemGrid(row)"><i class="fa fa-edit"></i>&nbspEdit</button></div>'
        },
        {
            name: 'Delete Action', displayName: "Action", enableFiltering: false, width: "100",pinnedRight: true  ,
            cellTemplate: '<div style="text-align:center;position: relative;width:100%;padding:2px 2px 2px 5px;"><button class="btn btn-danger btn-sm" ng-click="grid.appScope.deleteItemGrid(row)"><i class="fa fa-remove"></i>&nbspDelete</button></div>'
        }

    ];
    $scope.gridDoctorDataUploadOptionsValue = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnDoctorDataUploadValueList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'Doctor_info.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }

    };
    $scope.GetDoctorGridData = function () {
        $scope.gridDoctorDataUploadOptionsValue.data = [];
        $http({
            method: "POST",
            url: MyApp.rootPath + "DoctorDataUpload/GetDoctorGridData",
            params: { DepotCode: $scope.frmDoctorDataUpload.Depot, ZoneCode: $scope.frmDoctorDataUpload.Zone, RegionCode: $scope.frmDoctorDataUpload.Region, AreaCode: $scope.frmDoctorDataUpload.Area, TerritoryCode: $scope.frmDoctorDataUpload.Territory }
        }).then(function (response) {
            if (response.data.Status === null || response.data.Status === undefined) {
            if (response.data.length > 0) {
                    $scope.gridDoctorDataUploadOptionsValue.data = response.data;
                    $scope.gridDoctorDataUploadOptionsValue.columnDefs[12].visible = false;
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
    
    //edit row from grid
    $scope.editItemGrid = function (row) {
        $('#DoctorModal').modal('show');
        $scope.SlNoModal = row.entity.SL_NO;
        $scope.DoctorCodeModal = row.entity.DOCTOR_CODE;
        $scope.DoctorNameModal = row.entity.DOCTOR_NAME;
        $scope.ClassModal = row.entity.CLASS_GROUP;
        $scope.AddressModal = row.entity.ADDRESS;
        $scope.DegreesModal = row.entity.DEGREES;
        $scope.DesignationModal = row.entity.DESIGNATION;
        $scope.ContractNoModal = row.entity.CONTRACT_NO;
        $scope.EmailModal = row.entity.EMAIL;
        $scope.TerritoryCode4PModal = row.entity.TERRITORY_CODE_4P;
        $scope.SpecialtyModal = row.entity.SPECIALTY;
        index = $scope.gridDoctorDataUploadOptionsValue.data.indexOf(row.entity);
    };
    //add item to grid
    $scope.addItemGrid = function () {
        if (index !== null) {
            $scope.gridDoctorDataUploadOptionsValue.data.splice(index, 1);
        }
        $scope.gridDoctorDataUploadOptionsValue.data.unshift({
            SL_NO: $scope.SlNoModal,
            DOCTOR_CODE: $scope.DoctorCodeModal, DOCTOR_NAME: $scope.DoctorNameModal,
            CLASS_GROUP: $scope.ClassModal, ADDRESS: $scope.AddressModal,
            DEGREES: $scope.DegreesModal, DESIGNATION: $scope.DesignationModal,
            CONTRACT_NO: $scope.ContractNoModal,
            EMAIL: $scope.EmailModal,
            TERRITORY_CODE_4P: $scope.TerritoryCode4PModal,
            SPECIALTY: $scope.SpecialtyModal
        });
        $scope.SlNoModal = "";
        $scope.DoctorCodeModal = "";
        $scope.DoctorNameModal = "";
        $scope.ClassModal = "";
        $scope.AddressModal = "";
        $scope.DegreesModal = "";
        $scope.DesignationModal = "";
        $scope.ContractNoModal = "";
        $scope.EmailModal = "";
        $scope.TerritoryCode4PModal = "";
        $scope.SpecialtyModal = "";

        index = null;
        $('#DoctorModal').modal('hide');
    };
    //upload doc to grid
    //delete row from grid
    $scope.deleteItemGrid = function (row) {
        var index = $scope.gridDoctorDataUploadOptionsValue.data.indexOf(row.entity);
        $scope.gridDoctorDataUploadOptionsValue.data.splice(index, 1);
    };
    //upload doc to grid
    $scope.UploadDocument = function () {
        $scope.gridDoctorDataUploadOptionsValue.data = [];
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
                            //toastr.warning("Upload Failed!");
                            toastr.warning(response.Status);
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        //alert(xhr.responseText);
                        console.log(xhr.responseText);
                        toastr.warning("Error Occurred!", { timeOut: 2000 });
                    }
                });
            }
        }

    };
    $scope.GetDocinfo = function(fileName, physicalPath) {

        $http({
            method: "POST",
            url: MyApp.rootPath + "DoctorDataUpload/LoadExcelFile",
            params: { fileName: fileName, physicalPath: physicalPath }
        }).then(function(response) {
                if (response.data.Status === null || response.data.Status === undefined) {
                    if (response.data.length > 0) {
                        $scope.gridDoctorDataUploadOptionsValue.data = response.data;
                        $scope.gridDoctorDataUploadOptionsValue.columnDefs[12].visible = false;
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

    //save data from grid
    $scope.SaveData = function () {
        var doctorData = $scope.gridDoctorDataUploadOptionsValue.data;
        if (!doctorData.length > 0) {
            toastr.warning('Please Insert Data First');
            return false;
        }
        var doctorDataLength = doctorData.length - 1;
        $scope.SaveDataFunction(doctorDataLength, doctorData);
    };
    $scope.SaveDataFunction = function (doctorDataLength, doctorData) {
        var n = parseInt((doctorData.length - 1) / maxValue);
        if (doctorDataLength > maxValue) {
            var j = doctorDataLength - maxValue;
        } else {
            var j = 0;
        }
        var doctorDataK = doctorData.slice(j, doctorDataLength);
        $http({
            method: "post",
            url: MyApp.rootPath + "DoctorDataUpload/OperationsMode",
            datatype: "json",
            data: { model: doctorDataK }
        }).then(function (response) {
            if (response.data.Status === 'Yes') {
                if (i >= n) {
                    toastr.success('Updated Successfully', 'Success Alert', { timeOut: 2000 });
                } else {
                    doctorDataLength = j - 1;
                    $scope.SaveDataFunction(doctorDataLength, doctorData);
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
    
    //Reset
    $scope.Reset = function () {
        var fileElement = angular.element('#DocFile');
        angular.element(fileElement).val(null);
        $scope.gridDoctorDataUploadOptionsValue.data = [];
        $scope.frmDoctorDataUpload.Depot = "";
        $scope.frmDoctorDataUpload.Zone = "";
        $scope.frmDoctorDataUpload.Region = "";
        $scope.frmDoctorDataUpload.Area = "";
        $scope.frmDoctorDataUpload.Territory = "";
        $scope.Regions = [];
        $scope.Zones = [];
        $scope.Areas = [];
        $scope.Territories = [];

    };
});

