
function ShiftManagementdynamicViewModell() {
    var self = this;
    self.Buildings = ko.observableArray();
    self.Gates = ko.observableArray();

    self.Header = ko.observableArray();
    self.Body = ko.observableArray();

    self.BuildingId = ko.observable(-1);
    self.GateId = ko.observable(-1);
    self.ShiftDate = ko.observable('');
    self.NoOfDays = ko.observable(0);


    var request = new Object();
    request.BuildingId = self.BuildingId();
    request.GateId = self.GateId();
    request.ShiftDate = self.ShiftDate();
    request.NoOfDays = self.NoOfDays();

    AjaxCall('/Api/ShiftAssignment/GetAllShift', request, 'POST', function (data) {
        self.Header(data.Header);
        self.Body(data.Body);
    })



    AjaxCall('/Api/Gates/GetAllBuilding', null, 'GET', function (data) {
        self.Buildings(data);
    })

    self.GetGates = function () {
        //  
        if (self.BuildingId() != undefined && self.BuildingId() != 0) {
            AjaxCall('/Api/ShiftAssignment/GetAllGates?BuildingId=' + self.BuildingId(), null, 'GET', function (data) {
                self.Gates(new Object());
                self.Gates(data);
                //self.GateId(gateId);

            })
        }
    }


    self.GetUsers = function () {
    }

    self.ChangeShift = function () {
        if (gblTableId != '' && gblTableId != undefined) {
            if ($('#' + gblTableId).html().indexOf('fa-check-unique') != -1) {
                $('#' + gblTableId).html('');
            }
            else {
                $('#' + gblTableId).html(' <i class="fa fa-check fa-check-unique" aria-hidden="true"></i>')
            }

            $('#myModal').modal('hide');
            toastr.success('Shift changed successfully!!');
        }
       
    }
}