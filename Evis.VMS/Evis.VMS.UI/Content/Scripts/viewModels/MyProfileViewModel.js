function MyProfileViewModel() {
    var self = this;

    self.UserId = ko.observable('');
    self.FullName = ko.observable('').extend({
        required: true,
        deferValidation: true
    });
    self.PhoneNumber = ko.observable('').extend({
        required: true,
        pattern: {
            message: 'Invalid phone number.',
            params: /^([0-9\(\)\/\+ \-\.]*)$/
        }
    });
    self.ContactAddress = ko.observable('');
    self.GenderId = ko.observable(0);
    self.RoleId = ko.observable('');
    self.Nationality = ko.observable('');

    self.errors = ko.validation.group(self);

    self.Genders = ko.observableArray();
    AjaxCall('/Api/MyProfile/GetGender', null, 'GET', function (data) {
        self.Genders(data);
    });

    self.Roles = ko.observableArray();
    AjaxCall('/Api/MyProfile/GetAllRoles', null, 'GET', function (data) {
        self.Roles(data);
    });

    var User = {
        UserId: self.UserId,
        FullName: self.FullName,
        Email: self.Email,
        PhoneNumber: self.PhoneNumber,
        ContactAddress: self.ContactAddress,
        GenderId: self.GenderId,
    };

    self.User = ko.observable(User);
    AjaxCall('/Api/MyProfile/GetMyProfile', null, 'GET', function (data) {
        debugger;
        self.User(data);
        self.GenderId = self.User().GenderId;
        self.RoleId = ko.observable(data.Roles[0].RoleId);
        self.Nationality = ko.observable(data.CountryMaster.LookUpValue);
    });

    self.SaveMyProfile = function () {
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                console.log(event);
            });
        }
        else {
            var data = new Object();
            data.FullName = self.User().FullName,
            data.PhoneNumber = self.User().PhoneNumber,
            data.ContactAddress = self.User().ContactAddress;

            AjaxCall('/Api/MyProfile/SaveMyProfile', data, 'PUT', function () {
                toastr.success('My Profile data is saved successfully!!')
                ApplyCustomBinding('myprofile');
                self.IsInsert(true);
            })
        }
    }

    self.ResetMyProfile = function () {
        //self.User().FullName('');
        //self.User().PhoneNumber('');
        //self.User().ContactAddress('');
        ApplyCustomBinding('myprofile');
    }
}

$(function () {
    $("#avatarInput").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = imageIsLoaded;
            reader.readAsDataURL(this.files[0]);
        }
    });
});

function imageIsLoaded(e) {
    $('#myImg').attr('src', e.target.result);
};
