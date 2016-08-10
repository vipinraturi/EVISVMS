
function ScanVisitorViewModel() {

    var self = this;
    self.VisitorName = ko.observable('');
    self.Gender = ko.observable('');
    self.GenderText = ko.observable('');
    self.DOB = ko.observable('');
    self.TypeOfCard = ko.observable('');
    self.TypeOfCardText = ko.observable('');
    self.IdNumber = ko.observable('');
    self.Nationality = ko.observable('');
    self.NationalityText = ko.observable('');
    self.CompanyName = ko.observable('');
    self.EmailAddress = ko.observable('');
    self.ContactNumber = ko.observable('');
    self.IdentityImages = ko.observableArray('');

    self.ReadImageData = function () {
        debugger;
        if ($('.dz-filename').length == 0) {
            toastr.warning('No image available to read text.');
            return;
        }
        var firstimg = $('.dz-image-preview img').eq(0).attr("alt");
        var secoundimg = $('.dz-image-preview img').eq(1).attr("alt");
        var thirdimg = $('.dz-image-preview img').eq(2).attr("alt");
        var data = [];
        data.push(firstimg);
        data.push(secoundimg);
        data.push(thirdimg);
        AjaxCall('/Api/Visitor/ScanImage', data, 'POST', function (data) {
            debugger;
            self.VisitorName(data.VisitorName);
            self.Gender(data.Gender);
            self.GenderText(data.Gender);
            self.DOB(data.DateOfBirth);
            self.TypeOfCard(data.TypeOfCard);
            self.TypeOfCardText(data.TypeOfCard);
            self.IdNumber(data.IDNumber);
            self.Nationality(data.Nationality);
            self.NationalityText(data.Nationality);
            self.CompanyName('EVIS');
            self.EmailAddress('visitor@domain.com');
            self.ContactNumber('+971-2567789455');
            $('input[type=text]').removeAttr('readonly').removeClass('inputdisable');
            $('.dz-image img').each(function () {
                self.IdentityImages.push($(this).attr('alt'));
            });


        })

    
        self.PrepareData();
    }

    self.ResetImageData = function () {
        $('.dz-image-preview').empty();
        $('input[type=text]').attr('readonly', 'readonly').addClass('inputdisable');
        self.IdentityImages([]);
        self.VisitorName('');
        self.Gender('');
        self.GenderText('');
        self.DOB('');
        self.TypeOfCard('');
        self.TypeOfCardText('');
        self.IdNumber('');
        self.Nationality('');
        self.NationalityText('');
    }

    self.PrepareData = function () {
        dataToSend =
            (self.VisitorName() + "_" +
            self.Gender() + "_" +
            self.Nationality() + "_" +
            self.DOB() + "_" +
            self.TypeOfCard() + "_" +
            self.IdNumber() + "_" +
            self.Nationality() + "_" +
            self.CompanyName() + "_" +
            self.EmailAddress() + "_" +
            self.ContactNumber() + "_" +
            self.IdentityImages());
    }


    self.ContinueRegistration = function () {
        self.PrepareData();
        if (self.VisitorName() == "" || self.Gender() == '' || self.DOB() == '' || self.TypeOfCard() == '' || self.IdNumber() == '' || self.Nationality() == '') {
            toastr.warning('No scanned image data available to proceed.');
            return;
        }
        ApplyCustomBinding('managevisitor');
    }
    return dataToSend;
}