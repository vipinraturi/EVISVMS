
function ScanVisitorViewModel() {

    var self = this;
    self.VisitorName = ko.observable('[Visitor Name]');
    self.Gender = ko.observable('');
    self.GenderText = ko.observable('[Gender]');
    self.DOB = ko.observable('[Date Of Birth]');
    self.TypeOfCard = ko.observable('');
    self.TypeOfCardText = ko.observable('[Type Of Card]');
    self.IdNumber = ko.observable('[Id Number]');
    self.Nationality = ko.observable('');
    self.NationalityText = ko.observable('[Nationality]');
    self.CompanyName = ko.observable('[Company Name]');
    self.EmailAddress = ko.observable('[Email Id]');
    self.ContactNumber = ko.observable('[Contact Number]');
    self.IdentityImages = ko.observable('');

    self.ReadImageData = function () {
        if ($('.dz-filename').length == 0) {
            toastr.clear();toastr.warning('No image available to read text.');
            return;
        }
        var firstimg = $('.dz-image-preview img').eq(0).attr("img-name-unique");
        var secoundimg = $('.dz-image-preview img').eq(1).attr("img-name-unique");
        var thirdimg = $('.dz-image-preview img').eq(2).attr("img-name-unique");
        var data = [];
        data.push(firstimg);
        data.push(secoundimg);
        data.push(thirdimg);
        ShowLoader();
        AjaxCall('/Api/Visitor/ScanImage', data, 'POST', function (data) {
            
            if (data.Gender != undefined) {
                var gender = data.Gender.trim().toLowerCase();
                if (gender == "m" || gender == "male") {
                    self.Gender(1);
                }
                else if (gender == "f" || gender == "female") {
                    self.Gender(2);
                }
                else {
                    self.TypeOfCard('-1');
                }
                self.GenderText(data.Gender);
            }

            if (data.TypeOfCard != undefined) {
                var typeOfCardVal = data.TypeOfCard.trim().toLowerCase();
                if (typeOfCardVal.indexOf("emirates") != -1) {
                    self.TypeOfCard('32');//Emirates
                }
                else if (typeOfCardVal.indexOf("license") != -1) {
                    self.TypeOfCard('36');//Driving licence
                }
                else {
                    self.TypeOfCard('-1');
                }
                self.TypeOfCardText(data.TypeOfCard);

            }

          
                if (data.Nationality != undefined) {

                self.NationalityText(data.Nationality);
                self.Nationality(data.NationalityId);
            }
            //if (data.Nationality != undefined) {
            //    var nationalityVal = data.Nationality.trim().toLowerCase();
            //    if (nationalityVal.indexOf("arab") != -1) {
            //        self.Nationality('35');//UAE
            //    }
            //    else if (nationalityVal.indexOf("ind") != -1) {
            //        self.Nationality('32');//Emirates
            //    }
            //    else {
            //        self.Nationality('-1');
            //       //self.TypeOfCard('-1');
            //    }
            //    self.NationalityText(data.Nationality);
            //}

            if (data.VisitorName != undefined) {
                self.VisitorName(data.VisitorName);
            }
            if (data.IDNumber != undefined) {
                self.IdNumber(data.IDNumber);
            }
            if (data.DateOfBirth != undefined) {
                self.DOB(data.DateOfBirth);
            }
            if (data.CompanyName != undefined) {
                self.CompanyName(data.CompanyName);
            }
            if (data.EmailAddress != undefined) {
                self.EmailAddress(data.EmailAddress);
            }
            if (data.ContactNumber != undefined) {
                self.ContactNumber(data.ContactNumber);
            }
            $('#txtFirstName').focus();
            //$('input[type=text]').removeAttr('readonly').removeClass('inputdisable');

            $('.dz-image img').each(function () {
                
                if (self.IdentityImages().indexOf(",") == -1) {
                    self.IdentityImages($(this).attr('img-name-unique')+",");
                }
                else {
                    self.IdentityImages(self.IdentityImages() + $(this).attr('img-name-unique') + ",");
                }
            });

            HideLoader();
        })


        //self.PrepareData();
    }

    self.ResetImageData = function () {
        $('.dz-image-preview').empty();
        $('input[type=text]').attr('readonly', 'readonly').addClass('inputdisable');
        self.IdentityImages('');
        self.VisitorName('');
        self.Gender('');
        self.GenderText('');
        self.DOB('');
        self.TypeOfCard('');
        self.TypeOfCardText('');
        self.IdNumber('');
        self.CompanyName('');
        self.EmailAddress('');
        self.ContactNumber('');
        self.Nationality('');
        self.NationalityText('');
    }

    self.PrepareData = function () {
       
        dataToSend =
            (self.VisitorName() + "&&" +
            self.Gender() + "&&" +
            self.Nationality() + "&&" +
            self.DOB() + "&&   " +
            self.TypeOfCard() + "&&" +
            self.IdNumber() + "&&" +
            self.Nationality() + "&&" +
            self.CompanyName() + "&&" +
            self.EmailAddress() + "&&" +
            self.ContactNumber() + "&&" +
            self.IdentityImages());
        alert(dataToSend);
        
    }


    self.ContinueRegistration = function () {
        self.PrepareData();
        if (self.VisitorName() == ""
            && self.Gender() == ''
            && self.DOB() == ''
            && self.TypeOfCard() == ''
            && self.IdNumber() == ''
            && self.Nationality() == '') {
            toastr.clear();toastr.warning('No scanned image data available to proceed.');
            return;
        }
        ApplyCustomBinding('managevisitor');
    }
    return dataToSend;
}