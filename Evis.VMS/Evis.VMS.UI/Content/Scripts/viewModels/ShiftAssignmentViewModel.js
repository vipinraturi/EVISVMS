﻿function ShiftAssignmentViewModel() {
    var self = this;
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
    // self.errors = ko.validation.group(self);
    // self.Id = ko.observable(0);

    self.Id = ko.observable(0);
    self.ShitfId = ko.observable(undefined).extend({ required: true });
    self.UserId = ko.observable(undefined).extend({ required: true });
    self.BuildingId = ko.observable(undefined).extend({ required: true });
    self.GateId = ko.observable(undefined).extend({ required: true });
    self.FromDate = ko.observable('').extend({ required: true });
    self.ToDate = ko.observable('').extend({ required: true });

    //self.BuildingName = ko.observable('').extend({ required: true });
    //self.GateNumber = ko.observable('').extend({ required: true });
    //self.ShitfName = ko.observable('').extend({ required: true });
    //self.UserName = ko.observable('').extend({ required: true });
    self.errors = ko.validation.group(
      {
          UserId: this.UserId,
          ShitfId: this.ShitfId,
          BuildingId: this.BuildingId,
          GateId: this.GateId,
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
    AjaxCall('/Api/ShiftAssignment/GetAllGates', null, 'GET', function (data) {

        self.Gates(data);

    })
    self.Shift = ko.observableArray();
    AjaxCall('/Api/ShiftAssignment/GetAllShift', null, 'GET', function (data) {
        self.Shift(data);
    })
    self.Buildings = ko.observableArray();
    AjaxCall('/Api/Gates/GetAllBuilding', null, 'GET', function (data) {
        //debugger;;
        self.Buildings(data);
    })
    self.Users = ko.observableArray();
    AjaxCall('/Api/ShiftAssignment/GetAllUsers', null, 'GET', function (data) {
        //debugger;;
        self.Users(data);
    })
    self.ResetShiftAssignment = function () {
        self.GlobalSearch('');
        self.ShitfId('');
        self.UserId('');
        self.BuildingId('');
        self.GateId('');
        ApplyCustomBinding('shiftassignment');

    }
    self.DeleteShift = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected record!");
        if (message == true) {
            debugger;
            AjaxCall('/Api/ShiftAssignment/DeleteShift', tableItem, 'POST', function () {
                toastr.success('ShiftAssignment deleted successfully!!')
                ApplyCustomBinding('shiftassignment');
            });
        }
    }
    self.EditShift = function (tableItem) {
        debugger;
        if (tableItem != undefined) {
            self.Id(tableItem.Id);
            self.BuildingId(tableItem.BuildingId);
            self.GateId(tableItem.GateId);
            self.ShitfId(tableItem.ShiftId);
            self.UserId(tableItem.UserId);
            self.FromDate(tableItem.FromDate);
            self.ToDate(tableItem.ToDate);
            //stateId = (tableItem.ToDate);
            //cityId = (tableItem.CityId);
            //self.IsInsert(false);
            //data.CityId = self.CityId();

        }
    }
    SaveShiftAssignment = function () {
        debugger;

        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                //toastr.warning(data);
            });
        }
        else {
            var data = new Object();
            data.Id = self.Id(),
            data.ShitfId = self.ShitfId(),
            data.UserId = self.UserId(),
            data.BuildingId = self.BuildingId(),
            data.GateId = self.GateId(),
            data.FromDate = self.FromDate(),
            data.ToDate = self.ToDate()

            //data.State = self.State(),
            // data.Country = self.Country(),
            //  data.CityId = self.CityId(),
            //// display any error messages if we have them
            AjaxCall('/Api/ShiftAssignment/SaveShiftAssignment', data, 'POST', function () {
                toastr.success('building saved successfully!!')
                ApplyCustomBinding('shiftassignment');
                //  self.IsInsert(true);

            })
        }
    }
    self.GetAllShiftAssignmentData();
}