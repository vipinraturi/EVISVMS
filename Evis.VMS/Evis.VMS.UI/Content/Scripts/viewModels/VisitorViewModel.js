
function VisitorViewModel() {

    var self = this;
    Id = ko.observable(0);
    VisitorName = ko.observable('').extend({ required: true });
    EmailAddress = ko.observable('').extend({ required: true, email: { message: "Invalid email" } });
    Gender = ko.observable(undefined).extend({ required: true });
    DOB = ko.observable('').extend({ required: true });
    TypeOfCard = ko.observable(undefined).extend({ required: true });
    IdNo = ko.observable('').extend({ required: true });
    Nationality = ko.observable(undefined).extend({ required: true });
    ContactNo = ko.observable('').extend({ required: true });
    ContactAddress = ko.observable('');

    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);
    self.LookUpValues = ko.observableArray();
    self.Genders = ko.observableArray();
    self.TypeOfCards = ko.observableArray();
    self.Nationalities = ko.observableArray();

    //[
    // { Text: 'Indian', Id: 1 },
    // { Text: 'Emirate', Id: 2 },
    // { Text: 'Others', Id: 3 }
    //]

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Visitor/GetVisitorData', 7);

    self.VisitorList = ko.observableArray([]);
    self.errors = ko.validation.group(self);

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
        //debugger;
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                //toastr.warning(data);
            });
        }
        else {
            var data = new Object();
            ////debugger;

            data.VisitorName = self.VisitorName(),
            data.EmailAddress = self.EmailAddress(),
            data.Gender = self.Gender(),
            data.DOB = self.DOB(),
            data.TypeOfCard = self.TypeOfCard(),
            data.IdNo = self.IdNo(),
            data.Nationality = self.Nationality()
            data.ContactNo = self.ContactNo()
            data.ContactAddress = self.ContactAddress()
            data.IsInsert = self.IsInsert();

            //// display any error messages if we have them
            AjaxCall('/Api/Visitor/SaveVisitor', data, 'POST', function () {
                toastr.success('Visitor saved successfully!!')
                ApplyCustomBinding('managevisitor');
                self.IsInsert(true);
            })
        }
    }

    self.ResetVisitor = function () {
        self.IsInsert(true);
        self.GlobalSearch('');
        self.VisitorName('');
        self.Gender(-1);
        self.DOB('');
        self.TypeOfCard(-1);
        self.IdNo('');
        self.EmailAddress('');
        self.Nationality(-1);
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
            self.TypeOfCard(tableItem.TypeOfCard);
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

    self.GetAllVisitor();
    self.LoadMasterData();

}