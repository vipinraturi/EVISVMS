

function VisitorViewModel(visitorName, gender, nationalityVal, dateOfBirth, typeOfCard, idNumber, nationalityVal) {

    nationality = (nationalityVal != "" ? nationalityVal : undefined);
    typeOfCard = (typeOfCard != "" ? typeOfCard : undefined);
    gender = (gender != "" ? gender : undefined);

    //alert(nationality + ' ' + typeOfCard + ' ' + gender);

    var self = this;
    Id = ko.observable(0);
    VisitorName = ko.observable(visitorName).extend({ required: true });
    EmailAddress = ko.observable('').extend({ required: true, email: { message: "Invalid email" } });
    Gender = ko.observable(gender).extend({ required: true });
    DOB = ko.observable(dateOfBirth).extend({ required: true });
    TypeOfCardValue = ko.observable(typeOfCard).extend({ required: true });
    IdNo = ko.observable(idNumber).extend({ required: true });
    Nationality = ko.observable(nationality).extend({ required: true });
    ContactNo = ko.observable('').extend({ required: true });
    ContactAddress = ko.observable('');

    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);
    self.LookUpValues = ko.observableArray();
    self.Genders = ko.observableArray();
    self.TypeOfCards = ko.observableArray();
    self.Nationalities = ko.observableArray();
    self.errors = ko.validation.group({
        VisitorName: this.VisitorName,
        EmailAddress: this.EmailAddress,
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
        //alert(gblParam);
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
            data.VisitorName = self.VisitorName(),
            data.EmailAddress = self.EmailAddress(),
            data.Gender = self.Gender(),
            data.DOB = self.DOB(),
            data.TypeOfCard = self.TypeOfCardValue(),
            data.IdNo = self.IdNo(),
            data.Nationality = self.Nationality()
            data.ContactNo = self.ContactNo();
            data.ImagePath = $('.dz-image img').attr('alt');
            data.ContactAddress = self.ContactAddress()
            data.IsInsert = self.IsInsert();

            //// display any error messages if we have them
            AjaxCall('/Api/Visitor/SaveVisitor', data, 'POST', function (result) {
                if (result.Success) {
                    toastr.success('Visitor saved successfully!!')
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
        self.IsInsert(true);
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
            $('#viewVisitorImageUnique').show();
            $('.img_responsive_Avatar').removeAttr('src')
            self.IsInsert(false);
            self.VisitorName(tableItem.VisitorName);
            self.EmailAddress(tableItem.EmailAddress);
            self.Gender(tableItem.Gender);
            self.DOB(tableItem.DOB);
            self.TypeOfCardValue(tableItem.TypeOfCard);
            self.IdNo(tableItem.IdNo);
            self.Nationality(tableItem.Nationality);
            self.ContactNo(tableItem.ContactNo);
            self.ContactAddress(tableItem.ContactAddress);
            $('.img_responsive_Avatar').attr('src', '/images/VisitorImages/' + tableItem.ImagePath).addClass('dz-message')
        }
    }

    self.ViewVisitorImage = function () {
        var srcURL = '';
        if ($('.dz-image img').attr('alt') != undefined) {
            srcURL = ($('.dz-image img').attr('alt'));
        }
        else if ($('.img_responsive_Avatar').attr('src') != undefined) {
            srcURL = ($('.img_responsive_Avatar').attr('src'));
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
}