function ScanVisitorViewModel() {
    var self = this;
    self.VisitorName = ko.observable('').extend({ required: true });
    self.Gender = ko.observable('').extend({ required: true });
    self.DOB = ko.observable('').extend({ required: true });
    self.TypeOfCard = ko.observable('').extend({ required: true });
    self.IdNumber = ko.observable('').extend({ required: true });
    self.Nationality = ko.observable('').extend({ required: true });

    return self.VisitorName() + "_" + self.Gender() + "_" + self.DOB() + "_" + self.TypeOfCard()
        + "_" + self.IdNumber() + "_" + self.Nationality();
} 