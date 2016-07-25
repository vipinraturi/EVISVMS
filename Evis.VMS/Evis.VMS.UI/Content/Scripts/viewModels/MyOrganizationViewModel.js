function MyOrganizationViewModel() {
    var self = this;
    var ImagePath = null;
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
    self.Theme = ko.observable(undefined).extend({ required: true });
    self.CountryId = ko.observable(undefined).extend({ required: true });
    self.StateId = ko.observable(undefined).extend({ required: true });
    self.CityId = ko.observable(undefined).extend({ required: true });
    self.CompanyName = ko.observable().extend({
        required: true,
        deferValidation: true
    });
    self.Email = ko.observable('').extend({ required: true, minLength: 2, maxLength: 40, email: { message: "Invalid email" } });
    self.ContactNumber = ko.observable('').extend({
        required: true,
        pattern: {
            message: 'Invalid phone number.',
            params: /^([0-9\(\)\/\+ \-\.]*)$/
        }
    });
    self.ContactAddress = ko.observable('').extend({ required: true });
    self.FaxNumber = ko.observable('').extend({ required: true });
    self.POBox = ko.observable('').extend({ required: true });
    self.WebSite = ko.observable('').extend({ required: true });
    self.IsInsert = ko.observable(false);

    self.Themes = ko.observableArray();
    AjaxCall('/Api/Administration/GetTheme', null, 'GET', function (data) {
        debugger;
        self.Themes(data);
    });


    self.Countries = ko.observableArray();
    AjaxCall('/Api/Administration/GetCountries', null, 'GET', function (data) {
        self.Countries(data);
    });

    self.States = ko.observableArray();
    self.LoadStates = function () {
        AjaxCall('/Api/Administration/GetStatesOrCities?id=' + self.CountryId(), null, 'GET', function (data) {
            self.States(new Object());
            self.States(data);
            self.StateId(stateId);
        });
    }

    self.Cities = ko.observableArray();
    self.LoadCities = function () {
        AjaxCall('/Api/Administration/GetStatesOrCities?id=' + self.StateId(), null, 'GET', function (data) {
            self.Cities(new Object());
            self.Cities(data);
            self.CityId(cityId);
        });
    }

    self.SaveOrganization = function () {
        debugger;
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                toastr.warning(data);
            });
        }
        else {


            var data = new Object();
            //debugger;
            data.Id = self.Id(),
            data.CompanyName = self.CompanyName(),
            data.CityId = self.CityId(),
            data.EmailId = self.Email(),
            data.ContactNumber = self.ContactNumber(),
            data.ContactAddress = self.ContactAddress(),
            data.FaxNumber = self.FaxNumber(),
            data.ZipCode = self.POBox(),
            data.WebSite = self.WebSite()
            data.IsInsert = self.IsInsert();

            //// display any error messages if we have them
            AjaxCall('/Api/Administration/SaveOrganization', data, 'POST', function () {
                toastr.success('Organization updated successfully!!')
                ApplyCustomBinding('myorganization');
                self.IsInsert(true);
            })
        }
    }


    self.ResetOrganization = function () {
        self.IsInsert(true);
        self.GlobalSearch('');
        self.CompanyName('');
        self.Email('');
        self.ContactAddress('');
        self.FaxNumber('');
        self.ContactNumber('');
        self.POBox('');
        self.WebSite('');
        ApplyCustomBinding('organization');
    }
    //var User = {
    //    OrginizationId: self.OrginizationId
    //};
    //var Orginization = {
    //    CompanyName: self.CompanyName,
    //    ContactNumber: self.ContactNumber,
    //    Email: self.Email,
    //    PhoneNumber: self.PhoneNumber,
    //    POBox: self.POBox,
    //    WebSite: self.WebSite,
    //};
    //debugger;
    //self.Orginization = ko.observable(Orginization);


    AjaxCall('/Api/MyOrginization/GetMyOrginization', null, 'GET', function (data) {

        self.IsInsert(false);
        self.CountryId(data.CityMaster.ParentValues.ParentId);
        self.stateId = data.CityMaster.ParentId;
        self.cityId = data.CityId;
        self.Theme(data.ThemeName);
        self.CompanyName(data.CompanyName);
        self.ContactNumber(data.ContactNo);
        self.Id(data.CompanyId);
        self.Email(data.EmailAddress);
        self.ContactAddress(data.Address);
        self.FaxNumber(data.FaxNo);
        self.POBox(data.ZipCode);
        self.WebSite(data.WebSite);
        $("#myLogo").removeAttr('src');
        $("#myImg").attr('src', '');
        var d = new Date();
        debugger;
        if (data.ImagePath == null) {
            ImagePath = "/images/logo/main_logo.png";
            $("#myLogo").attr('src', ImagePath);
            $("#myImg").attr('src', ImagePath);
        } else {
            ImagePath = data.ImagePath;
            $("#myLogo").attr('src', ImagePath + "?" + d.getTime());
            $("#myImg").attr('src', ImagePath + "?" + d.getTime());
        }
        debugger;


    })

    self.ApplyTheme = function () {
        if (self.Theme() == '') {

        }
        else {
            changetheme(self.Theme());
        }
    }
    self.SaveImage = function () {
        debugger;
        $('.loader-div').show();
        $("#myLogo").removeAttr('src');
        $("#mainLogo").removeAttr('src');
        $("#myImg").attr('src', '');
        $('#avatar-modal').modal('hide');
        var formData = new FormData();
        var totalFiles = document.getElementById("avatarInput").files.length;
        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("avatarInput").files[i];

            formData.append("avatarInput", file);
        }
        //debugger;
        $.ajax({
            type: "POST",
            url: '/Administration/SaveUploadedFile',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                // alert('succes!!');

            },
            error: function (error) {
                //  alert("errror");
            }

        });
        RefreshImage(ImagePath);
    }

}


RefreshImage = function (Imagepath) {
    debugger;

    setTimeout(function () {
        var d = new Date();
        $("#myLogo").attr('src', '#');
        $("#mainLogo").attr('src', '#');
        $("#myImg").attr('src', '#');
        $("#mainLogo").attr('src', Imagepath + "?" + d.getTime());
        $("#myLogo").attr('src', Imagepath + "?" + d.getTime());
        $("#myImg").attr('src', Imagepath + "?" + d.getTime());
        $('.loader-div').hide();
    }, 2000);
}