var glbData1 = [];
var glbData2Unique = [];
var todayDate = GetTodayDate();
var shiftAssinedLst = [];

function ShiftManagementdynamicViewModell() {
    var self = this;
    self.Buildings = ko.observableArray();
    self.Gates = ko.observableArray();
    self.Header = ko.observableArray();
    self.Body = ko.observableArray();
    self.BuildingId = ko.observable(-1);
    self.GateId = ko.observable(-1);
    self.NoOfDays = ko.observable(7);
    shiftAssinedLst = [];

    //GetShiftDetails(todayDate);

    self.ChangeShiftAssignment = function (data) {
        var checked = true;
        var isExist = false;

        for (var i = 0; i < shiftAssinedLst.length; i++) {
            if (shiftAssinedLst[i].Id == data.Id) {
                {
                    isExist = true;
                    shiftAssinedLst.splice(i, 1);
                }
            }
        }

        if ($('#shiftCell_' + data.Id).html().indexOf('fa-check-unique') != -1) {
            $('#shiftCell_' + data.Id).html('');
            checked = false;
        }
        else {
            checked = true;
            $('#shiftCell_' + data.Id).html(' <i class="fa fa-check fa-check-unique" aria-hidden="true"></i>')
        }

        if (!isExist) {
            var shiftAssinedObj = new Object();
            shiftAssinedObj.Id = data.Id;
            shiftAssinedObj.ShiftId = data.ShiftId;
            shiftAssinedObj.IsAssigned = checked;
            shiftAssinedObj.ShiftDate = data.ShiftDate;
            shiftAssinedObj.ShiftName = data.ShiftName;
            shiftAssinedObj.UserId = data.UserId;
            shiftAssinedObj.GateId = data.GateId;
            shiftAssinedLst.push(shiftAssinedObj);
        }
        //debugger;
        //console.log('click..' + data.Id + '  ' + data.IsAssigned + '  ' + data.ShiftDate + '  ' + data.ShiftName + '  ' + data.UserId);
    }

    self.ApplyChanges = function (data) {
        //shiftAssinedLst
        AjaxCall('/Api/ShiftAssignment/ApplyShiftAssignmentChanges', shiftAssinedLst, 'POST', function (data) {
            //debugger;
            shiftAssinedLst = [];
            if (data.Success) {
                toastr.success('Shift changed successfully!!')
            }
        });
    }

    self.ResetChanges = function (data) {
        ApplyCustomBinding('shiftmanagementdynamic');
    }

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

