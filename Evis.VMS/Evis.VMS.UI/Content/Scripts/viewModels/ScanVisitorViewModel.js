
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
        if ($('.dz-filename').length == 0) {
            toastr.warning('No image available to read text.');
            return;
        }

        self.VisitorName('Tintu John');
        self.Gender('1');
        self.GenderText('Male');
        self.DOB('01/04/1989');
        self.TypeOfCard('32');
        self.TypeOfCardText('Emirates Id');
        self.IdNumber('H8888SHSJHDF');
        self.Nationality('36');
        self.NationalityText('Indian');
        self.CompanyName('EVIS');
        self.EmailAddress('visitor@domain.com');
        self.ContactNumber('+971-2567789455');

        $('.dz-image img').each(function () {
            self.IdentityImages.push($(this).attr('alt'));
        });

        self.PrepareData();
    }

    self.ResetImageData = function () {
        $('.dz-image-preview').empty();
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

    self.PrepareData = function ()
    {
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
        if (self.VisitorName() == "" ||  self.Gender() == '' || self.DOB() == '' || self.TypeOfCard() =='' || self.IdNumber() == '' || self.Nationality() =='') {
            toastr.warning('No scanned image data available to proceed.');
            return;
        }
        ApplyCustomBinding('managevisitor');
    }
    return dataToSend;
} 