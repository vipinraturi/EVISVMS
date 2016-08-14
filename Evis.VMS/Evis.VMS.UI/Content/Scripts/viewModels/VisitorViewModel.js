function VisitorViewModel(visitorName, gender, nationalityVal, dateOfBirth, typeOfCard, idNumber, nationalityVal, companyName, emailAddress, contactNumber, identityImages) {

    nationality = (nationalityVal != "" ? nationalityVal : undefined);
    typeOfCard = (typeOfCard != "" ? typeOfCard : undefined);
    gender = (gender != "" ? gender : undefined);

    var self = this;
    Id = ko.observable(0);
    VisitorName = ko.observable(visitorName).extend({ required: true });
    EmailAddress = ko.observable(emailAddress);
    Gender = ko.observable(gender).extend({ required: true });
    DOB = ko.observable(dateOfBirth).extend({ required: true });
    TypeOfCardValue = ko.observable(typeOfCard).extend({ required: true });
    IdNo = ko.observable(idNumber).extend({ required: true });
    Nationality = ko.observable(nationality).extend({ required: true });
    ContactNo = ko.observable(contactNumber).extend({ required: true });
    ContactAddress = ko.observable('');

    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);
    self.IsEdit = ko.observable(false);
    self.LookUpValues = ko.observableArray();
    self.Genders = ko.observableArray();
    self.TypeOfCards = ko.observableArray();
    self.Nationalities = ko.observableArray();
    self.errors = ko.validation.group({
        VisitorName: this.VisitorName,
        Gender: this.Gender,
        DOB: this.DOB,
        TypeOfCardValue: this.TypeOfCardValue,
        IdNo: this.IdNo,
        Nationality: this.Nationality,
        ContactNo: this.ContactNo
    });

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Visitor/GetVisitorData', 7);
    self.VisitorList = ko.observableArray([]);

    self.GetAllVisitor = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData(true);
    }

    self.LoadMasterData = function () {
        var lookUpTypes = [];
        lookUpTypes.push("Gender");
        lookUpTypes.push("TypeOfCards");
        lookUpTypes.push("Nationalities");

        AjaxCall('/Api/Common/GetLookUpData', lookUpTypes, 'POST', function (data) {
            self.LookUpValues(data);
            self.Genders(ko.utils.arrayFilter(self.LookUpValues(), function (item) {
                return item.LookUpType.TypeCode == "Gender";
            }));
            self.TypeOfCards(ko.utils.arrayFilter(self.LookUpValues(), function (item) {
                return item.LookUpType.TypeCode == "TypeOfCards";
            }));

            self.Nationalities(ko.utils.arrayFilter(self.LookUpValues(), function (item) {
                return item.LookUpType.TypeCode == "Nationalities";
            }));
        })
    }
    self.SaveVisitor = function () {

        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {

            });
        }
        else {
            var data = new Object();
            var initial = self.DOB().split(/\//);
            data.VisitorName = self.VisitorName(),
            data.EmailAddress = self.EmailAddress(),
            data.Gender = self.Gender(),
            data.DOB = [initial[1], initial[0], initial[2]].join('/'),
            data.TypeOfCard = self.TypeOfCardValue(),
            data.IdNo = self.IdNo(),
            data.Nationality = self.Nationality()
            data.ContactNo = self.ContactNo();

            if ($('.dz-image img').attr("img-name-unique") == undefined) {
                data.ImagePath = "";
            }
            else {
                data.ImagePath = $('.dz-image img').attr("img-name-unique");
            }

            data.IdentityImage1_Path = $('.dz-image-preview img').eq(0).attr("img-name-unique");
            data.IdentityImage2_Path = $('.dz-image-preview img').eq(1).attr("img-name-unique");
            data.IdentityImage3_Path = $('.dz-image-preview img').eq(2).attr("img-name-unique");

            data.ContactAddress = self.ContactAddress()
            data.IsInsert = self.IsInsert();
            data.Id = self.Id();

            //// display any error messages if we have them
            AjaxCall('/Api/Visitor/SaveVisitor', data, 'POST', function (result) {
                if (result.Success) {
                    if (self.IsEdit()) {
                        toastr.success('Visitor updated successfully!!')
                    }
                    else {
                        toastr.success('Visitor saved successfully!!')
                    }

                    self.ResetData();
                    self.IsInsert(true);
                    self.GetAllVisitor();
                }
                else {
                    toastr.warning(result.Message)
                }
            })
        }
    }

    self.ResetVisitor = function () {
        ResetData();
    }

    self.ResetData = function () {
        $('#btnSave').html('Save <i class="fa fa-save"></i>');
        self.IsInsert(true);
        self.IsEdit(false);
        self.GlobalSearch('');
        self.VisitorName('');
        self.Gender(undefined);
        self.DOB('');
        self.TypeOfCardValue(undefined);
        self.IdNo('');
        self.EmailAddress('');
        self.Nationality(undefined);
        self.ContactNo('');
        self.ContactAddress('');
        dataToSend = '';
        //identityImages = [];
        //self.LoadIdentityImage(identityImages);
        $('#viewVisitorImageUnique').hide();
        ApplyCustomBinding('managevisitor');
    }

    self.DeleteVisitor = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected record!");
        if (message == true) {
            AjaxCall('/Api/Visitor/DeleteVisitor', tableItem, 'POST', function (result) {
                if (result.Success) {
                    toastr.success('Visitor deleted successfully!!')
                    ApplyCustomBinding('managevisitor');
                } else if (result.Success) {
                    toastr.warning("Visitor has dependency on other data. Please delete selected visitor's corresponding records first!!")
                }
            });
        }
    }

    self.EditVisitor = function (tableItem) {
        if (tableItem != undefined) {

            var d = new Date(tableItem.DOB),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();
            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;
            var dateDob = [day, month, year].join('/');

           
            $('#viewVisitorImageUnique').show();
            self.IsEdit(true);
            self.IsInsert(false);
            self.Id(tableItem.Id);
            self.VisitorName(tableItem.VisitorName);
            self.EmailAddress(tableItem.EmailAddress);
            self.Gender(tableItem.Gender);
            self.DOB(dateDob);
            self.TypeOfCardValue(tableItem.TypeOfCard);
            self.IdNo(tableItem.IdNo);
            self.Nationality(tableItem.Nationality);
            self.ContactNo(tableItem.ContactNo);
            self.ContactAddress(tableItem.ContactAddress);

            $('.dz-image-preview').empty();

            if (tableItem.ImagePath != undefined && tableItem.ImagePath != "") {
                var imagePath = '/images/VisitorImages/' + tableItem.ImagePath;
                var mockFile = { name: tableItem.ImagePath, size: 1024 };
                dropZoneVisitorImage.emit("addedfile", mockFile);
                dropZoneVisitorImage.emit("thumbnail", mockFile, imagePath);
                dropZoneVisitorImage.createThumbnailFromUrl(mockFile, imagePath);
                $('.dz-image').addClass('dz-message');
                $('.dz-image img').addClass('dz-message');
                $('#btnSave').html('Update <i class="fa fa-save"></i>');
                self.LoadIdentityImage(identityImages);
            }

            if (tableItem.IdentityImage1_Path != undefined && tableItem.IdentityImage1_Path != "") {
                var imagePath = '/images/VisitorIdentityImages/' + tableItem.IdentityImage1_Path;
                var mockFile = { name: tableItem.IdentityImage1_Path, size: 1024 };
                dropZoneMultipleFiels.emit("addedfile", mockFile);
                dropZoneMultipleFiels.emit("thumbnail", mockFile, imagePath);
                dropZoneMultipleFiels.createThumbnailFromUrl(mockFile, imagePath);
                $('.dz-image').addClass('dz-message');
                $('.dz-image img').addClass('dz-message');
                $('#btnSave').html('Update <i class="fa fa-save"></i>');
                self.LoadIdentityImage(identityImages);
            }

            if (tableItem.IdentityImage2_Path != undefined && tableItem.IdentityImage2_Path != "") {
                var imagePath = '/images/VisitorIdentityImages/' + tableItem.IdentityImage2_Path;
                var mockFile = { name: tableItem.IdentityImage2_Path, size: 1024 };
                dropZoneMultipleFiels.emit("addedfile", mockFile);
                dropZoneMultipleFiels.emit("thumbnail", mockFile, imagePath);
                dropZoneMultipleFiels.createThumbnailFromUrl(mockFile, imagePath);
                $('.dz-image').addClass('dz-message');
                $('.dz-image img').addClass('dz-message');
                $('#btnSave').html('Update <i class="fa fa-save"></i>');
                self.LoadIdentityImage(identityImages);
            }

            if (tableItem.IdentityImage3_Path != undefined && tableItem.IdentityImage3_Path != "") {
                var imagePath = '/images/VisitorIdentityImages/' + tableItem.IdentityImage3_Path;
                var mockFile = { name: tableItem.IdentityImage3_Path, size: 1024 };
                dropZoneMultipleFiels.emit("addedfile", mockFile);
                dropZoneMultipleFiels.emit("thumbnail", mockFile, imagePath);
                dropZoneMultipleFiels.createThumbnailFromUrl(mockFile, imagePath);
                $('.dz-image').addClass('dz-message');
                $('.dz-image img').addClass('dz-message');
                $('#btnSave').html('Update <i class="fa fa-save"></i>');
                self.LoadIdentityImage(identityImages);
            }
        }
    }


    self.LoadIdentityImage = function (identityImages) {
        
        $('#dropzoneForm .dz-image-preview').remove();
        var MultipleImagePath = [];

        if (identityImages != undefined && identityImages != "" && identityImages != null) {
            if (identityImages.split(',').length > 0) {
                var obj1 = new Object();
                obj1.fileName = identityImages.split(',')[0];
                obj1.ImgURL = '/images/VisitorIdentityImages/' + identityImages.split(',')[0];
                obj1.size = 1024;
                MultipleImagePath.push(obj1);
            }

            if (identityImages.split(',').length > 1) {
                var obj2 = new Object();
                obj2.fileName = identityImages.split(',')[1];
                obj2.ImgURL = '/images/VisitorIdentityImages/' + identityImages.split(',')[1];
                obj2.size = 1024;
                MultipleImagePath.push(obj2);
            }

            if (identityImages.split(',').length > 2) {
                var obj3 = new Object();
                obj3.fileName = identityImages[2];
                obj3.ImgURL = '/images/VisitorIdentityImages/' + identityImages.split(',')[2];
                obj3.size = 1024;
                MultipleImagePath.push(obj3);
            }

            $.each(MultipleImagePath, function (key, value) { //loop through it
                var mockFile = { name: value.fileName, size: value.size }; // here we get the file name and size as response 
                dropZoneMultipleFiels.options.addedfile.call(dropZoneMultipleFiels, mockFile);
                dropZoneMultipleFiels.options.thumbnail.call(dropZoneMultipleFiels, mockFile, value.ImgURL);//uploadsfolder is the folder where you have all those uploaded files
                dropZoneMultipleFiels.createThumbnailFromUrl(mockFile, value.ImgURL); // Set Image in strech mode.
                $("#dropzoneForm .dz-progress").remove();
            });
        }
    }



    self.ViewVisitorImage = function () {
        var srcURL = '';
        if ($('.dz-image img').attr("img-name-unique") != undefined) {
            srcURL = ($('.dz-image img').attr("img-name-unique"));
        }

        if (srcURL.indexOf('/images/VisitorImages') == -1) {
            srcURL = '/images/VisitorImages/' + srcURL;
        }

        $('#visitorOriginalSize').attr('src', srcURL);
        $('#visitorImageModal').modal('show');
    }

    self.GlobalSearchEnter = function (data, event) {
        if (event.which == 13 || event.keycode == 13) {
            self.GetAllVisitor();
        }
    }

    self.GetAllVisitor();
    self.LoadMasterData();

    if (identityImages != undefined && identityImages != "") {
        setTimeout(function () {
            self.LoadIdentityImage(identityImages);
        }, 1000);
    }
}