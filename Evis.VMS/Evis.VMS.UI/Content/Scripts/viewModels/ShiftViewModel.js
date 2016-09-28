function ShiftViewModel() {
    var self = this;
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
    self.Id = ko.observable(0);
    self.ShitfName = ko.observable('').extend({ required: true });
    self.FromTime = ko.observable('').extend({ required: true });
    self.strFromTime = ko.observable('').extend({ required: true });
    self.ToTime = ko.observable('').extend({ required: true });
    self.strToTime = ko.observable('').extend({ required: true });
    self.GlobalSearch = ko.observable('');
    self.errors = ko.validation.group(
       {
           ShitfName: this.ShitfName,
           FromTime: this.strFromTime,
           ToTime: this.strToTime,

       });
    //self.errors = ko.validation.group(self);
    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Shift/GetAllShift', 7);
    self.GetAllShiftData = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }
    self.Editshift = function (tableItem) {
        
        if (tableItem != undefined) {
            self.Id(tableItem.Id);
            self.ShitfName(tableItem.ShitfName);
            self.strFromTime(tableItem.strFromTime);
            self.strToTime(tableItem.strToTime);
            self.FromTime(tableItem.FromTime);
            self.ToTime(tableItem.ToTime);
            $("#btnSaveShift").text("Update");
        }
    }
    self.Saveshift = function () {
        
        if (self.errors().length > 0) {
            // alert(self.errors());
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                // toastr.warning(data);
            })
        }
        else {
            var data = new Object();
            data.Id = self.Id(),
            data.ShitfName = self.ShitfName(),
            //data.FromTime = self.FromTime();
            //data.ToTime = self.ToTime();
            data.FromTime = self.strFromTime();
            data.ToTime = self.strToTime();
            AjaxCall('/Api/Shift/SaveShift', data, 'POST', function (data) {
                if (data.Success == true) {
                    toastr.clear();
                    
                    ApplyCustomBinding('newshiftcreate');
                    toastr.success(data.Message);
                }
                else if (data.Success == false) {
                    //self.ShitfName('');
                    //self.FromTime('');
                    //self.ToTime('');
                    //self.strFromTime('');
                    //self.strToTime('');
                    toastr.error(data.Message)
                }
            })
        }
    }
    self.Resetshift = function () {
        self.GlobalSearch('');
        self.ShitfName('');
        self.FromTime('');
        self.ToTime('');
        ApplyCustomBinding('newshiftcreate');
    }
    self.DeleteSwift = function (tableItem) {

        //var message = confirm("Are you sure, you want to delete selected record!");
        //if (message == true) {
        //    AjaxCall('/Api/Shift/DeleteShift', tableItem, 'POST', function () {
        //        
        //        toastr.success('Shift deleted successfully!!');
        //        ApplyCustomBinding('newshiftcreate');

        //    });
        //}
        recordToDelete = tableItem;
    }
    self.DeleteConfirmed = function () {
        $('#myModal').modal('hide');
        $('.modal-backdrop').modal('show');
        $('.modal-backdrop').modal('hide');
        AjaxCall('/Api/Shift/DeleteShift', recordToDelete, 'POST', function (data) {
            if (data.Success == true) {
                toastr.clear();
                toastr.success(data.Message);
                ApplyCustomBinding('newshiftcreate');
            }
            else if (data.Success == false) {
                toastr.clear();
                toastr.warning(data.Message);
            }

        });
    }

    self.GlobalSearchEnter = function (data) {
        
        self.GetAllShiftData();
        //console.log(event);
    }
    ko.bindingHandlers.enterkey = {
        init: function (element, valueAccessor, allBindings, viewModel) {
            var callback = valueAccessor();
            $(element).keypress(function (event) {
                var keyCode = (event.which ? event.which : event.keyCode);
                if (keyCode === 13) {
                    callback.call(viewModel);
                    return false;
                }
                return true;
            });
        }
    };
    self.GetAllShiftData();
}