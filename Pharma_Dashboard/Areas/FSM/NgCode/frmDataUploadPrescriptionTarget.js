app.controller("myCtrl", function ($scope, $http, uiGridConstants) {
    $scope.EventPerm(28);
    $scope.isDisabled = false;
    
    var index = null;
    var maxValue = 6000;
    var i = 0;
    var dateObj = new Date();
    var month = ('0' + (dateObj.getMonth() + 1)).slice(-2);
    var year = dateObj.getFullYear();
    $scope.Year = year;
    $http({
        method: "POST",
        url: MyApp.rootPath + "DataUploadPrescriptionTarget/GetMonthName"
    }).then(function(response) {

        if (response.data.length > 0) {
            $scope.Months = response.data;
           
            $scope.frmDataUploadPrescriptionTarget.MonthNumber = month;
            //$scope.gridDataUploadPrescriptionTargetOptionsValue.columnDefs[10].visible = true;
        } else {
            toastr.warning("No Data Found!");
        }
    }, function () {
        toastr.warning("Error Occurred");
        });

    var columnDataUploadPrescriptionTargetValueList = [
        { name: "SL_NO", displayName: "Sln", width: 60, type: "number" },
        { name: "MPO_CODE", displayName: "MPO Code", width: 80 },
        { name: "MPO_NAME", displayName: "MPO Name", width: 180 },
        { name: "TERRITORY_CODE", displayName: "Territory Code 4P", width: 120 },
        { name: "PRESCRIPTION_QTY", displayName: "Prescription Qty", width: 150 },
        { name: "SET_DATE", displayName: "Set Date", width: 150  },
        {
            name: 'Action ', enableFiltering: false, enableSorting: false, width: "100",
            cellTemplate: '<div style="text-align:center;position: relative;width:100%;padding:2px 2px 2px 6px;"><button  class="btn btn-sm btn-success " ng-click="grid.appScope.editItemGrid(row)"><i class="fa fa-edit"></i>&nbspEdit</button></div>'
        }
        //,
        //{
        //    name: 'Delete Action', displayName: "Action", enableFiltering: false, width: "100",
        //    cellTemplate: '<div style="text-align:center;position: relative;width:100%;padding:2px 2px 2px 5px;"><button class="btn btn-danger btn-sm" ng-click="grid.appScope.deleteItemGrid(row)"><i class="fa fa-remove"></i>&nbspDelete</button></div>'
        //}

    ];


    $scope.gridDataUploadPrescriptionTargetOptionsValue = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: columnDataUploadPrescriptionTargetValueList,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'DataUploadPrescriptionTarget.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }

    };
    //delete row from grid
    $scope.deleteItemGrid = function (row) {
        var index = $scope.gridDataUploadPrescriptionTargetOptionsValue.data.indexOf(row.entity);
        $scope.gridDataUploadPrescriptionTargetOptionsValue.data.splice(index, 1);
    };
    //edit row from grid
    $scope.editItemGrid = function (row) {
        $('#prescriptionTarModal').modal('show');
        $scope.SlNoModal = row.entity.SL_NO;
        $scope.MPONameModal = row.entity.MPO_NAME;
        $scope.MPOCodeModal = row.entity.MPO_CODE;
        $scope.TerritoryCodeModal = row.entity.TERRITORY_CODE;
        $scope.PrescriptionQtyModal = row.entity.PRESCRIPTION_QTY;
        $scope.SetDateModal = row.entity.SET_DATE;
        $scope.YearModal = row.entity.YEAR;
        $scope.MonthNumberModal = row.entity.MONTH_NUMBER;
        index = $scope.gridDataUploadPrescriptionTargetOptionsValue.data.indexOf(row.entity);
    };
    //add item to grid
    $scope.addItemGrid = function () {
        if (index !== null) {
            $scope.gridDataUploadPrescriptionTargetOptionsValue.data.splice(index, 1);
        }
        // $scope.gridDataUploadPrescriptionTargetOptionsValue.data.push({ MPO_CODE: $scope.MPOCodeModal, TERRITORY_CODE: $scope.TerritoryCodeModal, PRESCRIPTION_QTY: $scope.PrescriptionQtyModal, SET_DATE: $scope.SetDateModal, YEAR: $scope.YearModal, MONTH_NUMBER: $scope.MonthNumberModal });
        $scope.gridDataUploadPrescriptionTargetOptionsValue.data.unshift({ SL_NO: $scope.SlNoModal, MPO_CODE: $scope.MPOCodeModal, MPO_NAME: $scope.MPONameModal, TERRITORY_CODE: $scope.TerritoryCodeModal, PRESCRIPTION_QTY: $scope.PrescriptionQtyModal, SET_DATE: $scope.SetDateModal, YEAR: $scope.YearModal, MONTH_NUMBER: $scope.MonthNumberModal });
        $scope.SlNoModal = "";
        $scope.MPOCodeModal = "";
        $scope.MPONameModal = "";
        $scope.TerritoryCodeModal = "";
        $scope.PrescriptionQtyModal = "";
        $scope.YearModal = "";
        $scope.MonthNumberModal = "";
        $scope.SetDateModal = "";
        index = null;
        $('#prescriptionTarModal').modal('hide');
    };
    //upload doc to grid
    $scope.UploadDocument = function () {
        $scope.gridDataUploadPrescriptionTargetOptionsValue.data = [];
        var fileUpload = $("#DocFile").get(0);
        var files = fileUpload.files;
        if (files.length <= 0) {
            toastr.warning("Please Select a File !");
            return false;
        }
        var name = files[0].name;
        var regex = new RegExp("(.*?)\.(xlsx|xls|csv)$");
        if (!(regex.test(name))) {
            $('#DocFile').val('');
            toastr.warning('Please select correct file format');
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
                            $scope.GetDocInfo(response.fileName, response.physicalPath);
                        } else {
                            toastr.warning("Upload Failed!");
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
    $scope.GetDocInfo = function (fileName, physicalPath) {

        $http({
            method: "POST",
            url: MyApp.rootPath + "DataUploadPrescriptionTarget/LoadExcelFile",
            params: { fileName: fileName, physicalPath: physicalPath }
        }).then(function (response) {
            if (response.data.Status === null || response.data.Status === undefined) {
                if (response.data.length > 0) {
                    $scope.gridDataUploadPrescriptionTargetOptionsValue.data = response.data;
                    //$scope.gridDataUploadPrescriptionTargetOptionsValue.columnDefs[10].visible = true;
                } else {
                    toastr.warning("No Data Found!");
                }
            } else {
                toastr.warning(response.data.Status, { timeOut: 2000 });
            }
        }, function () {
            toastr.warning("Error Occurred");
        });
    }

    //save data from grid

    $scope.SaveData = function () {
        var prescriptionData = $scope.gridDataUploadPrescriptionTargetOptionsValue.data;
        if (!prescriptionData.length > 0) {
            toastr.warning('Please Insert Data First');
            return false;
        }
        var prescriptionDataLength = prescriptionData.length - 1;
        $scope.SaveDataFunction(prescriptionDataLength, prescriptionData);
    };
    $scope.SaveDataFunction = function (prescriptionDataLength, prescriptionData) {
        var n = parseInt((prescriptionData.length - 1) / maxValue);
        if (prescriptionDataLength > maxValue) {
            var j = prescriptionDataLength - maxValue;
        } else {
            var j = 0;
        }
        var prescriptionDataK = prescriptionData.slice(j, prescriptionDataLength);
        $http({
            method: "post",
            url: MyApp.rootPath + "DataUploadPrescriptionTarget/OperationsMode",
            datatype: "json",
            data: { model: prescriptionDataK, MonthNumber: $scope.frmDataUploadPrescriptionTarget.MonthNumber, Year: $scope.Year }
        }).then(function (response) {
            if (response.data.Status === 'Yes') {
                if (i >= n) {
                    toastr.success('Updated Successfully', 'Success Alert', { timeOut: 2000 });
                } else {
                    prescriptionDataLength = j - 1;
                    $scope.SaveDataFunction(prescriptionDataLength, prescriptionData);
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
    $scope.GetPrescriptionTargetData = function () {
        $scope.Reset();
        $http({
            method: "POST",
            url: MyApp.rootPath + "DataUploadPrescriptionTarget/GetPrescriptionTargetData",
            data: {MonthNumber: $scope.frmDataUploadPrescriptionTarget.MonthNumber, Year: $scope.Year }
        }).then(function (response) {
            if (response.data.Status === null || response.data.Status === undefined) {
                if (response.data.length > 0) {
                    $scope.gridDataUploadPrescriptionTargetOptionsValue.data = response.data;
                    //$scope.gridDataUploadPrescriptionTargetOptionsValue.columnDefs[10].visible = false;
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
        $scope.frmDataUploadPrescriptionTarget.MonthNumber = month;
        $scope.Year = year;
        var fileElement = angular.element('#DocFile');
        angular.element(fileElement).val(null);
        $scope.gridDataUploadPrescriptionTargetOptionsValue.data = [];

    };
});

