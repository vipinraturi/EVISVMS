function BuildingViewModel() {
    var self = this;
    var stateId = 0;
    var cityId = 0;

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
    self.errors = ko.validation.group(self);
    self.Id = ko.observable(0);
    self.BuildingName = ko.observable().extend({
        required: { message: 'BuildingName name is required' },
        deferValidation: true
    });
    self.BuildingName = ko.observable('').extend({ required: true });
    self.StateName = ko.observable('').extend({ required: true });
    self.Address = ko.observable('').extend({ required: true });
    self.ZipCode = ko.observable('').extend({ required: true });
    self.Nationality = ko.observable('').extend({ required: true });
    self.StateId = ko.observable(0).extend({ required: true });
    self.CityId = ko.observable(0).extend({ required: true });
    self.NationalityId = ko.observable(0).extend({ required: true });
    self.OrganizationId = ko.observable(0).extend({ required: true });
    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);

    self.Organizations = ko.observableArray();
    AjaxCall('/Api/Users/GetAllOrganizations', null, 'GET', function (data) {
        self.Organizations(data);
    })
    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Administration/GetBuildingData', 7);

    self.GetAllBuildingData = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }
    self.Countries = ko.observableArray();
    AjaxCall('/Api/Users/GetAllCountries', null, 'GET', function (data) {
        debugger;
        self.Countries(data);

    })

    //self.City = ko.observableArray();
    //AjaxCall('/Api/Administration/GetAllStateOrCity', null, 'GET', function (data) {
    //    debugger;
    //    self.City(data);
    //    State
    //})
    self.City = ko.observableArray();
    self.LoadCities = function () {
        debugger;
        if (self.StateId() != undefined && self.StateId() != 0) {
            AjaxCall('/Api/Administration/GetAllStateOrCity?id=' + self.StateId(), null, 'GET', function (data) {
                debugger;
                self.City(data);
                self.CityId(cityId);

            })
        }
    }
    self.State = ko.observableArray();
    self.LoadStates = function () {
        debugger;
        if (self.NationalityId() != undefined && self.NationalityId() != 0) {
            AjaxCall('/Api/Administration/GetAllStateOrCity?id=' + self.NationalityId(), null, 'GET', function (data) {
                debugger;
                self.State(data);
                self.StateId(stateId);

            })
        }
    }
    self.SaveBuilding = function () {
        //debugger;
        //if (self.errors().length > 0) {
        //    self.errors.showAllMessages(true);
        //    this.errors().forEach(function (data) {
        //        //toastr.warning(data);
        //    });
        //}
        //else {
        var data = new Object();
        data.Id = self.Id(),
        data.OrganizationId = self.OrganizationId(),
        data.BuildingName = self.BuildingName(),
        data.Address = self.Address(),
        data.ZipCode = self.ZipCode(),
        //data.State = self.State(),
        // data.Country = self.Country(),
        data.CityId = self.CityId(),
        //// display any error messages if we have them
        AjaxCall('/Api/Administration/SaveBuilding', data, 'POST', function () {
            toastr.success('building saved successfully!!')
            ApplyCustomBinding('buildings');
            self.IsInsert(true);

        })
        //}

    }

    self.ResetBuilding = function () {
        self.GlobalSearch('');
        self.BuildingName('');
        self.Address('');
        self.ZipCode('');
        self.City('');
        ApplyCustomBinding('buildings');
    }
    self.EditBuilding = function (tableItem) {
        debugger;
        if (tableItem != undefined) {
            self.Id(tableItem.Id);
            self.BuildingName(tableItem.BuildingName);
            self.Address(tableItem.Address);
            self.ZipCode(tableItem.ZipCode);
            self.OrganizationId(tableItem.OrganizationId);
            self.NationalityId(tableItem.NationalityId);
            stateId = (tableItem.StateId);
            cityId = (tableItem.CityId);
            self.IsInsert(false);
            //data.CityId = self.CityId();

        }
    }
    self.DeleteBuilding = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected record!");
        if (message == true) {
            debugger;
            AjaxCall('/Api/Administration/DeleteBuilding', tableItem, 'POST', function () {
                toastr.success('Building deleted successfully!!')
                ApplyCustomBinding('buildings');
            });
        }
    }
    self.GlobalSearchEnter = function (data, event) {
        if (event.which == 13) {
            self.GetAllOrganizations();
            console.log(event);
        }
    }

    self.GetAllBuildingData();
}