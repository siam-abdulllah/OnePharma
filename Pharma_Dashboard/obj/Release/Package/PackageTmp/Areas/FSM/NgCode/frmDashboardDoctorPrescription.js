app.controller("myCtrl", function ($scope, $http, $interval, uiGridConstants) {
    //$scope.EventPerm(1);
 
    var dashboardData = '';
    var barLevel = [];
    var barData = [];
    var barBackgroundColor = [];


   


    $scope.GetDashboardData = function () {
        $http({
            method: "post",
            url: MyApp.rootPath + "DashboardDoctorPrescription/GetDoctorPrescriptionData",
            datatype: "json",
        }).then(function (response) {
            if (response.data.Status == 'Ok') {

                $scope.dashboardData = response.data.Data;




            } else {
                toastr.error("No Data Found!", '');
            }
        });
    };
    $scope.GetDashboardData();

    $http({
        method: "post",
        url: MyApp.rootPath + "DashboardDoctorPrescription/GetGridData",
        datatype: "json"
    }).then(function (response) {
      
        if (response.data.length>0) {
  
            $scope.DoctorPrescriptionGridValue.data = response.data;

        }
        else {
            toastr.error("No Data Found!", '');
        }
    });



    var DataSourceDeclaration = [

    { name: "DoctorCode", displayName: "DoctorCode", visible:false },
    { name: "DoctorName", displayName: "Doctor Name" },
    { name: "DailyProduct", displayName: "Daily Product" },
    { name: "LastDayPrescription", displayName: "LastDay Prescription" },
    { name: "MonthlyProduct", displayName: "Monthly Product" },
    { name: "Total", displayName: "Total" },
    { name: "Achievement", displayName: "Ach(%)" },    
    { name: "HonorariumAmt", displayName: "Honorarium Amt" },
    { name: "TerriroryName", displayName: "Terrirory Name" },
     { name: "AreaName", displayName: "Area Name" }
    ];


    $scope.DoctorPrescriptionGridValue = {
        //showGridFooter: true,
        //showColumnFooter: true,
        enableFiltering: true,
        enableSorting: true,
        columnDefs: DataSourceDeclaration,
        //rowTemplate: rowTemplate(),
        enableGridMenu: true,
        enableSelectAll: true,
        exporterCsvFilename: 'DoctorPrescription.csv',
        exporterMenuPdf: false,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }

    };
});