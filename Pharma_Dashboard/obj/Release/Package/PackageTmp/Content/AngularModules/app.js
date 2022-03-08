var app = angular.module("myApp", ['ngTouch', 'ui.grid.selection', 'ui.grid.autoResize', 'ui.grid', 'ui.grid.cellNav', 'ui.grid.edit', 'ui.grid.rowEdit', 'ui.grid.grouping', 'ui.grid.pinning', 'ui.grid.exporter', 'ui.grid.resizeColumns', 'ui.grid.exporter', 'ui.grid.pagination', 'ui.grid.moveColumns', 'ngSanitize', 'ui.grid.pagination', 'ui.select', 'ui.grid.pinning']);

app.run(function ($rootScope, $http, $window) {
    //var pathname = window.location.pathname; // Returns path only
    //var url = window.location.href;
    //alert(pathname);
    //alert(url);
    $rootScope.ActiveSts = "Active";

    $rootScope.ViewPerm = "";
    $rootScope.SearchPerm = "";
    $rootScope.FormTitle = "";
    $rootScope.MenuName = "";
    $rootScope.NumberPattern = "/^[0-9]+(\.[0-9]{1,2})?$/";
    //$rootScope.NumberPattern = "/^[0-9]*(\.{1})?([0-91-9][1-9])?$/";
    $rootScope.EmailFormat = "/^[a-z]+[a-z0-9._]+@[a-z]+\.[a-z.]{2,5}$/";

    $rootScope.EventPerm = function (smID) {
        $http({
            method: "Post",
            url: MyApp.rootPath + "Home/EventPermission",
            datatype: "json",
            data: { smID: smID }
        }).then(function (res) {
            if (res.data.Data.length !== 0) {
                $rootScope.ViewPerm = (res.data.Data[0].SV);
                $rootScope.SearchPerm = (res.data.Data[0].DL);
                $rootScope.FormTitle = (res.data.Data[0].SM_NAME);
                $rootScope.MenuName = (res.data.Data[0].MH_NAME);
            } else {
                //if (res.data.role_name === "FSM") {
                //    $window.location.href = '/FSM/DashboardNationalPrescription/frmDashboardNationalPrescription';
                //} else {
                //    $window.location.href = '/Dashboard/HomeDashboard/frmHomeDashboard';
                //}
                $window.location.href = '/Home/Index';
            }
        });
    };
    
    $rootScope.ResetForm = function () {
        $('input[type="hidden"]').val("");
        $('input[type="text"]').val("");
        $("textarea").val("");
        $('input[type="checkbox"]:checked').prop('checked', false);
        $("select").prop('selectedIndex', 0);
    };
    
});
app.filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }
                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }
        return out;
    };
});
app.filter('FullDateTime', function () {
    return function (value) {
        if (value === "ALL") { return value; }
        if (!value) { return ''; }

        var dt = new Date(parseInt(value.substr(6)));
        var month = ("0" + (dt.getMonth() + 1)).slice(-2);
        var day = ("0" + dt.getDate()).slice(-2);
        var year = dt.getFullYear();
        var hours = dt.getHours();
        var minutes = dt.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var dtpDepEfct = day + '/' + month + '/' + dt.getFullYear();
        return dtpDepEfct;
    }
});
app.factory('Session', function ($http) {
    var Session = {
        data: {},
        saveSession: function () { /* save session data to db */ },
        updateSession: function () {
            /* load data from db */
            $http.get('session.json')
                .then(function (r) { return Session.data = r.data; });
        }
    };
    Session.updateSession();
    return Session;
});
app.directive('loading', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };
            scope.$watch(scope.isLoading,
                function (value) {
                    if (value) {
                        element.removeClass('ng-hide');
                        //element.parent().addClass('blur');
                        $(".form-horizontal").addClass('blur');
                    } else {
                        element.addClass('ng-hide');
                        $(".form-horizontal").removeClass('blur');
                    }
                });
        }
    };
}]);
app.directive('tooltip', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.hover(function () {
                // on mouseenter
                element.tooltip('show');
            }, function () {
                // on mouseleave
                element.tooltip('hide');
            });
        }
    };
});
app.filter('fractionFilter', function () {
    return function (value) {
        return value.toFixed(0);
    };
})
function OperationMsg(mode) {

    if (mode === "I") {
        toastr.success("Saved Successfully!", '');
    }
    else if (mode === "U") {
        toastr.success('Updated Successfully!', '');
    }
    else if (mode === "No") {
        toastr.error('Not Saved!', '');
    }
    else if (mode === "D") {
        toastr.success('Deleted Successfully!', '');
    }
    else if (mode === "NoDel") {
        toastr.error('Not Deleted!', '');
    }
    else if (mode === "Unique") {
        toastr.error("Data Exists!", '');

    }
    else if (mode === "C") {
        toastr.success("Checked Successfully!", '');
    }
    else if (mode === "A") {
        toastr.success("Approved Successfully!", '');
    }
}
function CompareDate(fromDate, toDate, checkingMode) {
    //var fDate = new Date(fromDate);
   // var tDate = new Date(toDate);


    var startDate = new Date(fromDate.split("/").reverse().join("-"));
    startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate());

    var sDate = (startDate.getMonth() + 1) + '/' + startDate.getDate() + '/' + startDate.getFullYear();
    var endDate = new Date(toDate.split("/").reverse().join("-"));
    endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
    var eDate = (endDate.getMonth() + 1) + '/' + endDate.getDate() + '/' + endDate.getFullYear();
    var todayDate = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
    var tDate = (todayDate.getMonth() + 1) + '/' + todayDate.getDate() + '/' + todayDate.getFullYear();
    
    if (checkingMode === "greater") {
        if (startDate > endDate) {
            toastr.warning("Start Date Cannot be Greater Than End Date !");
            return false;
        }
        if (endDate > todayDate) {
            toastr.warning("End Date Cannot be Greater Than Present Date !");
            return false;
        }
    }if (checkingMode === "onlygreater") {
        if (startDate > endDate) {
            toastr.warning("Start Date Cannot be Greater Than End Date !");
            return false;
        }
        
    }
    if (checkingMode === "less") {
        if (startDate > endDate) {
            toastr.warning("Start Date Cannot be Greater Than End Date !");
            return false;
        }
        if (endDate > todayDate) {
            toastr.warning("End Date Cannot be Greater Than Present Date !");
            return false;
        }
    }
   

    return true;
}
function DateCheck(fromDate, toDate) {

    var todayDate = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
    var startDate = fromDate.split("/");
    var convertedStartDate = new Date(+startDate[2], startDate[1] - 1, +startDate[0]); 
    var sDate = new Date(convertedStartDate.getFullYear(), convertedStartDate.getMonth(), convertedStartDate.getDate());
    if (toDate === 'null') {
        if (sDate > todayDate) {
            toastr.warning("Date  Cannot be Greater Than Current Date !");
            return false;
        }
        return true;
    }

    var endDate = toDate.split("/");
    var convertedEndDate = new Date(+endDate[2], endDate[1] - 1, +endDate[0]);
    var eDate = new Date(convertedEndDate.getFullYear(), convertedEndDate.getMonth(), convertedEndDate.getDate());
    if (parseInt(endDate[1], 10) !== parseInt(startDate[1], 10)) {
        toastr.warning("You cannot select different month!");
        return false;
    }
   
    
    var sMonth = new Date(convertedStartDate.getMonth());
    var eMonth = new Date(convertedEndDate.getMonth());
    
    if (eDate > todayDate) {
        toastr.warning("To Date  Cannot be Greater Than Current Date !");
        return false;
    }

    if (sDate > eDate) {
        toastr.warning("From Date  Cannot be Greater Than To Date !");
        return false;
    }
    return true;
}
 