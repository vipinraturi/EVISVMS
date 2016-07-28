var gblTEst = [];

function OrganizationViewModel() {
    var self = this;
    //var stateId = 0;
    //var cityId = 0;

    self.Id = ko.observable(0);
    self.CompanyName = ko.observable().extend({
        required: true,
        deferValidation: true
    });

    self.CountryId = ko.observable(undefined).extend({ required: true });
    //self.StateId = ko.observable(undefined).extend({ required: true });
    //self.CityId = ko.observable(undefined).extend({ required: true });
    //self.EmailId = ko.observable('').extend({ required: true, minLength: 2, maxLength: 40, email: { message: "Invalid email" } });
    //self.ContactNumber = ko.observable('').extend({
    //    required: true,
    //    pattern: {
    //        message: 'Invalid phone number.',
    //        params: /^([0-9\(\)\/\+ \-\.]*)$/
    //    }
    //});
    //self.ContactAddress = ko.observable('').extend({ required: true });
    //self.ZipCode = ko.observable('').extend({ required: true });
    //self.FaxNumber = ko.observable('');
    self.WebSite = ko.observable('').extend({ url: true });
    self.Country = ko.observable('');
    self.GlobalSearch = ko.observable('');

    ko.validation.rules['url'] = {
        validator: function (val, required) {
            if (!val) {
                return !required
            }
            val = val.replace(/^\s+|\s+$/, ''); //Strip whitespace
            //Regex by Diego Perini from: http://mathiasbynens.be/demo/url-regex
            return val.match(/^(?:(?:https?|ftp):\/\/)(?:\S+(?::\S*)?@)?(?:(?!10(?:\.\d{1,3}){3})(?!127(?:\.‌​\d{1,3}){3})(?!169\.254(?:\.\d{1,3}){2})(?!192\.168(?:\.\d{1,3}){2})(?!172\.(?:1[‌​6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1‌​,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00‌​a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u‌​00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/[^\s]*)?$/i);
        },
        message: 'This field has to be a valid URL'
    };
    ko.validation.registerExtenders();

    self.organizationErrors = ko.validation.group({
        CompanyName: this.CompanyName,
        CountryId: this.CountryId
        //StateId: this.StateId,
        //CityId: this.CityId,
        //EmailId: this.EmailId,
        //ContactNumber: this.ContactNumber,
        //ContactAddress: this.ContactAddress,
        //ZipCode: this.ZipCode
    });

    self.Countries = ko.observableArray();
    AjaxCall('/Api/Administration/GetCountries', null, 'GET', function (data) {
        self.Countries(data);
    });

    //self.States = ko.observableArray();
    //self.LoadStates = function () {
    //    AjaxCall('/Api/Administration/GetStatesOrCities?id=' + self.CountryId(), null, 'GET', function (data) {
    //        self.States(new Object());
    //        self.States(data);
    //        self.StateId(stateId);
    //    });
    //}

    //self.Cities = ko.observableArray();
    //self.LoadCities = function () {
    //    AjaxCall('/Api/Administration/GetStatesOrCities?id=' + self.StateId(), null, 'GET', function (data) {
    //        self.Cities(new Object());
    //        self.Cities(data);
    //        self.CityId(cityId);
    //    });
    //}

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Administration/GetOrganizationsData', 7);

    self.GetAllOrganizations = function () {

        self.DataGrid.UpdateSearchParam('?globalSearch=' + $.trim($("#txtGlobalSearch").val()));
        DataGrid.FlipPage(1);
        self.DataGrid.GetData();
    }

    self.SaveOrganization = function () {
        if (self.organizationErrors().length > 0 || ($("#txtWebsite").val() != '' && $("span.validationMessage").text() != '')) {
            self.organizationErrors.showAllMessages(true);
            return false;
        }

        else {
            var data = new Object();
            //
            data.Id = self.Id(),
            data.CompanyName = self.CompanyName(),
            data.CountryId = self.CountryId(),
            //data.EmailId = self.EmailId(),
            //data.ContactNumber = self.ContactNumber(),
            //data.ContactAddress = self.ContactAddress(),
            //data.FaxNumber = self.FaxNumber(),
            //data.ZipCode = self.ZipCode(),
            data.WebSite = self.WebSite();

            $('.loader-div').show();
            //// display any error messages if we have them
            AjaxCall('/Api/Administration/SaveOrganization', data, 'POST', function (data) {
                if (data.Success == true) {
                    toastr.success(data.Message);
                    self.ResetOrganizationDetails();
                    self.GetAllOrganizations();
                }
                else {
                    toastr.warning(data.Message);
                }
            })
        }
    }

    self.ResetOrganizationDetails = function () {
        self.GlobalSearch('');
        self.CompanyName('');
        self.CountryId(undefined);
        //self.EmailId('');
        //self.ContactAddress('');
        //self.FaxNumber('');
        //self.ContactNumber('');
        //self.ZipCode('');
        self.WebSite('');
        ApplyCustomBinding('organization');
    }

    self.ResetOrganization = function () {
        if (self.Id() == undefined || self.Id() == 0) {
            ResetOrganizationDetails();
        }
        else {
            self.EditOrganization(editOrg);
        }
    }


    self.DeleteOrganization = function (tableItem) {
        recordToDelete = tableItem;
    }

    self.DeleteConfirmed = function () {
        $('#myModal').modal('hide');
        AjaxCall('/Api/Administration/DeleteOrganization', recordToDelete, 'POST', function (data) {
            if (data.Success == true) {
                toastr.success(data.Message);
                ApplyCustomBinding('organization');
            }
            else if (data.Success == false) {
                toastr.warning(data.Message);
            }
        });
    }

    var editOrg = new Object();
    self.EditOrganization = function (tableItem) {
        if (tableItem != undefined) {
            editOrg = tableItem;
            self.Id(tableItem.Id);
            self.CompanyName(tableItem.CompanyName);
            self.CountryId(tableItem.CountryId);
            //stateId = tableItem.StateId;
            //cityId = tableItem.CityId;
            //self.EmailId(tableItem.EmailId);
            //self.ContactAddress(tableItem.ContactAddress);
            //self.FaxNumber(tableItem.FaxNumber);
            //self.ContactNumber(tableItem.ContactNumber);
            //self.ZipCode(tableItem.ZipCode);
            self.WebSite(tableItem.WebSite);
            $("#btnSaveOrg").text("Update");
        }
    }

    self.GlobalSearchEnter = function (data) {
        self.GetAllOrganizations();
        console.log(event);
    }

    self.GetAllOrganizations();

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
}