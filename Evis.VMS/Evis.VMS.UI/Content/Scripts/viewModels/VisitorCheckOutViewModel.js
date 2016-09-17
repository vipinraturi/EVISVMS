function VisitorCheckOutViewModel() {
    var self = this;
    self.VisitorId = ko.observable('');

    self.VisitorName = ko.observable('[Visitor Name]');
    self.Gender = ko.observable('[Gender]');
    self.DOB = ko.observable('[DOB]');
    self.MobileNo = ko.observable('[Mobile No]');
    self.EmailId = ko.observable('[EmailId]');
    self.IdentificationNo = ko.observable('[Identification No.]');
    self.Nationality = ko.observable('[Nationality]');
    self.Purpose_Remark = ko.observable('');
    self.logoURL = ko.observable('');
    self.IsAlreadyCheckIn = ko.observable(false);
    self.VisitorHiostory = ko.observableArray();
    self.TotalDuration = ko.observable('');
    self.TotalDuration = ko.observable('');


    self.CompanyName = ko.observable('');
    self.VahicleNumber = ko.observable('');
    self.Floor = ko.observable('');
    self.ContactPerson = ko.observable('');
    self.NoOfPerson = ko.observable('');
    self.Purpose_Remark = ko.observable('');

    self.GetVisitorCheckInHistoryData = function (visitorId, logoURL) {
        AjaxCall('/Api/VisitorManagement/GetVisitorCheckInHistory?visitorId=' + visitorId, null, 'POST', function (data) {
            $('.visitorImageUnique').attr('src', logoURL);
            self.logoURL(logoURL);
            self.VisitorId(data.VisitorId);
            self.VisitorName(data.VisitorName);
            self.Gender(data.Gender);
            self.DOB(data.DOB);
            self.MobileNo(data.MobileNo);
            self.EmailId(data.EmailId);
            self.IdentificationNo(data.IdentificationNo);
            self.CompanyName(data.CompanyName);
            self.Nationality(data.Nationality);
            self.VisitorHiostory(data.VisitorHiostory);
            self.IsAlreadyCheckIn(data.IsAlreadyCheckIn);
            self.TotalDuration(data.TotalDuration);

            //debugger;

            self.CompanyName(data.CompanyName);
            self.VahicleNumber(data.VahicleNumber);
            self.Floor(data.Floor);
            self.ContactPerson(data.ContactPerson);
            self.NoOfPerson(data.NoOfPerson);
            self.Purpose_Remark(data.Purpose);


            $('.searchVisitor').val('');
            //toastr.clear();toastr.success('Visitor data loaded!!');
        });
    }

    self.SaveVisitorCheckOut = function () {

        if (self.VisitorId() == '') {
            toastr.clear();toastr.warning('No visitor available to check-in.');
            return;
        }

        if (self.IsAlreadyCheckIn() == false) {
            toastr.clear();toastr.warning('Visitor not checked-in yet.');
            return;
        }


        var data = new Object();
        data.VisitorId = self.VisitorId();

        AjaxCall('/Api/VisitorManagement/SaveVisitorCheckOut', data, 'POST', function () {
            toastr.clear();toastr.success('Visitor CheckOut Successfully.!!');
            //alert(self.VisitorId() + '  ' + self.logoURL());
            self.GetVisitorCheckInHistoryData(self.VisitorId(), self.logoURL());
            //self.ResetCheckInData();
        })
    }


    self.ViewHistory = function (tableItem) {
        //debugger;
        self.ContactPerson(tableItem.ContactPerson);
        self.NoOfPerson(tableItem.NoOfPerson);
        self.Purpose_Remark(tableItem.Purpose);
        self.CompanyName(tableItem.CompanyName);
        self.VahicleNumber(tableItem.VahicleNumber);
        self.Floor(tableItem.Floor);
    }

    self.ResetCheckInData = function () {
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
        self.VisitorHiostory([]);
        $('.searchVisitor').val('');
    }
}

BindAutoCompleteEventCheckout = function () {
    function log(message) {
        $("<div>").text(message).prependTo("#log");
        $("#log").scrollTop(0);
    }

    $('.searchVisitorCheckout').autocomplete({
        source: function (request, response) {

            $.ajax({
                url: '/Visitor/GetVisitorsCheckOutData',
                data: { searchterm: request.term },
                success: function (data) {
                    if (data.length >0) {
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
                    else {
                        toastr.clear();
                        toastr.warning('Visitor not available or still not checked-in.');
                    }
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            self.GetVisitorCheckInHistoryData(ui.item.VisitorId, ui.item.logoUrl);
        },
        open: function () {
            $(this).removeClass('ui-corner-all').addClass('ui-corner-top');
        },
        close: function () {
            $(this).removeClass('ui-corner-top').addClass('ui-corner-all');
        }
    })
.autocomplete("instance")._renderItem = function (ul, item) {
    console.log(item.logoUrl);
    return $('<li>')
         .data('item.autocomplete', item)
         .append('<div  style="border: 1px solid black" class="row" ><div class=col-sm-9>'
                    + ' <strong>Visitor Name:</strong> ' + item.VisitorName + ', '
                    + ' <strong>Email:</strong> ' + item.Email + '<br /> '
                    + ' <strong>MobileNumber:</strong> ' + item.MobileNumber + ', '
                    + ' <strong>Indentity Number:</strong> ' + item.IndentityNumber
                    + '</div><div class=col-sm-3><img class="pull-right" width=80px src='
                    + item.logoUrl + ' alt="" /></div></div>')
         .appendTo(ul);
};
}

