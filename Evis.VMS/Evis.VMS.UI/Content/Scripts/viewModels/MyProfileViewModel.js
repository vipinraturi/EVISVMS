function MyProfileViewModel() {
    var self = this;

    self.UserId = ko.observable('');
    self.FullName = ko.observable('').extend({
        required: true
    });
    self.PhoneNumber = ko.observable('').extend({
        required: true,
        pattern: {
            message: 'Invalid phone number.',
            params: /^([0-9\(\)\/\+ \-\.]*)$/
        }
    });
    self.Email = ko.observable('');
    self.ContactAddress = ko.observable('');
    self.GenderId = ko.observable(0);
    self.RoleId = ko.observable('');
    self.Nationality = ko.observable('');
    self.DisplayName = ko.observable('');
    self.RoleName = ko.observable('');
    self.ProfilePicturePath = ko.observable('');

    self.myProfileErrors = ko.validation.group({
        FullName: this.FullName,
        PhoneNumber: this.PhoneNumber
    });

    self.Genders = ko.observableArray();
    AjaxCall('/Api/MyProfile/GetGender', null, 'GET', function (data) {
        self.Genders(data);
    });

    self.Roles = ko.observableArray();
    AjaxCall('/Api/MyProfile/GetAllRoles', null, 'GET', function (data) {
        self.Roles(data);
    });

    AjaxCall('/Api/MyProfile/GetMyProfile', null, 'GET', function (data) {
        self.UserId(data.Id);
        self.FullName(data.FullName);
        self.DisplayName(data.FullName);
        self.Email(data.Email);
        self.PhoneNumber(data.PhoneNumber);
        self.ContactAddress(data.ContactAddress);
        self.GenderId(data.GenderId);
        self.RoleId(data.Roles[0].RoleId);
        self.Nationality(data.CountryMaster.LookUpValue);
        $('#viewImageUnique').show();
        $('.img_responsive_Avatar').attr('src', data.ProfilePicturePath).addClass('dz-message');

        self.Roles().forEach(function (item) {
            if (item.Id === self.RoleId()) {
                self.RoleName(item.Name);
            }
        });
    });

    self.SaveMyProfile = function () {
        if (self.myProfileErrors().length > 0) {
            self.myProfileErrors.showAllMessages(true);
            this.myProfileErrors().forEach(function (data) {
                console.log(event);
            });
        }
        else {
            var data = new Object();
            data.FullName = self.FullName();
            data.PhoneNumber = self.PhoneNumber();
            data.ContactAddress = self.ContactAddress();
            data.ProfilePicturePath = $('.dz-image img').attr('alt');
            //debugger;
            AjaxCall('/Api/MyProfile/SaveMyProfile', data, 'PUT', function () {
                toastr.success('My Profile data is saved successfully!!')
                ApplyCustomBinding('myprofile');
                //self.IsInsert(true);
            })
        }
    }

    self.ResetMyProfile = function () {
        ApplyCustomBinding('myprofile');
    }

    self.ViewImage = function () {
        var srcURL = '';
        if ($('.dz-image img').attr('alt') != undefined) {
            srcURL = ($('.dz-image img').attr('alt'));
        }
        else if ($('.img_responsive_Avatar').attr('src') != undefined) {
            srcURL = ($('.img_responsive_Avatar').attr('src'));
        }

        if (srcURL.indexOf('/images/UserImages') == -1) {
            srcURL = '/images/UserImages/' + srcURL;
        }

        $('#originalSize').attr('src', srcURL);
        $('#imageModal').modal('show');
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
