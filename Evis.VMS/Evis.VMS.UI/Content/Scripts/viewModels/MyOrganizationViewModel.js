﻿function MyOrganizationViewModel() {
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

    var WebSite = ko.observable().extend({ url: true });
    self.errors = ko.validation.group(self);
    self.CompanyName = ko.observable().extend({
        required: true,
        deferValidation: true
    });
    self.Id = ko.observable(0);
   // self.Theme = ko.observable(undefined).extend({ required: true });
    self.CountryId = ko.observable(undefined).extend({ required: true });
   
    self.WebSite = ko.observable('').extend({ url: true });
    self.IsInsert = ko.observable(false);

    //self.Themes = ko.observableArray();

    ko.validation.rules['url'] = {
        validator: function (val, required) {
            if (!val) {
                return !required
            }
            val = val.replace(/^\s+|\s+$/, ''); //Strip whitespace
            //Regex by Diego Perini from: http://mathiasbynens.be/demo/url-regex
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

  
    self.SaveOrganization = function () {
        $('.loader-div').show();
        setTimeout(function () {
            if (self.organizationErrors().length > 0 || ($("#txtWebsite").val() != '' && $("span.validationMessage").text() != '')) {
                self.organizationErrors.showAllMessages(true);
                return false;
            }
            else {
                

                var data = new Object();
              
                data.Id = self.Id(),
                data.CompanyName = self.CompanyName(),
                data.CountryId = self.CountryId(),
                data.WebSite = self.WebSite()
                data.IsInsert = self.IsInsert();

                //// display any error messages if we have them
                AjaxCall('/Api/Administration/SaveOrganization', data, 'POST', function () {
                    toastr.clear();
                    toastr.success('Organization updated successfully!!')
                    ApplyCustomBinding('myorganization');
                    self.IsInsert(true);
                })
            }
           
        }, 500);
        $('.loader-div').hide();
    }
    self.LoadImage = function () {
        debugger;
        $('#avatar-modal').modal('hide');
        var output = document.getElementById('myImg');
        output.src = '#';
        document.getElementById("avatarInput").files.length = 0;
        $("#avatarInput").val('');
        RefreshImage(ImagePath);
    }
   
  
    self.ResetOrganization = function () {
        self.IsInsert(true);
        self.GlobalSearch('');
        self.CompanyName('');
     
        self.WebSite('');
        ApplyCustomBinding('myorganization');
    }

    var Orginization = {
        CompanyName: self.CompanyName,
        
    };
    //
    self.Orginization = ko.observable(Orginization);


    AjaxCall('/Api/MyOrginization/GetMyOrginization', null, 'GET', function (data) {
        $('.loader-div').show();
        setTimeout(function () {
            
            self.IsInsert(false);
            
            $("#myLogo").removeAttr('src');
            $("#myImg").attr('src', '');
            var d = new Date();
            if (data.ImagePath == null) {
                ImagePath = "/images/logo/main_logo.png";
                $("#myLogo").attr('src', ImagePath);
                $("#myImg").attr('src', ImagePath);
            } else {
                ImagePath = data.ImagePath;
                $("#myLogo").attr('src', ImagePath + "?" + d.getTime());
                $("#myImg").attr('src', ImagePath + "?" + d.getTime());
            }
            $('.loader-div').hide();
        }, 1000);
        
        self.Id(data.CompanyId);
        self.CountryId(data.CountryId);
        self.CompanyName(data.CompanyName);

        self.WebSite(data.WebSite);

    })

    self.ApplyTheme = function () {
        if (self.Theme() == '') {

        }
        else {
            changetheme(self.Theme());
        }
    }
    self.SaveImage = function () {
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
        
        //
        $.ajax({
            type: "POST",
            url: '/Administration/SaveUploadedFile',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                RefreshImage(response.ImagePath);
            },
            error: function (error) {
                //  alert("errror");
            }

        });
       
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


