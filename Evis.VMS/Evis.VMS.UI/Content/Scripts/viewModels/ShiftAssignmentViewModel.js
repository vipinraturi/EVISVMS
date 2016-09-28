var gblDateToTest;

function ShiftAssignmentViewModel() {
    var self = this;
    var gateId = 0;
    var userId = 0;
    ko.validation.rules.pattern.message = 'Invalid.';
    ko.validation.configure({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null,
        decorateElement: true,
        errorElementClass: 'err'
    });
    self.Id = ko.observable(0);

    self.ShitfId = ko.observable(undefined).extend({ required: true });
    self.UserId = ko.observable(undefined).extend({ required: true });
    self.BuildingId = ko.observable(undefined).extend({ required: true });
    self.GateId = ko.observable(undefined).extend({ required: true });
    self.FromDate = ko.observable('').extend({ required: true });
    self.ToDate = ko.observable('').extend({ required: true });
    self.City = ko.observable(undefined).extend({ required: true });
    self.strFromDate = ko.observable('');
    self.strToDate = ko.observable('');


    self.errors = ko.validation.group(
      {
          BuildingId: this.BuildingId,
          GateId: this.GateId,
          UserId: this.UserId,
          ShitfId: this.ShitfId,
          FromDate: this.FromDate,
          ToDate: this.ToDate

      });
    self.GlobalSearch = ko.observable('');

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/ShiftAssignment/GetAllShiftAssignment', 7);


    self.GetAllShiftAssignmentData = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }
    self.Gates = ko.observableArray();
    self.GetGates = function () {
        //  
        if (self.BuildingId() != undefined && self.BuildingId() != 0) {
            AjaxCall('/Api/ShiftAssignment/GetAllGates?BuildingId=' + self.BuildingId(), null, 'GET', function (data) {
                self.Gates(new Object());
                self.Gates(data);
                self.GateId(gateId);

            })
        }
    }
    self.Shift = ko.observableArray();
    AjaxCall('/Api/ShiftAssignment/GetAllShift', null, 'GET', function (data) {
        self.Shift(data);
    })
    self.Buildings = ko.observableArray();
    AjaxCall('/Api/Gates/GetAllBuilding', null, 'GET', function (data) {
        
        self.Buildings(data);
    })
    self.Users = ko.observableArray();
    self.GetUsers = function () {
        // 
        if (self.GateId() != undefined && self.GateId() != 0) {
            AjaxCall('/Api/ShiftAssignment/GetAllUsers?GateId=' + self.GateId(), null, 'GET', function (data) {
                self.Users(new Object());
                self.Users(data);
                self.UserId(userId);
            })
        }
    }

    self.ResetShiftAssignment = function () {
        self.GlobalSearch('');
        self.ShitfId('');
        self.UserId('');
        self.BuildingId('');
        self.GateId('');
        $('#SecurityPerson').attr('disabled', true);
        ApplyCustomBinding('shiftassignment');

    }
    self.DeleteShift = function (tableItem) {
        recordToDelete = tableItem;
    }
    self.DeleteConfirmed = function () {
        
        $('#myModal').modal('hide');
        $('.modal-backdrop').modal('show');
        $('.modal-backdrop').modal('hide');
        AjaxCall('/Api/ShiftAssignment/DeleteShift', recordToDelete, 'POST', function () {
            $(".modal-backdrop").hide();
            toastr.clear();
            toastr.success('Shift deleted successfully!!')
            ApplyCustomBinding('shiftassignment');
        });
    }
    self.EditShift = function (tableItem) {
        gblDateToTest = tableItem.ToDate;
        //console.log('tableItem.ToDate ' + tableItem.ToDate + 'tableItem.FromDate  ' + tableItem.FromDate);

        //var datetoFormat = new Date(tableItem.ToDate),
        //    month = '' + (datetoFormat.getMonth() + 1),
        //    day = '' + datetoFormat.getDate(),
        //    year = '' + datetoFormat.getFullYear();


        //if (month.length < 2) month = '0' + month;
        //if (day.length < 2) day = '0' + day;
        //var toDate = [day, month, year].join('/');


        //var datefromDate = new Date(tableItem.FromDate);
        //month = '' + (datefromDate.getMonth() + 1),
        //day = '' + datefromDate.getDate(),
        //year = datefromDate.getFullYear();
        //if (month.length < 2) month = '0' + month;
        //if (day.length < 2) day = '0' + day;
        //var fromDate = [day, month, year].join('/');
        $('#SecurityPerson').attr('disabled', true);
        
        var from = tableItem.FromDate.slice(0, 10).split('-');
        var fromdate = from[2] + '/' + from[1] + '/' + from[0];
        var TO = tableItem.ToDate.slice(0, 10).split('-');
        var TODate = TO[2] + '/' + TO[1] + '/' + TO[0];

        if (tableItem != undefined) {
            self.Id(tableItem.Id);
            self.BuildingId(tableItem.BuildingId);
            gateId = tableItem.GateId;
            self.ShitfId(tableItem.ShitfId);
            self.City(tableItem.City);
            userId = tableItem.UserId;
            self.UserId(tableItem.UserId);
            self.strFromDate(fromdate);
            self.strToDate(TODate);
            self.FromDate(fromdate);
            self.ToDate(TODate);
            //alert(fromDate);
            //alert(toDate);
            $("#btnSaveshiftassignment").text("Update");
        }
    }
    SaveShiftAssignment = function () {

        //var initial = $('#dateFrom').val().split(/\//);
        //self.FromDate([initial[0], initial[1], initial[2]].join('/'));

        //var initial1 = $('#dateTo').val().split(/\//);
        //self.ToDate([initial1[0], initial1[1], initial1[2]].join('/'));



        var from = $('#dateFrom').val().slice(0, 10).split('-');
        var fromdate = from[2] + '/' + from[1] + '/' + from[0];
        self.FromDate(from);
        var TO = $('#dateTo').val().slice(0, 10).split('-');
        var TODate = TO[2] + '/' + TO[1] + '/' + TO[0];
        self.ToDate(TO);
        

        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
            });
        }
        else {
            var data = new Object();
            data.Id = self.Id();
            data.ShitfId = self.ShitfId();
            data.UserId = self.UserId();
            data.BuildingId = self.BuildingId();
            data.GateId = self.GateId();
            data.FromDate = $('#dateFrom').val();
            data.ToDate = $('#dateTo').val();
            data.strFromDate = $('#dateFrom').val();
            data.strToDate = $('#dateTo').val();
            //console.log('self.FromDate()' + self.FromDate() + '  self.ToDate()' + self.ToDate());

            AjaxCall('/Api/ShiftAssignment/SaveShiftAssignment', data, 'POST', function (data) {
                
                if (data.Success == true) {
                    toastr.clear();
                    toastr.success(data.Message);
                    ApplyCustomBinding('shiftassignment');
                }
                else {
                    toastr.clear();
                    toastr.error('shift already assigned!')
                }
            })

        }
    }
    self.GlobalSearchEnter = function (data) {
        self.GetAllShiftAssignmentData();
        //console.log(event);
    }
    ko.bindingHandlers.enterkey = {
        init: function (element, valueAccessor, allBindings, viewModel) {
            var callback = valueAccessor();
            $(element).keypress(function (event) {
                var keyCode = (event.which ? event.which : event.keyCode);
                if (keyCode === 13) {
                    callback.call(viewModel);
                    return false;
                }
                return true;
            });
        }
    };
    self.GetAllShiftAssignmentData();
}