function UsersViewModel() {
    var self = this;
    var recordToDelete = new Object();

    self.UserId = ko.observable('');
    self.FullName = ko.observable('').extend({
        required: true,
        deferValidation: true
    });

    self.Email = ko.observable('').extend({ required: true, maxLength: 50, email: { message: "Invalid email" } });
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
    self.ProfilePicturePath = ko.observable('');

    self.userErrors = ko.validation.group({
        OrganizationId: this.OrganizationId,
        FullName: this.FullName,
        GenderId: this.GenderId,
        Email: this.Email,
        ContactNumber: this.ContactNumber,
        RoleId: this.RoleId,
        Nationality: this.Nationality
    });

    self.GlobalSearch = ko.observable('');

    var Organization = {
        Id: self.Id,
        CompanyName: self.CompanyName
    }

    self.Organizations = ko.observableArray();
    AjaxCall('/Api/User/GetAllOrganizations', null, 'GET', function (data) {
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
    //self.userErrors = ko.validation.group(self);

    self.GetAllUsers = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + $.trim($("#txtGlobalSearch").val()));
        DataGrid.FlipPage(1);
        self.DataGrid.GetData();
    }

    self.GetAllUsers();

    //self.GlobalSearchEnter = function (data, event) {
    //    self.GetAllUsers();
    //}

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

    self.SaveUser = function () {
        if (self.userErrors().length > 0) {
            self.userErrors.showAllMessages(true);
            return false;
        }
        else {
            var data = new Object();
            data.UserId = self.UserId(),
            data.OrganizationId = self.OrganizationId(),
            data.FullName = self.FullName(),
            data.Email = self.Email(),
            data.ContactNumber = self.ContactNumber(),
            data.GenderId = self.GenderId(),
            data.Nationality = self.Nationality(),
            data.RoleId = self.RoleId();
            data.ProfilePicturePath = $('.dz-image img').attr('alt');
            
            $('.loader-div').show();
            //// display any error messages if we have them
            AjaxCall('/Api/Users/SaveUser', data, 'POST', function (data) {
                if (data.Success == true) {
                    toastr.success(data.Message);
                    self.ResetUserDetails();
                    self.GetAllUsers();
                }
                else {
                    toastr.warning(data.Message);
                }
            })
        }
    }

    var editUser = new Object();
    self.EditUser = function (tableItem) {
        if (tableItem != undefined) {
            editUser = tableItem;
            $('#viewImageUnique').show();
            self.UserId(tableItem.UserId);
            self.OrganizationId(tableItem.OrganizationId);
            self.FullName(tableItem.FullName);
            self.Email(tableItem.Email);
            self.ContactNumber(tableItem.ContactNumber);
            self.GenderId(tableItem.GenderId);
            self.RoleId(tableItem.RoleId);
            self.Nationality(tableItem.Nationality);
            $("#btnSaveUser").text("Update");
            //alert(tableItem.ProfilePicturePath);
            ////debugger;

            $('.dz-image-preview').empty();
            var imagePath = tableItem.ProfilePicturePath;
            self.ProfilePicturePath(tableItem.ProfilePicturePath);
            var mockFile = { name: tableItem.ImagePath, size: 1024 };
            myDropzone.emit("addedfile", mockFile);
            myDropzone.emit("thumbnail", mockFile, imagePath);
            myDropzone.createThumbnailFromUrl(mockFile, imagePath);
            $('.dz-image').addClass('dz-message');
            $('.dz-image img').addClass('dz-message');

            //$('.img_responsive_Avatar').attr('src', tableItem.ProfilePicturePath).addClass('dz-message');
        }
    }

    self.DeleteUser = function (tableItem) {
        recordToDelete = tableItem;
    }

    self.DeleteConfirmed = function () {
        $('#myModal').modal('hide');
        AjaxCall('/Api/User/DeleteUser', recordToDelete, 'POST', function () {
            toastr.success('User deleted successfully!!')
            ApplyCustomBinding('newuser');
        });
    }

    self.ResetUser = function () {
        if (self.UserId() == undefined || self.UserId() == "" || self.UserId() == 0) {
            self.ResetUserDetails();
        }
        else {
            self.EditUser(editUser);
        }
    }

    self.ResetUserDetails = function () {
        self.UserId('');
        self.FullName('');
        self.Email('');
        self.GenderId(0);
        self.RoleId('');
        self.GlobalSearch('');
        self.OrganizationId(0);
        self.Nationality(0);
        ApplyCustomBinding('newuser');
    }

    self.ViewImage = function () {
        $('#originalSize').attr('src', self.ProfilePicturePath());
        $('#imageModal').modal('show');
    }

}