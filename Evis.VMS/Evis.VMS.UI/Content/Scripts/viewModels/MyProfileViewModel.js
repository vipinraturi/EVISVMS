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
            params: /^\s*\+?([0-9\(\)\/\-\.\s*]*)$/
        },
        minLength: {
            params: 6,
            message: 'Enter minimum of 6-length number'
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

    setTimeout(function () {

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

            $('.dz-image-preview').empty();
            //debugger;

            if (data.ProfilePicturePath != null && data.ProfilePicturePath != "" && data.ProfilePicturePath != "/images/UserImages/VisitorImage") {
                var imagePath = data.ProfilePicturePath;
                self.ProfilePicturePath(data.ProfilePicturePath);
                var mockFile = { name: "User Image", size: 1024 };
                myDropzoneUnique.emit("addedfile", mockFile);
                myDropzoneUnique.emit("thumbnail", mockFile, imagePath);
                myDropzoneUnique.createThumbnailFromUrl(mockFile, imagePath);
                $('.dz-image').addClass('dz-message');
                $('.dz-image img').addClass('dz-message');

                if (imagePath != "") {
                    $('#imgUserAvatar').attr('src', imagePath);
                }
            }
            else {
                var imagePath = '/images/avatar.jpg';
                var mockFile = { name: 'UserImage', size: 1024 };
                myDropzoneUnique.emit("addedfile", mockFile);
                myDropzoneUnique.emit("thumbnail", mockFile, imagePath);
                myDropzoneUnique.createThumbnailFromUrl(mockFile, imagePath);
                $('#dropzoneMyProfileImageForm .dz-image').addClass('dz-message');
                $('#dropzoneMyProfileImageForm .dz-image img').addClass('dz-message');
            }

            self.Roles().forEach(function (item) {
                if (item.Id === self.RoleId()) {
                    self.RoleName(item.Name);
                }
            });
        });

    }, 1000);

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
            data.GenderId = self.GenderId();
            data.PhoneNumber = self.PhoneNumber();
            data.ContactAddress = self.ContactAddress();
            data.ProfilePicturePath = $('.dz-image img').attr('img-name-unique');

            // To clear 
            self.UserId('');
            self.FullName('');
            self.Email('');
            self.GenderId(0);
            self.RoleId('');

            $('.loader-div').show();
            AjaxCall('/Api/MyProfile/SaveMyProfile', data, 'POST', function () {
                toastr.clear();
                toastr.success('My Profile updated successfully!!');
                ApplyCustomBinding('myprofile');
                $('.loader-div').hide();
            })
        }
    }

    self.ResetMyProfile = function () {
        ApplyCustomBinding('myprofile');
    }

    self.ViewImage = function () {

        //debugger;
        var srcURL = '';
        if ($('.dz-image img').attr('alt') != "" && $('.dz-image img').attr('alt') != "User Image" && $('.dz-image img').attr('alt') !=  "undefined") {
            srcURL = '/images/UserImages/' + $('.dz-image img').attr('alt');
        }
        else {
            srcURL =  self.ProfilePicturePath();
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
