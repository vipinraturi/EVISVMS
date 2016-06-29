﻿var gblTEst = [];

function OrganizationViewModel() {
    var self = this;

    //self.Organizations = ko.observable();
    //AjaxCall('/Api/Administration/GetOrganizationsData', null, 'POST', function (data) {
    //    debugger;
    //    self.Organizations(data);
    //})

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
    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);


    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Administration/GetOrganizationsData', 7);

    self.OrganizationList = ko.observableArray([]);
    self.errors = ko.validation.group(self);

    self.GetAllOrganizations = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }

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

    self.DeleteOrganization = function (tableItem) {
        var message = confirm("Are you sure, you want to delete selected record!");
        if (message == true) {
            AjaxCall('/Api/Administration/DeleteOrganization', tableItem, 'POST', function () {
                toastr.success('Organization deleted successfully!!')
                ApplyCustomBinding('organization');
            });
        }
    }

    self.EditOrganization = function (tableItem) {
        //debugger;
        if (tableItem != undefined) {
            self.IsInsert(false);
            self.Id(tableItem.Id);
            self.CompanyName(tableItem.CompanyName);
            self.Email(tableItem.Email);
            self.ContactAddress(tableItem.ContactAddress);
            self.FaxNumber(tableItem.FaxNumber);
            self.ContactNumber(tableItem.ContactNumber);
            self.POBox(tableItem.POBox);
            self.WebSite(tableItem.WebSite);
        }
    }

    self.GlobalSearchEnter = function (data, event) {
        if (event.which == 13) {
            self.GetAllOrganizations();
            console.log(event);
        }
    }

    self.GetAllOrganizations();


    //$("form").validate({ submitHandler: self.SaveOrganization });

    //ko.validation.configure({
    //    insertMessages: true,
    //    decorateElement: true,
    //    errorElementClass: 'error',
    //    messagesOnModified: true
    //});

    //$('#txtGlobalSearch').keyup(function (e) {
    //    if (e.keyCode == 13) {
    //        alert('aa');
    //        self.GetAllOrganizations();
    //    }
    //});
}