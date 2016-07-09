
function ScanVisitorViewModel() {
    var self = this;
    self.VisitorName = ko.observable('');
    self.Gender = ko.observable('');
    self.DOB = ko.observable('');
    self.TypeOfCard = ko.observable('');
    self.IdNumber = ko.observable('');
    self.Nationality = ko.observable('');

    self.ReadImageData = function () {
        if ($('.dz-filename').length == 0) {
            toastr.warning('No image available to read text.');
            return;
        }

        self.VisitorName('Tintu John');
        self.Gender('1');
        self.DOB('01/04/1989');
        self.TypeOfCard('32');
        self.IdNumber('H8888SHSJHDF');
        self.Nationality('36');
        dataToSend = self.VisitorName() + "_" + self.Gender() + "_" + self.Nationality() + "_" + self.DOB()
        + "_" + self.TypeOfCard() + "_" + self.IdNumber() + "_" + self.Nationality();

    }

    self.ResetImageData = function () {
        $('.dz-image-preview').empty();
        self.VisitorName('');
        self.Gender('');
        self.DOB('');
        self.TypeOfCard('');
        self.IdNumber('');
        self.Nationality('');
        dataToSend = self.VisitorName() + "_" + self.Gender() + "_" + self.Nationality() + "_" + self.DOB()
        + "_" + self.TypeOfCard() + "_" + self.IdNumber() + "_" + self.Nationality();
    }

    self.ContinueRegistration = function () {

        if (self.VisitorName() == "" ||  self.Gender() == '' || self.DOB() == '' || self.TypeOfCard() =='' || self.IdNumber() == '' || self.Nationality() =='') {
            toastr.warning('No scanned image data available to proceed.');
            return;
        }

        ApplyCustomBinding('managevisitor');
    }

    //debugger;
    return dataToSend;
} 