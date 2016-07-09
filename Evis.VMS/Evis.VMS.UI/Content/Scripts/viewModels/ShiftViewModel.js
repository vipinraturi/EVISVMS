function ShiftViewModel() {
    debugger;

    ko.validation.rules.pattern.message = 'Invalid.';
    ko.validation.configure({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null,
        decorateElement: true,
        errorElementClass: 'err'
    })

    var self = this;

    self.ShitfName = ko.observable().extend({
        required: { message: 'Gate Number is required' },
        deferValidation: true
    });
    self.Id = ko.observable(0);
    self.ShitfName = ko.observable('').extend({ required: true });
    self.FromTime = ko.observable('').extend({ required: true });
    self.ToTime = ko.observable('').extend({ required: true });
    self.GlobalSearch = ko.observable('');
    self.errors = ko.validation.group(
       {
           ShitfName: this.ShitfName,
           FromTime: this.FromTime,
           ToTime: this.ToTime,

       });
    self.errors = ko.validation.group(self);

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Shift/GetAllShift', 7);

    self.GetAllShiftData = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }


    self.Editswift = function (tableItem) {
        debugger;
        if (tableItem != undefined) {
            self.Id(tableItem.Id);
            self.ShitfName(tableItem.ShitfName);
            self.FromTime(tableItem.FromTime);
            self.ToTime(tableItem.ToTime);
        }
    }
    self.Saveswift = function () {
        debugger;
        var abc = self.ShitfName();
        abc = self.FromTime();
        abc = self.ToTime();
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                // toastr.warning(data);
            })
        }
        else {
            var data = new Object();
            data.Id = self.Id(),
            data.ShitfName = self.ShitfName(),
            data.FromTime = self.FromTime();
            data.ToTime = self.ToTime();
            AjaxCall('/Api/Swift/SaveShift', data, 'POST', function () {
                debugger;
                toastr.success('Shift saved successfully!!');
                ApplyCustomBinding('newshiftcreate');
            })
        }
    }
    self.Resetswift = function () {
        self.GlobalSearch('');
        self.ShitfName('');
        self.FromTime('');
        self.ToTime('');
        ApplyCustomBinding('newshiftcreate');
    }
    self.DeleteSwift = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected record!");
        if (message == true) {
            AjaxCall('/Api/Shift/DeleteShift', tableItem, 'POST', function () {
                debugger;
                toastr.success('Shift deleted successfully!!');
                ApplyCustomBinding('newshiftcreate');

            });
        }
    }

    debugger;
    self.GetAllShiftData();
}