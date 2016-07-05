function ShiftAssignmentViewModel() {
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
    self.BuildingId = ko.observable(undefined).extend({ required: true });
    self.GateId = ko.observable(undefined).extend({ required: true });
    self.GetAllShiftAssignment = ko.observableArray();
    AjaxCall('/Api/Users/GetAllShiftAssignment', null, 'GET', function (data) {
        self.GetAllShiftAssignment(data);
    })
    self.errors = ko.validation.group(self);
    self.Id = ko.observable(0);
    self.Gates = ko.observableArray();
    AjaxCall('/Api/ShiftAssignment/GetAllShiftAssignment', null, 'GET', function (data) {
        debugger;
        self.Gates(data);

    })
    self.Buildings = ko.observableArray();
    AjaxCall('/Api/Gates/GetAllBuilding', null, 'GET', function (data) {
        //debugger;;
        self.Buildings(data);
    })
}