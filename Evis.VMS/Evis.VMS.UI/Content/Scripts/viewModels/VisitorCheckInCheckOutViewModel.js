function VisitorCheckInCheckOutViewModel() {
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
    self.VisitorHiostory = ko.observableArray();

    self.errors = ko.validation.group({
        ContactPerson: this.ContactPerson,
        NoOfPerson: this.NoOfPerson
    });

    self.GetVisitorCheckInHistoryData = function (visitorId, logoURL) {
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
            $('.searchVisitor').val('');
        });
    }

    self.SaveVisitorCheckIn = function () {
        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            this.errors().forEach(function (data) {
            });
        }
        else {
            if (self.VisitorId() == '') {
                toastr.message('No visitor available to check-in.');
                return;
            }

            var data = new Object();
            data.VisitorId = self.VisitorId();
            data.ContactPerson = self.ContactPerson();
            data.NoOfPerson = self.NoOfPerson();
            data.PurposeOfVisit = self.Purpose_Remark();
            AjaxCall('/Api/VisitorManagement/SaveVisitorCheckIn', data, 'POST', function () {
                toastr.success('Visitor CheckIn Successfully.!!');
                self.GetVisitorCheckInHistoryData(self.VisitorId(), self.logoURL())
                self.ResetCheckInData();
            })
        }
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
        self.VisitorHiostory = ko.observableArray([]);
        $('.searchVisitor').val('');
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
                            Id: item.Value,
                            label: item.Name,
                            value: item.Name,
                            logoUrl: item.LogoUrl
                        };
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            self.GetVisitorCheckInHistoryData(ui.item.Id, ui.item.logoUrl);
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
         .append(item.label + '<img class="pull-right" width=100px src=' + item.logoUrl + ' alt="" />')
         .appendTo(ul);
};
}

