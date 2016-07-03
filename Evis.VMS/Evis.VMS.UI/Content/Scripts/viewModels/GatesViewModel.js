function GatesViewModel() {
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
    var self = this;
    self.errors = ko.validation.group(self);
    self.GateNumber = ko.observable().extend({
        required: { message: 'Gate Number is required' },
        deferValidation: true
    });
    self.Buildings = ko.observableArray();
    AjaxCall('/Api/Gates/GetAllBuilding', null, 'GET', function (data) {
        debugger;
        self.Buildings(data);
    })
    self.BuildingName = ko.observable('').extend({ required: true });
    self.GateNumber = ko.observable('').extend({ required: true });
    self.GlobalSearch = ko.observable('');
    self.CountryName = ko.observable('');
    self.StateName = ko.observable('');
    self.CityName = ko.observable('');
    self.BuildingId = ko.observable(0).extend({ required: true });
    self.Id = ko.observable(0).extend({ required: true });
    self.CountryId = ko.observable(0).extend({ required: true });
    self.StateId = ko.observable(0).extend({ required: true });
    self.CityId = ko.observable(0).extend({ required: true });

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Gates/GetAllGate', 7);
    self.GetAllGateData = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }
    self.BuildingChanged = function () {
        debugger;
        if (self.BuildingId() != undefined && self.BuildingId() != 0) {
            AjaxCall('/Api/Gates/GetAllBuildingDetails?id=' + self.BuildingId(), null, 'GET', function (data) {
                debugger;
                self.CityName(data[0].CityMaster.LookUpValue);
                self.CityId(data[0].CityMaster.Id);

                self.StateName(data[0].CityMaster.ParentValues.LookUpValue);
                self.StateId(data[0].CityMaster.ParentId);

                self.CountryName(data[0].CityMaster.ParentValues.ParentValues.LookUpValue);
                self.CountryId(data[0].CityMaster.ParentValues.ParentId);
            })
        }
    }

    self.SaveGate = function () {
        //if (self.errors().length > 0) {
        //    self.errors.showAllMessages(true);
        //    this.errors().forEach(function (data) {
        //        //toastr.warning(data);
        //    });
        //}
        //else {

        var data = new Object();
        data.GateNumber = self.GateNumber(),
        data.BuildingId = self.Id(),
        //// display any error messages if we have them
        AjaxCall('/Api/Gates/SaveGate', data, 'POST', function () {
            toastr.success('building saved successfully!!')
            ApplyCustomBinding('gates');

        })
        // }
    }
    self.EditGate = function (tableItem) {
        debugger;
        if (tableItem != undefined) {
            self.Id(tableItem.Id);
            self.GateNumber(tableItem.GateNumber);
            self.BuildingId(tableItem.BuildingId);
            self.CityId(tableItem.CityId);
            self.CityName(tableItem.CityName);
        }
    }
    self.DeleteGate = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected record!");
        if (message == true) {
            AjaxCall('/Api/Gates/DeleteGate', tableItem, 'POST', function () {
                toastr.success('Gate deleted successfully!!')
                ApplyCustomBinding('gates');
            });
        }
    }
    self.ResetGates = function () {
        self.GlobalSearch('');
        self.GateNumber('');
        self.BuildingName('');
        self.CityName('');


        ApplyCustomBinding('gates');
    }
    self.SaveGate = function () {
        debugger;
        //if (self.errors().length > 0) {
        //    self.errors.showAllMessages(true);
        //    this.errors().forEach(function (data) {
        //        //toastr.warning(data);
        //    });
        //}
        //else {
        var data = new Object();
        //debugger;
        data.Id = self.Id(),
        data.BuildingId = self.BuildingId(),
        data.GateNumber = self.GateNumber();
        //// display any error messages if we have them
        AjaxCall('/Api/Gates/SaveGate', data, 'POST', function () {
            toastr.success('Gate saved successfully!!')
            ApplyCustomBinding('gates');
            self.IsInsert(true);
        })
        //}
    }
    self.GetAllGateData();
}