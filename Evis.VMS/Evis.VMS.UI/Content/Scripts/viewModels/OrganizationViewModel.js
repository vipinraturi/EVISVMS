var gblTEst = [];

function OrganizationViewModel() {
    var self = this;

    self.Id = ko.observable(0);
    self.CompanyName = ko.observable().extend({
        required: true,
        deferValidation: true
    });

    self.CountryId = ko.observable(undefined).extend({ required: true });
    self.WebSite = ko.observable('').extend({ url: true });
    self.Country = ko.observable('');
    self.GlobalSearch = ko.observable('');

    ko.validation.rules['url'] = {
        validator: function (val, required) {
            if (!val) {
                return !required
            }
            val = val.replace(/^\s+|\s+$/, '');
            return val.match(/[-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?/gi);
        },
        message: 'This field has to be a valid URL'
    };
    ko.validation.registerExtenders();

    self.organizationErrors = ko.validation.group({
        CompanyName: this.CompanyName,
        CountryId: this.CountryId
    });

    self.Countries = ko.observableArray();
    AjaxCall('/Api/Administration/GetCountries', null, 'GET', function (data) {
        self.Countries(data);
    });

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
            data.Id = self.Id(),

            data.CompanyName = $.trim(self.CompanyName()),
            data.CountryId = self.CountryId(),
            data.WebSite = self.WebSite();

            $('.loader-div').show();
            AjaxCall('/Api/Administration/SaveOrganization', data, 'POST', function (data) {
                if (data.Success == true) {
                    toastr.clear();
                    toastr.success(data.Message);
                    self.ResetOrganizationDetails();
                    self.GetAllOrganizations();
                }
                else {
                    toastr.clear();
                    toastr.warning(data.Message);
                }
            })
            $('.loader-div').hide();
        }
    }

    self.ResetOrganizationDetails = function () {
        self.GlobalSearch('');
        self.CompanyName('');
        self.CountryId(undefined);
        self.WebSite('');
        ApplyCustomBinding('organization');
    }

    self.DeleteOrganization = function (tableItem) {
        recordToDelete = tableItem;
    }

    self.DeleteConfirmed = function () {
        $('#myModal').modal('hide');
        $('.modal-backdrop').modal('show');
        $('.modal-backdrop').modal('hide');
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