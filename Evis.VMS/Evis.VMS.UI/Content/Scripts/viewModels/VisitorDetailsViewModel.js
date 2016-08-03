﻿function VisitorDetailsViewModel() {
    var self = this;
    var gateId = 0;
    var userId = 0;
    self.BuildingId = ko.observable(undefined).extend({ required: false });
    self.UserId = ko.observable(undefined).extend({ required: false });
    self.GateId = ko.observable(undefined).extend({ required: false });
    self.VisitorName = ko.observable('').extend({ required: false });
    self.FromDate = ko.observable('').extend({ required: false });
    self.ToDate = ko.observable('').extend({ required: false });

    debugger;
    self.DataGrid = new RIT.eW.DataGridAjax('/Api/VisitorsDetails/GetVisitorsDetails', 7);
    self.SearchVisitorsDetails = function () {
        //self.DataGrid.UpdateSearchParam('?search=' + self.Search());
        debugger;
        self.DataGrid.UpdateSearchParam('?searchDetails=' + new Object());
        self.DataGrid.GetData();
    }

    // To get all buildings.
    self.Buildings = ko.observableArray();
    AjaxCall('/Api/Gates/GetAllBuilding', null, 'GET', function (data) {
        self.Buildings(data);
    });

    // To get all building's gates.
    self.Gates = ko.observableArray();
    self.GetGates = function () {
        if (self.BuildingId() != undefined && self.BuildingId() != 0) {
            AjaxCall('/Api/ShiftAssignment/GetAllGates?BuildingId=' + self.BuildingId(), null, 'GET', function (data) {
                self.Gates(new Object());
                self.Gates(data);
                self.GateId(gateId);

            })
        }
    }

    // To get all security persons.
    self.Users = ko.observableArray();
    self.GetUsers = function () {
        if (self.GateId() != undefined && self.GateId() != 0) {
            AjaxCall('/Api/ShiftAssignment/GetAllUsers?GateId=' + self.GateId(), null, 'GET', function (data) {
                self.Users(new Object());
                self.Users(data);
                self.UserId(userId);
            })
        }
    }

    self.SearchVisitorsDetails = function () {
        var data = new Object();
        data.BuildingId = self.BuildingId();
        data.GateId = self.GateId();
        data.UserId = self.UserId();
        data.VisitorName = self.VisitorName();
        data.FromDate = self.FromDate();
        data.ToDate = self.ToDate();

        self.DataGrid.UpdateSearchParam('?searchDetails=' + data);
        self.DataGrid.GetData();

        //AjaxCall('/Api/VisitorsDetails/GetVisitorsDetails', data, 'POST', function () {
        //    toastr.success('ShiftAssignment saved successfully!!')
        //    ApplyCustomBinding('shiftassignment');
        //})
    }

    self.ResetVisitorsDetails = function () {
        self.BuildingId(undefined);
        self.GateId(undefined);
        self.UserId(undefined);
        self.VisitorName('');
        self.FromDate('');
        self.ToDate('');
        ApplyCustomBinding('visitordetailsreport');
    }
}