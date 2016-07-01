function MyOrganizationViewModel() {
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
    });



    self.Id = ko.observable(0);
    self.CompanyName = ko.observable().extend({
        required: true,
        deferValidation: true
    });
    self.Email = ko.observable('').extend({ required: true, minLength: 2, maxLength: 40, email: { message: "Invalid email" } });
    self.ContactNumber = ko.observable('').extend({ required: true, number: { message: "Numbers only" } });
    self.ContactAddress = ko.observable('').extend({ required: true });
    self.FaxNumber = ko.observable('').extend({ required: true });
    self.POBox = ko.observable('').extend({ required: true });
    self.WebSite = ko.observable('').extend({ required: true });
    self.IsInsert = ko.observable(true);


    self.SaveOrganization = function () {
        debugger;
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
                //toastr.warning(data);
            });
        }
        else {
            var data = new Object();
            //debugger;
            data.Id = self.Id(),
            data.CompanyName = self.CompanyName(),
            data.Email = self.Email(),
            data.ContactNumber = self.ContactNumber(),
            data.ContactAddress = self.ContactAddress(),
            data.FaxNumber = self.FaxNumber(),
            data.POBox = self.POBox(),
            data.WebSite = self.WebSite()
            data.IsInsert = self.IsInsert();

            //// display any error messages if we have them
            AjaxCall('/Api/Administration/SaveOrganization', data, 'POST', function () {
                toastr.success('Organization saved successfully!!')
                ApplyCustomBinding('organization');
                self.IsInsert(true);
            })
        }
    }


    self.ResetOrganization = function () {
        self.IsInsert(true);
        self.GlobalSearch('');
        self.CompanyName('');
        self.Email('');
        self.ContactAddress('');
        self.FaxNumber('');
        self.ContactNumber('');
        self.POBox('');
        self.WebSite('');
        ApplyCustomBinding('organization');
    }

}