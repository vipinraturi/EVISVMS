function UsersViewModel() {
    var self = this;
    var recordToDelete = new Object();

    self.UserId = ko.observable('');
    self.FullName = ko.observable('').extend({
        required: true,
        deferValidation: true
    });

    self.Email = ko.observable('').extend({
        required: true,
        maxLength: 50,
        pattern: {
            message: 'Invalid email.',
            params: /\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*/
        }
    });

    self.ContactNumber = ko.observable('').extend({
        required: true,
        pattern: {
            message: 'Invalid phone number.',
            params: /^\s*\+?([0-9\(\)\/\-\.\s*]*)$/
        },
        minLength: {
            params: 6,
            message: 'Enter minimum of 6-length number'
        },
        maxLength: {
            params: 12,
            message: 'Enter maximum of 12-length number'
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
        self.DataGrid.UpdateSearchParam('?globalSearch=' + $.trim($("#txtGlobalSearchUser").val()));
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
            data.FullName = $.trim(self.FullName()),
            data.Email = $.trim(self.Email()),
            data.ContactNumber = $.trim(self.ContactNumber()),
            data.GenderId = self.GenderId(),
            data.Nationality = self.Nationality(),
            data.RoleId = self.RoleId();
            if ($('.dz-image img').attr('alt') == undefined || $('.dz-image img').attr('alt') == "undefined") {
                data.ProfilePicturePath = "";
            }
            else {
                var imagePath = $('.dz-image img').attr('alt');
                if (imagePath != "/images/UserImages/VisitorImage") {
                    data.ProfilePicturePath = imagePath;
                }
            }

            
            $('.loader-div').show();
            AjaxCall('/Api/Users/SaveUser', data, 'POST', function (data) {
                if (data.Success == true) {
                    toastr.clear();
                    toastr.success(data.Message);
                    self.ResetUserDetails();
                    self.GetAllUsers();
                    $('.loader-div').hide();
                }
                else {
                    toastr.clear();
                    toastr.warning(data.Message);
                    $('.loader-div').hide();
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

            $('.dz-image-preview').empty();

            if (tableItem.IsImageAvailable) {
                if (tableItem.ProfilePicturePath != undefined && tableItem.ProfilePicturePath != "") {
                    //debugger;
                    var imagePath = tableItem.ProfilePicturePath;
                    self.ProfilePicturePath(tableItem.ProfilePicturePath);
                    var mockFile = { name: tableItem.ImagePath, size: 1024 };
                    myDropzone.emit("addedfile", mockFile);
                    myDropzone.emit("thumbnail", mockFile, imagePath);
                    myDropzone.createThumbnailFromUrl(mockFile, imagePath);
                    $('.dz-image').addClass('dz-message');
                    $('.dz-image img').addClass('dz-message');
                }
            }
            else {

                var imagePath = '/images/avatar.jpg';
                var mockFile = { name: 'UserImage', size: 1024 };
                myDropzone.emit("addedfile", mockFile);
                myDropzone.emit("thumbnail", mockFile, imagePath);
                myDropzone.createThumbnailFromUrl(mockFile, imagePath);
                $('.dz-image').addClass('dz-message');
                $('.dz-image img').addClass('dz-message');
            }
            
        }
    }

    self.DeleteUser = function (tableItem) {
        recordToDelete = tableItem;
    }

    self.DeleteConfirmed = function () {
        
        $('#myModal').modal('hide');
        $('.modal-backdrop').modal('show');
        $('.modal-backdrop').modal('hide');
        AjaxCall('/Api/User/DeleteUser', recordToDelete, 'POST', function (data) {
            //toastr.success('User deleted successfully!!')
            //ApplyCustomBinding('newuser');
            if (data.Success == true) {
                toastr.success(data.Message);
                ApplyCustomBinding('newuser');
            }
            else if (data.Success == false) {
                toastr.warning(data.Message);
            }
        });

    }

    self.ResetUser = function () {
        //if (self.UserId() == undefined || self.UserId() == "" || self.UserId() == 0) {
        self.ResetUserDetails();
        //}
        //else {
        //    self.EditUser(editUser);
        //}
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
        var srcURL = '';
       // debugger;
        if ($('.dz-image img').attr('alt') != "" && $('.dz-image img').attr('alt') != "undefined") {
            srcURL = '/images/UserImages/' + $('.dz-image img').attr('alt');
        }
        else {
            srcURL = self.ProfilePicturePath();
        }


        $('#originalSize').attr('src', srcURL);
        $('#imageModal').modal('show');
    }

}