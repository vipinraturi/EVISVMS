function ShiftDetailsViewModel() {
    var self = this;
    var organisationId = ko.observable(undefined);
    self.BuildingId = ko.observable(undefined);
    self.GateId = ko.observable(undefined);
    self.SecurityId = ko.observable(undefined);
    self.ShiftId = ko.observableArray(undefined);
    self.FromDate = ko.observable(undefined);
    self.ToDate = ko.observable(undefined);


    self.GetOrganization = function () {
        AjaxCall('/Api/Report/Getorganisation', null, 'GET', function (data) {
            self.organisationId = (data.OrganizationId);
        });
    }
    GetOrganization();


    self.GetAllShiftAssignmentData = function () {
        self.DataGrid = new RIT.eW.DataGridAjax('/Api/Report/GetShiftDetailsGrid', 7);
        self.DataGrid.UpdateSearchParam('?search=' + JSON.stringify(new Object()));
        self.DataGrid.GetData();
    }
    GetAllShiftAssignmentData();


    self.Security = ko.observableArray();
    self.GetUsers = function () {
        AjaxCall('/Api/Report/GetUsers', null, 'GET', function (data) {
            self.Security(data);

        });
    }
    GetUsers();

    //To get the buildings
    self.Buildings = ko.observableArray();
    self.GetBuildings = function () {
        
        AjaxCall('/Api/Report/GetBuildings?id=' + self.organisationId, null, 'GET', function (data) {
            self.Buildings(data);
        });
    }
    GetBuildings();


    //To get the Gates
    self.Gates = ko.observableArray();
    self.GetGates = function () {
        //To get the gates based on the building selected
        if (self.BuildingId() != undefined && self.BuildingId() != 0) {
            AjaxCall('/Api/ShiftAssignment/GetAllGates?BuildingId=' + self.BuildingId(), null, 'GET', function (data) {
                self.Gates(new Object());
                self.Gates(data);
                self.GateId(Id);

            });
        }
        else {

            AjaxCall('/Api/Report/GetGates?id=' + self.organisationId, null, 'GET', function (data) {
                self.Gates(new Object());
                self.Gates(data);
                self.GateId(GateId);
            });
        }
    }
    GetGates();
    self.shift = ko.observableArray();
    self.GetShiftname = function () {
        AjaxCall('/Api/Report/GetShifts', null, 'GET', function (data) {
            self.shift(data);

        });
    }
    GetShiftname();

    self.SearchDetails = function () {
        
        var data = new Object();
        data.BuildingId = self.BuildingId();
        data.GateId = self.GateId();
        data.SecurityId = self.SecurityId();
        data.ShiftID = self.ShiftId();
        data.FromDate = $('#txtFromDate').val();
        data.ToDate = $('#txtToDate').val();

        self.DataGrid.UpdateSearchParam('?search=' + JSON.stringify(data));
        self.DataGrid.GetData();
    }
    self.ResetDetails = function () {
        
        $('#txtFromDate').val('');
        $('#txtToDate').val('');
        self.BuildingId(undefined);
        self.GateId(undefined);
        self.SecurityId(undefined);
        self.ShiftId('');
        self.FromDate('');
        self.ToDate('');
        ApplyCustomBinding('shiftdetailsreport');
    }

    self.GenerateRDLCReportPDF = function () {
        
        var data = new Object();
        data.BuildingId = self.BuildingId();
        data.GateId = self.GateId();
        data.SecurityId = self.SecurityId();
        data.ShiftName = self.ShiftId();
        data.FromDate = $('#txtFromDate').val();
        data.ToDate = $('#txtToDate').val();
        window.open('../Report/PrintShiftDetailReport?searchData=' + JSON.stringify(data), '_blankl');

    }
    self.GenerateRDLCReportExcel = function () {

        var data = new Object();
        data.BuildingId = self.BuildingId();
        data.GateId = self.GateId();
        data.SecurityId = self.SecurityId();
        data.ShiftName = self.ShiftId();
        data.FromDate = $('#txtFromDate').val();
        data.ToDate = $('#txtFromDate').val();
        window.open('../Report/ShiftReportExcelDownload?searchData=' + JSON.stringify(data), '_blankl');
    }

}