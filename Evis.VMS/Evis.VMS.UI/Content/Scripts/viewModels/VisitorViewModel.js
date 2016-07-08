
function VisitorViewModel(visitorName, gender, nationality, DOB, typeOfCard, idNumber, nationality) {

    nationality = (nationality != "" ? nationality : undefined);
    typeOfCard = (typeOfCard != "" ? typeOfCard : undefined);
    gender = (gender != "" ? gender : undefined);

    //debugger;

    var self = this;
    Id = ko.observable(0);
    VisitorName = ko.observable(visitorName).extend({ required: true });
    EmailAddress = ko.observable('').extend({ required: true, email: { message: "Invalid email" } });
    Gender = ko.observable(gender).extend({ required: true });
    DOB = ko.observable(DOB).extend({ required: true });
    TypeOfCardValue  = ko.observable(typeOfCard).extend({ required: true });
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


    //[
    // { Text: 'Indian', Id: 1 },
    // { Text: 'Emirate', Id: 2 },
    // { Text: 'Others', Id: 3 }
    //]

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Visitor/GetVisitorData', 7);

    self.VisitorList = ko.observableArray([]);

    self.GetAllVisitor = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
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
            //debugger;
        })
    }

    self.SaveVisitor = function () {
        //alert(self.errors().length);
        //debugger;
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                toastr.warning(data);
            });
        }
        else {
            var data = new Object();
            ////debugger;

            data.VisitorName = self.VisitorName(),
            data.EmailAddress = self.EmailAddress(),
            data.Gender = self.Gender(),
            data.DOB = self.DOB(),
            data.TypeOfCard = self.TypeOfCardValue(),
            data.IdNo = self.IdNo(),
            data.Nationality = self.Nationality()
            data.ContactNo = self.ContactNo()
            data.ContactAddress = self.ContactAddress()
            data.IsInsert = self.IsInsert();

            //// display any error messages if we have them
            AjaxCall('/Api/Visitor/SaveVisitor', data, 'POST', function (result) {
                if (result.Success) {
                    toastr.success('Visitor saved successfully!!')
                    ApplyCustomBinding('managevisitor');
                    self.IsInsert(true);
                }
                else {
                    toastr.warning('Visitor email already exist!!')
                }
            })
        }
    }

    self.ResetVisitor = function () {
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
        ApplyCustomBinding('managevisitor');
    }

    self.DeleteVisitor = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected record!");
        if (message == true) {
            AjaxCall('/Api/Visitor/DeleteVisitor', tableItem, 'POST', function () {
                toastr.success('Visitor deleted successfully!!')
                ApplyCustomBinding('managevisitor');

            });
        }
    }

    self.EditVisitor = function (tableItem) {
        if (tableItem != undefined) {
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
        }
    }

    self.GlobalSearchEnter = function (data, event) {
        if (event.which == 13) {
            self.GetAllVisitor();
            console.log(event);
        }
    }

    self.SaveVisitorImage = function () {
        var formData = new FormData();
        var totalFiles = document.getElementById("avatarInputorVisitor").files.length;

        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("avatarInputorVisitor").files[i];
            formData.append("avatarInput", file);
        }

        $.ajax({
            type: "POST",
            url: '/Visitor/SaveVisitorImage',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                debugger;
                $('#imgUploaded').attr('src', response.FilePath);
                toastr.success('Image uploaded');
            },
            error: function (error) {
                alert("errror");
            }
        });
    }

    self.GetAllVisitor();
    self.LoadMasterData();

}