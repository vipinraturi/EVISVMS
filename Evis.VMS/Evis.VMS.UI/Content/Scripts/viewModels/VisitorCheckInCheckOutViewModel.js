function VisitorCheckInCheckOutViewModel() {
    var self = this;

    //------START-------Static AutoComplete Data------------------//
    var projects = [
      {
          value: "jquery",
          label: "jQuery",
          desc: "the write less, do more, JavaScript library",
          icon: "jquery_32x32.png"
      },
      {
          value: "jquery-ui",
          label: "jQuery UI",
          desc: "the official user interface library for jQuery",
          icon: "jqueryui_32x32.png"
      },
      {
          value: "sizzlejs",
          label: "Sizzle JS",
          desc: "a pure-JavaScript CSS selector engine",
          icon: "sizzlejs_32x32.png"
      }
    ];

    $("#project").autocomplete({
        minLength: 0,
        source: projects,
        focus: function (event, ui) {
            $("#project").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#project").val(ui.item.label);
            $("#project-id").val(ui.item.value);
            $("#project-description").html(ui.item.desc);
            $("#project-icon").attr("src", "images/" + ui.item.icon);

            return false;
        }
    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<a>" + item.label + "<br>" + item.desc + "</a>")
          .appendTo(ul);
    };

    //------END-------Static AutoComplete Data------------------//


    self.VisitorId = ko.observable('');
    self.VisitorName = ko.observable('Visitor Name');
    self.Gender = ko.observable('Gender');
    self.DOB = ko.observable('DOB');
    self.MobileNo = ko.observable('Mobile No');
    self.EmailId = ko.observable('EmailId');
    self.IdentificationNo = ko.observable(' Identification No.');
    self.Nationality = ko.observable('Nationality');
    self.ContactPerson = ko.observable('').extend({ required: true });
    self.NoOfPerson = ko.observable('').extend({ required: true });
    self.Purpose_Remark = ko.observable('').extend({ required: true });

    AjaxCall('/Api/VisitorManagement/GetVisitorDetail?globalSearch=' + self.MobileNo(), null, 'GET', function (data) {
        self.VisitorId = data.VisitorId;
        self.VisitorName = data.VisitorName;
        self.Gender = data.Gender;
        self.DOB = data.DOB;
        self.MobileNo = data.MobileNo;
        self.EmailId = data.EmailId;
        self.IdentificationNo = data.IdentificationNo;
        self.Nationality = data.Nationality;
    });

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/VisitorManagement/GetVisitorCheckInHistory', 7);

    self.GetVisitorCheckInHistoryData = function () {
        self.DataGrid.UpdateSearchParam('?visitorId=' + self.VisitorId());
        self.DataGrid.GetData();
    }

    self.SaveVisitorCheckIn = function () {
       
        var data = new Object();

        data.VisitorId = self.VisitorId();
        data.ContactPerson = self.ContactPerson();
        data.NoOfPerson = self.NoOfPerson();
        data.PurposeOfVisit = self.Purpose_Remark();
        data.CheckIn = self.
        data.CheckOut = self.
        data.CreatedBy = self.
        data.CheckInGate = self.
        data.CheckOutGate = self.
        
        AjaxCall('/Api/VisitorManagement/SaveVisitorCheckIn', data, 'POST', function () {
            toastr.success('Visitor CheckIn Successfully.!!');
            Self.ResetCheckInData();
        })
    }

    self.ResetCheckInData = function () {
        self.VisitorId('');
        self.VisitorName('Visitor Name');
        self.Gender('Gender');
        self.DOB('DOB');
        self.MobileNo('Mobile No');
        self.EmailId('EmailId');
        self.IdentificationNo(' Identification No.');
        self.Nationality('Nationality');
        self.ContactPerson('');
        self.NoOfPerson('');
        self.Purpose_Remark('');

        //ApplyCustomBinding('visitorCheckIn');
    }


}