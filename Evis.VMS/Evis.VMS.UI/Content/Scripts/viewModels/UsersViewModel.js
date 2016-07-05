function UsersViewModel() {
    var self = this;

    self.UserId = ko.observable('');
    self.FullName = ko.observable('').extend({
        required: true,
        deferValidation: true
    });
    self.Email = ko.observable('').extend({ required: true, maxLength: 25, email: { message: "Invalid email" } });
    self.ContactNumber = ko.observable('').extend({
        required: true,
        pattern: {
            message: 'Invalid phone number.',
            params: /^([0-9\(\)\/\+ \-\.]*)$/
        }
    });
    self.RoleId = ko.observable(undefined).extend({ required: true });
    self.GenderId = ko.observable(undefined).extend({ required: true });
    self.OrganizationId = ko.observable(undefined).extend({ required: true });
    self.Nationality = ko.observable(undefined).extend({ required: true });

    self.GlobalSearch = ko.observable('');

    var Organization = {
        Id: self.Id,
        CompanyName: self.CompanyName
    }

    self.Organizations = ko.observableArray();
    AjaxCall('/Api/Users/GetAllOrganizations', null, 'GET', function (data) {
        self.Organizations(data);
    });

    self.Countries = ko.observableArray();
    AjaxCall('/Api/Users/GetAllCountries', null, 'GET', function (data) {
        self.Countries(data);
        Countries
    });

    self.Genders = ko.observableArray();
    AjaxCall('/Api/MyProfile/GetGender', null, 'GET', function (data) {
        self.Genders(data);
    });

    self.Roles = ko.observableArray();
    AjaxCall('/Api/Users/GetRoles', null, 'GET', function (data) {
        self.Roles(data);
    })


    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Users/GetUsersData', 7);

    self.UsersList = ko.observableArray([]);
    self.errors = ko.validation.group(self);

    self.GetAllUsers = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }

    self.GetAllUsers();

    self.GlobalSearchEnter = function (data, event) {
        if (event.which == 13) {
            self.GetAllUsers();
            console.log(event);
        }
    }

    self.SaveUser = function () {
        debugger;
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                //toastr.warning(data);
            });
        }
        else {
            var data = new Object();
            //debugger;)
            data.UserId = self.UserId(),
            data.OrganizationId = self.OrganizationId(),
            data.FullName = self.FullName(),
            data.Email = self.Email(),
            data.ContactNumber = self.ContactNumber(),
            data.GenderId = self.GenderId(),
            data.Nationality = self.Nationality(),
            data.RoleId = self.RoleId();

            //// display any error messages if we have them
            AjaxCall('/Api/Users/SaveUser', data, 'POST', function () {
                toastr.success('User saved successfully!!')
                ApplyCustomBinding('newuser');
            })
        }
    }

    self.EditUser = function (tableItem) {
        if (tableItem != undefined) {
            self.UserId(tableItem.UserId);
            self.OrganizationId(tableItem.OrganizationId);
            self.FullName(tableItem.FullName);
            self.Email(tableItem.Email);
            self.ContactNumber(tableItem.ContactNumber);
            self.GenderId(tableItem.GenderId);
            self.RoleId(tableItem.RoleId);
            self.Nationality(tableItem.Nationality);
        }
    }

    self.DeleteUser = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected user!");
        if (message == true) {
            AjaxCall('/Api/User/DeleteUser', tableItem, 'POST', function () {
                toastr.success('User deleted successfully!!')
                ApplyCustomBinding('newuser');
            });
        }
    }

    self.ResetUser = function () {
        self.UserId('');
        self.FullName('');
        self.Email('');
        self.GenderId(0);
        self.UserName('');
        self.RoleId('');
        self.GlobalSearch('');
        self.OrganizationId(0);
        self.Nationality(0);
        ApplyCustomBinding('newuser');
    }

}