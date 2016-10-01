var glbData1 = [];
var glbData2Unique = [];

var todayDate = GetTodayDate();


function ShiftManagementdynamicViewModell() {
    var self = this;
    self.Buildings = ko.observableArray();
    self.Gates = ko.observableArray();
    self.Header = ko.observableArray();
    self.Body = ko.observableArray();
    self.BuildingId = ko.observable(-1);
    self.GateId = ko.observable(-1);
    self.NoOfDays = ko.observable(7);

    //GetShiftDetails(todayDate);


    self.GeShiftsData = function () {
        GetShiftDetails(todayDate);
    }

    AjaxCall('/Api/Gates/GetAllBuilding', null, 'GET', function (data) {
        self.Buildings(data);
    })

    self.GetGates = function () {
        if (self.BuildingId() != undefined && self.BuildingId() != 0) {
            AjaxCall('/Api/ShiftAssignment/GetAllGates?BuildingId=' + self.BuildingId(), null, 'GET', function (data) {
                self.Gates(new Object());
                self.Gates(data);
            })
        }
    }


    self.GetUsers = function () {
    }

    self.ChangeShift = function () {
        if (gblTableId != '' && gblTableId != undefined) {
            if ($('#' + gblTableId).html().indexOf('fa-check-unique') != -1) {
                $('#' + gblTableId).html('');
            }
            else {
                $('#' + gblTableId).html(' <i class="fa fa-check fa-check-unique" aria-hidden="true"></i>')
            }

            $('#myModal').modal('hide');
            toastr.success('Shift changed successfully!!');
        }

    }


    self.DayClick = function () {
        SetSelectedClass(1);
        self.NoOfDays = ko.observable(1);
        GetShiftDetails(todayDate);
    }
    self.WeekClick = function () {
        SetSelectedClass(7);
        self.NoOfDays = ko.observable(7);
        GetShiftDetails(todayDate);

    }
    self.TwoWeekClick = function () {
        SetSelectedClass(14);
        self.NoOfDays = ko.observable(14);
        GetShiftDetails(todayDate);

    }

    self.FourWeekClick = function () {
        SetSelectedClass(31);
        self.NoOfDays = ko.observable(31);
        GetShiftDetails(todayDate);

    }

    self.PrevClick = function () {
        $('#txtShiftDate').val(todayDate);
        GetShiftDetails(todayDate);
    }

    self.TodayClick = function () {
        $('#txtShiftDate').val(todayDate);
        GetShiftDetails(todayDate);
    }

    self.NextClick = function () {
        $('#txtShiftDate').val(todayDate);
        GetShiftDetails(todayDate);
    }

}


function SetSelectedClass(dayType) {
    $('#tls_week').removeClass('selected');
    $('#tls_day').removeClass('selected');
    $('#tls_twoweek').removeClass('selected');
    $('#tls_month').removeClass('selected');

    switch (dayType) {
        case 1:
            $('#tls_day').addClass('selected');
            break;
        case 7:
            $('#tls_week').addClass('selected');

            break;
        case 14:
            $('#tls_twoweek').addClass('selected');

            break;
        case 31:
            $('#tls_month').addClass('selected');

            break;
        default:

    }

}

function GetShiftDetails(defaultDate) {
    var request = new Object();
    request.BuildingId = self.BuildingId();
    request.GateId = self.GateId();

    if (defaultDate != undefined) {
        request.ShiftDate = defaultDate;
    }


    if ($('#txtShiftDate').val() != "" && $('#txtShiftDate').val() != undefined) {
        request.ShiftDate = $('#txtShiftDate').val();
    }

    request.NoOfDays = self.NoOfDays();

    AjaxCall('/Api/ShiftAssignment/GetAllShift', request, 'POST', function (data) {
        self.Header(data.Header);
        glbData1 = self.Header();
        self.Body(data.Body);
        glbData2Unique = self.Body();
    });
}

