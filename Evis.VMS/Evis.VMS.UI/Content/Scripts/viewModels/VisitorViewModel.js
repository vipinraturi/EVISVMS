
function VisitorViewModel() {

    var self = this;
    VisitorName = ko.observable('').extend({ required: true });
    EmailAddress = ko.observable('').extend({ required: true });
    Gender = ko.observable(-1).extend({ required: true });
    DOB = ko.observable('').extend({ required: true });
    TypeOfCard = ko.observable(-1).extend({ required: true });
    IdNo = ko.observable('').extend({ required: true });
    Nationality = ko.observable(-1).extend({ required: true });
    ContactNo = ko.observable('').extend({ required: true });
    ContactAddress = ko.observable('');

    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);


    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Visitor/GetVisitorData', 7);

    self.VisitorList = ko.observableArray([]);
    self.errors = ko.validation.group(self);

    self.GetAllVisitor = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
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
            //debugger;
            data.CompanyId = self.CompanyId(),
            data.CompanyName = self.CompanyName(),
            data.EmailAddress = self.EmailAddress(),
            data.ContactNo = self.ContactNo(),
            data.Address = self.Address(),
            data.FaxNo = self.FaxNo(),
            data.ZipCode = self.ZipCode(),
            data.WebSite = self.WebSite()
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

}