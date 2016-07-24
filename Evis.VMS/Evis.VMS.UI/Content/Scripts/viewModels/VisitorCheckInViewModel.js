function VisitorCheckInViewModel() {
    var self = this;
    self.VisitorId = ko.observable('');
    self.VisitorName = ko.observable('[Visitor Name]');
    self.Gender = ko.observable('[Gender]');
    self.DOB = ko.observable('[DOB]');
    self.MobileNo = ko.observable('[Mobile No]');
    self.EmailId = ko.observable('[EmailId]');
    self.IdentificationNo = ko.observable('[Identification No.]');
    self.Nationality = ko.observable('[Nationality]');
    self.ContactPerson = ko.observable('').extend({ required: true });
    self.NoOfPerson = ko.observable('').extend({ required: true });
    self.Purpose_Remark = ko.observable('');
    self.logoURL = ko.observable('');
    self.CompanyName = ko.observable('');
    self.VahicleNumber = ko.observable('');
    self.Floor = ko.observable('');
    self.IsAlreadyCheckIn = ko.observable(false);

    self.VisitorHiostory = ko.observableArray();
    self.IsSecurityPerson = ko.observable(false);
    self.IsAnyGateExist = ko.observable(false);

    self.errors = ko.validation.group({
        ContactPerson: this.ContactPerson,
        NoOfPerson: this.NoOfPerson
    });

    self.GetVisitorCheckInHistoryData = function (visitorId, logoURL, isResetFields) {
        AjaxCall('/Api/VisitorManagement/GetVisitorCheckInHistory?visitorId=' + visitorId, null, 'POST', function (data) {
            $('.img-responsive').attr('src', logoURL);
            self.logoURL(logoURL);
            self.VisitorId(data.VisitorId);
            self.VisitorName(data.VisitorName);
            self.Gender(data.Gender);
            self.DOB(data.DOB);
            self.MobileNo(data.MobileNo);
            self.EmailId(data.EmailId);
            self.IdentificationNo(data.IdentificationNo);
            self.Nationality(data.Nationality);
            self.VisitorHiostory(data.VisitorHiostory);
            self.IsSecurityPerson(data.IsSecurityPerson);
            self.IsAnyGateExist(data.IsAnyGateExist);
            //alert(data.IsAlreadyCheckIn);
            self.IsAlreadyCheckIn(data.IsAlreadyCheckIn);

            $('.searchVisitor').val('');
            toastr.success('Visitor data loaded!!');

            if (isResetFields) {
                self.ResetCheckInFormData();
            }
        });
    }

    self.ViewHistory = function (tableItem) {
        debugger;
        self.ContactPerson(tableItem.ContactPerson);
        self.NoOfPerson(tableItem.NoOfPerson);
        self.Purpose_Remark(tableItem.Purpose_Remark);
        self.CompanyName(tableItem.CompanyName);
        self.VahicleNumber(tableItem.VahicleNumber);
        self.Floor('');
    }

    self.SaveVisitorCheckIn = function () {
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
            });
        }
        else {
            if (self.VisitorId() == '') {
                toastr.warning('No visitor available to check-in.');
                return;
            }

            if (self.IsSecurityPerson() == false) {
                toastr.warning('Only security role can check-in visitor.');
                return;
            }

            if (self.IsAnyGateExist() == false) {
                toastr.warning('No gate exist.');
                return;
            }

            if (self.IsAlreadyCheckIn() == false) {
                toastr.warning('Visitor already checked-in.');
                return;
            }

            var data = new Object();
            data.VisitorId = self.VisitorId();
            data.ContactPerson = self.ContactPerson();
            data.NoOfPerson = self.NoOfPerson();
            data.PurposeOfVisit = self.Purpose_Remark();
            data.CompanyName = self.CompanyName();
            data.VahicleNumber = self.VahicleNumber();
            data.Floor = self.Floor();
            
            AjaxCall('/Api/VisitorManagement/SaveVisitorCheckIn', data, 'POST', function () {
                toastr.success('Visitor CheckIn Successfully.!!');
                //alert(self.VisitorId() + '  ' + self.logoURL());
                self.GetVisitorCheckInHistoryData(self.VisitorId(), self.logoURL(), true)
                
            })
        }
    }


    self.ResetCheckInFormData = function () {
        //self.VisitorName('[Visitor Name]');
        //self.Gender('[Gender]');
        //self.DOB('[DOB]');
        //self.MobileNo('[Mobile No]');
        //self.EmailId('[EmailId]');
        //self.IdentificationNo('[Identification No.]');
        //self.Nationality(' [Nationality]');
        self.ContactPerson('');
        self.NoOfPerson('');
        self.Purpose_Remark('');
        self.CompanyName('');
        self.VahicleNumber('');
        self.Floor('');
        $('.searchVisitor').val('');
        //$('.img-responsive').attr('src', '');
    }

    self.ResetCheckInData = function ()
    {
        self.VisitorId('');
        self.VisitorName('[Visitor Name]');
        self.Gender('[Gender]');
        self.DOB('[DOB]');
        self.MobileNo('[Mobile No]');
        self.EmailId('[EmailId]');
        self.IdentificationNo('[Identification No.]');
        self.Nationality(' [Nationality]');
        self.ContactPerson('');
        self.NoOfPerson('');
        self.Purpose_Remark('');
        self.CompanyName('');
        self.VahicleNumber('');
        self.Floor('');
        self.VisitorHiostory([]);
        $('.searchVisitor').val('');
        $('.img-responsive').attr('src', '');
    }
}

BindAutoCompleteEvent = function () {
    function log(message) {
        $("<div>").text(message).prependTo("#log");
        $("#log").scrollTop(0);
    }

    $('.searchVisitor').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Visitor/GetCompanyNames',
                data: { searchterm: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            VisitorId: item.VisitorId,
                            VisitorName: item.VisitorName,
                            Email: item.Email,
                            MobileNumber: item.MobileNumber,
                            IndentityNumber: item.IndentityNumber,
                            logoUrl: item.LogoUrl
                        };
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            self.GetVisitorCheckInHistoryData(ui.item.VisitorId, ui.item.logoUrl, false);
        },
        open: function () {
            $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
        },
        close: function () {
            $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
        }
    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $('<li>')
         .data('item.autocomplete', item)
         .append('<div  style="border: 1px solid black" class="row" ><div class=col-sm-8>'
                    + ' Visitor Name: '+ item.VisitorName + '<br>'
                    + ' Email: ' + item.Email + '<br>'
                    + ' MobileNumber: ' + item.MobileNumber + '<br>'
                    + ' Indentity Number: ' + item.IndentityNumber
                    + '</div><div class=col-sm-4><img class="pull-right" width=80px src='
                    + item.logoUrl + ' alt="" /></div></div>')
         .appendTo(ul);
};
}

