﻿function ShiftAssignmentViewModel() {
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
        //  //debugger;
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
        // //debugger;
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
        ApplyCustomBinding('shiftassignment');

    }
    self.DeleteShift = function (tableItem) {
        //var message = confirm("Are you sure, you want to delete selected record!");
        //if (message == true) {
        //    AjaxCall('/Api/ShiftAssignment/DeleteShift', tableItem, 'POST', function () {
        //        toastr.success('ShiftAssignment deleted successfully!!')
        //        ApplyCustomBinding('shiftassignment');
        //    });
        //}
        recordToDelete = tableItem;
    }

    self.DeleteConfirmed = function () {
        $('#myModal').modal('hide');
        $('.modal-backdrop').modal('show');
        $('.modal-backdrop').modal('hide');
        AjaxCall('/Api/ShiftAssignment/DeleteShift', recordToDelete, 'POST', function () {
            toastr.success('Shift deleted successfully!!')
            ApplyCustomBinding('shiftassignment');
            });
        //    if (data.Success == true) {
        //        toastr.success(data.Message);
        //        ApplyCustomBinding('shiftassignment');
        //    }
        //    else if (data.Success == false) {
        //        toastr.warning(data.Message);
        //    }
        //});
    }
    self.EditShift = function (tableItem) {

        var datetoFormat = new Date(tableItem.ToDate),
            month = '' + (datetoFormat.getMonth() + 1),
            day = '' + datetoFormat.getDate(),
            year = '' + datetoFormat.getFullYear();
        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;
        var toDate = [day, month, year].join('/');


        var datefromDate = new Date(tableItem.FromDate);
        month = '' + (datefromDate.getMonth() + 1),
        day = '' + datefromDate.getDate(),
        year = datefromDate.getFullYear();
        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;
        var fromDate = [day, month, year].join('/')

        // alert(tableItem.UserId);
        ////debugger;
        if (tableItem != undefined) {
            self.Id(tableItem.Id);
            self.BuildingId(tableItem.BuildingId);
            gateId = tableItem.GateId;
            self.ShitfId(tableItem.ShitfId);
            userId = tableItem.UserId;
            //self.FromDate(tableItem.FromDate);
            //self.ToDate(tableItem.ToDate);
            
            self.FromDate(fromDate);
            self.ToDate(toDate);
            $("#btnSaveshiftassignment").text("Update");
        }
    }
    SaveShiftAssignment = function () {
        //debugger;
        //if (self.FromDate() == "" && $('#dateFrom').val() != "") {
        //    // self.FromDate($('#dateFrom').val());
        //    self.FromDate($('#dateFrom').val().split('/')[1] + '/' + $('#dateFrom').val().split('/')[0] + '/' + $('#dateFrom').val().split('/')[2]);
        //}

        //if (self.ToDate() == "" && $('#dateTo').val() != "") {
        //    //self.ToDate($('#dateTo').val());
        //    self.ToDate($('#dateTo').val().split('/')[1] + '/' + $('#dateTo').val().split('/')[0] + '/' + $('#dateTo').val().split('/')[2]);
        //}

 
        var initial = $('#dateFrom').val().split(/\//);
        self.FromDate([initial[1], initial[0], initial[2]].join('/'));
 
        var initial1 = $('#dateTo').val().split(/\//);
        self.ToDate([initial1[1], initial1[0], initial1[2]].join('/'));



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
            data.FromDate = self.FromDate();
            data.ToDate = self.ToDate();


            AjaxCall('/Api/ShiftAssignment/SaveShiftAssignment', data, 'POST', function () {
                toastr.success('ShiftAssignment saved successfully!!')
                ApplyCustomBinding('shiftassignment');
            })
        }
    }
    self.GlobalSearchEnter = function (data) {
        self.GetAllBuildingData();
        console.log(event);
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