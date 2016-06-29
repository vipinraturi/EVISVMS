function MyProfileViewModel() {
    var self = this;

    self.UserId = ko.observable('');
    self.FullName = ko.observable('');
    self.Email = ko.observable('');
    self.PhoneNumber = ko.observable('');
    self.ContactAddress = ko.observable(null);
    self.GenderId = ko.observable(0);
    self.RoleId = ko.observable('');
    self.IsInsert = ko.observable(true);

    //self.errors = ko.validation.group(self);


    var Gender = {
        Id: self.Id,
        Name: self.Name
    }

    self.Genders = ko.observableArray();
    AjaxCall('/Api/MyProfile/GetGender', null, 'GET', function (data) {
        debugger;
        self.Genders(ko.toJSON(data));
    })

    var Role = {
        Id: self.Id,
        Name: self.Name
    }

    self.Roles = ko.observableArray();
    AjaxCall('/Api/MyProfile/GetAllRoles', null, 'GET', function (data) {
        debugger;
        self.Roles(data);
    })

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
        //self.RoleId(data.Roles[0].RoleId);
        self.RoleId = ko.observable(data.Roles[0].RoleId);
    })



    //self.SelectedRole = ko.observable();

    //var CountryModel = function (data) {
    //    var self = this;
    //    self.Id = ko.observable(data.Id);
    //    self.Name = ko.observable(data.Name);
    //};

    //self.Roles = ko.observableArray([
    //     new CountryModel({ Id: "1", Name: "Security" }),
    //     new CountryModel({ Id: "2", Name: "Supervisor" })

    //]);

    //self.Gender = {
    //    wantsSpam: ko.observable(true),
    //    spamFlavors: ko.observableArray(["cherry", "almond"]) // Initially checks the Cherry and Almond checkboxes
    //};

    ////// ... then later ...
    ////viewModel.spamFlavors.push("msg");


    self.SaveMyProfile = function () {
        debugger;
        //if (self.errors().length > 0) {
        //    self.errors.showAllMessages(true);
        //    self.errors().forEach(function (data) {
        //        //toastr.warning(data);
        //    });
        //}
        //else {
        //var data = {
        //    //UserId: self.UserId(),
        //    FirstName: self.User().FullName,
        //    //EmailAddress: self.Email(),
        //    ContactNo: self.User().PhoneNumber,
        //    //Gender: self.Gender(),
        //    //Role: self.SelectedRole(),
        //    Address: self.User().ContactAddress
        //}
        //debugger;
        //console.log(JSON.stringify(data));
        //console.log(ko.toJSON(data));


        var data = new Object();
        //debugger;
        data.FullName = self.User().FullName,
        data.PhoneNumber = self.User().PhoneNumber,
        data.ContactAddress = self.User().ContactAddress;

        //// display any error messages if we have them
        AjaxCall('/Api/MyProfile/SaveMyProfile', data, 'PUT', function () {
            toastr.success('My Profile data is saved successfully!!')
            ApplyCustomBinding('myprofile');
            self.IsInsert(true);
        })
        //}
    }

    self.ResetMyProfile = function () {
        //self.User().FullName('');
        //self.User().PhoneNumber('');
        //self.User().ContactAddress('');
        debugger;
        ApplyCustomBinding('myprofile');
    }
}

$(function () {
    debugger;
    $("#avatarInput").change(function () {
        debugger;
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = imageIsLoaded;
            reader.readAsDataURL(this.files[0]);
        }
    });
});

function imageIsLoaded(e) {
    debugger;
    $('#myImg').attr('src', e.target.result);
};
