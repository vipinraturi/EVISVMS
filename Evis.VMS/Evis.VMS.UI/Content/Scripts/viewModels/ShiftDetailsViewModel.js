function ShiftDetailsViewModel() {
    var self = this;
    var organisationId = "";
    self.BuildingId = ko.observable(undefined);
    self.GateId = ko.observable(undefined);
    self.SecurityId = ko.observable(undefined);
    self.ShiftId = ko.observableArray(undefined);

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Report/GetShiftDetailsGrid', 7);
    self.DataGrid.GetData();

    self.GetOrganization = function () {
        //debugger;
     AjaxCall('/Api/Report/Getorganisation', null, 'GET', function (data) {
     self.organisationId=(data.OrganizationId);
     alert(self.organisationId);
        });
    }
    GetOrganization();
    

   

    self.Security = ko.observableArray();
    //debugger;
    self.GetUsers = function () {
        AjaxCall('/Api/Report/GetUsers', null, 'GET', function (data) {
            self.Security(data);
            //var id=data.Id
            //self.SecurityId(id)
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
    //debugger;
        self.GetShiftname = function () {
            AjaxCall('/Api/Report/GetShifts', null, 'GET', function (data) {
                self.shift(data);
                //self.ShiftId(ShiftId)

            });


        }
        GetShiftname();

        //self.SearchDetails = function () {
        //    debugger;
        //    var data = new Object();
        //    data.BuildingId = self.BuildingId();
        //    data.GateId = self.GateId();
        //    data.SecurityId = self.SecurityId();
        //    data.ShiftName = self.ShiftId();
        //    data.FromDate = self.FromDate();
        //    data.ToDate = self.ToDate();

        //    self.DataGrid.UpdateSearchParam('?search=' + JSON.stringify(data));
        //    self.DataGrid.GetData();
        //}

}