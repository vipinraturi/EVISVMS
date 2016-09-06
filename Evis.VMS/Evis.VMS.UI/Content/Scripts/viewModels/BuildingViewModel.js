function BuildingViewModel() {
    var self = this;
    var stateId = 0;
    var cityId = 0;
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
    self.BuildingName = ko.observable().extend({
        required: { message: 'BuildingName name is required' },
        deferValidation: true
    });
    self.BuildingName = ko.observable('').extend({ required: true });
    //self.StateName = ko.observable('').extend({ required: true });
    self.Address = ko.observable('').extend({ required: true });
    self.ZipCode = ko.observable('').extend({
        required: true,
        pattern: {
            message: "Invalid zip code.",
            params: /^([0-9\(\)\/\+ \-\.]*)$/
        },
        minLength: {
            params: 6,
            message: 'Enter minimum of 6-length number'
        }
    });
    self.EmailId = ko.observable('').extend({ minLength: 2, maxLength: 40, email: { message: "Invalid email" } });
    self.ContactNumber = ko.observable('').extend({
        required: true,
        pattern: {
            message: 'Invalid phone number.',
            params: /^\+?([0-9\(\)\/\-\.]*)$/
        },
        minLength: {
            params: 6,
            message: 'Enter minimum of 6-length number'
        }
    });

    self.FaxNumber = ko.observable('').extend({
        pattern: {
            message: 'Invalid Fax Number.',
            params: /^([0-9\(\)\/\+ \-\.]*)$/
        },
        minLength: {
            params: 8,
            message: 'Enter minimum of 8-length number'
        }
    });
    self.WebSite = ko.observable('').extend({ url: true });
    ko.validation.rules['url'] = {
        validator: function (val, required) {
            if (!val) {
                return !required
            }
            val = val.replace(/^\s+|\s+$/, ''); //Strip whitespace
            return val.match(/[-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?/gi);
        },
        message: 'This field has to be a valid URL'
    };
    ko.validation.registerExtenders();

    //self.Nationality = ko.observable('').extend({ required: true });
    self.Countrydlltxt = ko.observable();
    self.statedlltxt = ko.observable();
    self.citydlltxt = ko.observable();
    self.StateId = ko.observable();
    self.CityId = ko.observable();
    self.OrganizationName = ko.observable();
    self.NationalityId = ko.observable(undefined);
    self.OrganizationId = ko.observable(undefined).extend({ required: true });
    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);
    self.State = ko.observableArray();
    self.errors = ko.validation.group(
        {
            BuildingName: this.BuildingName,
            //StateName: this.StateName,
            Address: this.Address,
            ZipCode: this.ZipCode,
            //Nationality: this.Nationality,
            StateId: this.StateId,
            CityId: this.CityId,
            NationalityId: this.NationalityId,
            OrganizationId: this.OrganizationId,
            EmailId: this.EmailId,
            ContactNumber: this.ContactNumber,
            FaxNumber: this.FaxNumber,
            statedlltxt: this.statedlltxt,
            citydlltxt: this.citydlltxt
            //  WebSite: this.WebSite
        });
    self.Organizations = ko.observableArray();
    AjaxCall('/Api/User/GetAllOrganizations', null, 'GET', function (data) {
        console.log(data);
        debugger;
        // alert(data.length);
        self.Organizations(data);
        if (data.length == 1) {
            // alert(data[0].Name);
            self.OrganizationId(data[0].Id);
            $("#Org").attr('disabled', false);
        }
    });
    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Administration/GetBuildingData', 7);
    self.GetAllBuildingData = function () {
        //debugger;
        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }
    self.Countries = ko.observableArray();
    AjaxCall('/Api/Users/GetAllCountries', null, 'GET', function (data) {
        self.Countries(data);
    })
    self.City = ko.observableArray();
    self.LoadCities = function () {
        if (self.StateId() != undefined && self.StateId() != 0) {
            AjaxCall('/Api/Administration/GetAllStateOrCity?id=' + self.StateId(), null, 'GET', function (data) {
                if (data.length > 0) {
                    self.City(data);
                    self.CityId(cityId);
                }
                else {
                    document.getElementById('dropcity').style.visibility = 'visible';
                    $('#dropcountry').hide();
                    $('#dropcity').hide();
                    $('#dropstate').hide();
                }
            })
        }
    }

    self.LoadStates = function () {
        // alert(self.NationalityId());
        if (self.NationalityId() != 11) {
            if (self.NationalityId() != undefined && self.NationalityId() != 0) {
                AjaxCall('/Api/Administration/GetAllStateOrCity?id=' + self.NationalityId(), null, 'GET', function (data) {
                    if (data.length > 0) {
                        debugger;
                        self.State(data);
                        self.StateId(stateId);
                        $('#dropcountry').hide();
                        $('#dropcity').hide();
                        $('#dropstate').hide();
                        $('#state').show();
                        $('#city').show();
                        $(".ErrorCountryd").hide();
                    }
                    else {
                        $('#state').hide();
                        $('#city').hide();
                        self.Countrydlltxt('');
                        self.statedlltxt('');
                        self.citydlltxt('');
                        self.StateId(undefined);
                        self.CityId(undefined);
                        $('#dropcountry').show();
                        $('#dropcity').show();
                        $('#dropstate').show();
                    }
                })
            }
        }
    }
    self.SaveBuilding = function () {
        var abc = self.BuildingName();
        abc = self.WebSite();
        abc = self.Address();
        abc = self.ZipCode();
        //abc = self.Nationality();
        abc = self.StateId();
        abc = self.CityId();
        abc = self.NationalityId();
        abc = self.OrganizationId();
        abc = self.EmailId;
        abc = self.Countrydlltxt;
        abc = self.statedlltxt;
        abc = self.citydlltxt;

        var a = $("#selectCountries option:selected").text();

        if (a == "Others") {
            var txtcountry = $("#txtcountry").val();
            if (txtcountry == "") {
                //  alert('Fill this field');
                $(".loginErrorCountrydlltxt").show();
                return false;
            }
            else {
                $(".loginErrorCountrydlltxt").hide();
            }
            var txtstatedl = $("#txtstatedl").val();
            if (txtstatedl == "") {
                //alert('Fill this field');
                $(".loginErrorstatedlltxt").show();
                return false;
            }
            else {
                $(".loginErrorstatedlltxt").hide();
            }
            var txtcitydl = $("#txtcitydl").val();
            if (txtcitydl == "") {
                //  alert('Fill this field');
                $(".loginErrorcitydlltxt").show();
                return false;
            }
            else {
                $(".loginErrorcitydlltxt").hide();
            }
        }
        debugger;
        var a = $("#selectCountries option:selected").text();
        if (a != "Others") {
            debugger;
            if (document.getElementById("selectCountries").selectedIndex != 0) {
                var selectState = document.getElementById("selectState").selectedIndex;
                if (selectState == 0) {
                    $('.State').show();
                    return false;
                }
                else {
                    $('.State').hide();
                }
                var txtcity = $("#selectcity").val();
                if (txtcity == "") {
                    //alert('Fill this field');
                    $(".City").show();
                    return false;
                }
                else {
                    $(".City").hide();
                }
            }
        }
        if (document.getElementById("selectState").selectedIndex != 0) {
            var selectState = document.getElementById("selectcity").selectedIndex;
            if (selectState == 0) {
                $('.city').show();
                return false;
            }
            else {
                $('.city').hide();
            }
        }
        var a = $("#selectCountries option:selected").text();

        if (self.errors().length > 0) {
            self.errors.showAllMessages(true);
            if (a == "-- Select Country --") {
                $('.ErrorCountryd').show();
                //   document.getElementById('ErrorCountryd').style.visibility = 'visible';
            }
            else {
                $('ErrorCountryd').hide();
                // document.getElementById('ErrorCountryd').style.visibility = 'none';
            }
            this.errors().forEach(function (data) {
            });
        }
        else {
            var data = new Object();
            data.Id = self.Id(),
            data.OrganizationId = self.OrganizationId(),
            data.BuildingName = self.BuildingName(),
            data.Address = self.Address(),
            data.ZipCode = self.ZipCode(),
             data.EmailId = self.EmailId(),
            data.ContactNumber = self.ContactNumber(),
            data.FaxNumber = self.FaxNumber(),
            data.WebSite = self.WebSite();
            data.CityId = self.NationalityId() == 11 ? null : self.CityId(),
            //alert(self.NationalityId());
            data.txtcountry = self.Countrydlltxt();
            data.txtstate = self.statedlltxt();
            data.txtcity = self.citydlltxt();
            //// display any error messages if we have them
            AjaxCall('/Api/Administration/SaveBuilding', data, 'POST', function (data) {
                debugger;
                if (data.Success == true) {
                    toastr.clear();
                    toastr.success(data.Message)
                    ApplyCustomBinding('buildings');
                    self.Countrydlltxt('')
                }
                else {
                    self.Countrydlltxt('');
                    self.statedlltxt('');
                    self.citydlltxt('');
                    self.CityId('');
                    toastr.error('Building name alreday exists!!')
                }
                self.IsInsert(true);
            })
        }
    }
    self.ResetBuilding = function () {
        self.GlobalSearch('');
        self.BuildingName('');
        self.Address('');
        self.ZipCode('');
        self.City('');
        self.EmailId('');
        self.ContactNumber('');
        self.FaxNumber('');
        self.WebSite('');
        ApplyCustomBinding('buildings');
        $('#Org').attr('disabled', false);

    }
    self.EditBuilding = function (tableItem) {
        if (tableItem != undefined) {
            $('.ErrorCountryd').hide();
            self.Id(tableItem.Id);
            self.EmailId(tableItem.EmailId);
            self.ContactNumber(tableItem.ContactNumber);
            self.FaxNumber(tableItem.FaxNumber);
            self.WebSite(tableItem.WebSite);
            self.BuildingName(tableItem.BuildingName);
            self.Address(tableItem.Address);
            self.ZipCode(tableItem.ZipCode);
            self.OrganizationId(tableItem.OrganizationId);
            self.NationalityId((tableItem.NationalityId == 'undefined' || null) ? 11 : tableItem.NationalityId);
            stateId = (tableItem.StateId);
            cityId = (tableItem.CityId);
            if (tableItem.txtcountry != null) {
                $("#Country").show();
                $("#dropcountry").show();
                self.Countrydlltxt(tableItem.txtcountry);
                console.log(tableItem.txtcountry);
                self.NationalityId(11);
                self.CityId(0);
                tableItem.CityId = 0;

            }
            else {
                $("#Country").show();
                $("#dropcountry").hide();
            }
            if (tableItem.txtstate != null) {
                $("#state").hide();
                $("#dropstate").show();
                self.statedlltxt(tableItem.txtstate);
                console.log(tableItem.txtstate);
            }
            else {
                $("#state").show();
                $("#dropstate").hide();
            }
            if (tableItem.txtcity != null) {
                $("#city").hide();
                $("#dropcity").show();
                self.citydlltxt(tableItem.txtcity);
                console.log(tableItem.txtcity);
            }
            else {
                $("#city").show();
                $("#dropcity").hide();
            }
            self.IsInsert(false);
            $("#btnSaveBuilding").text("Update");
            $('#Org').attr('disabled', true);

        }
    }
    self.DeleteBuilding = function (tableItem) {
        //var message = confirm("Are you sure, you want to delete selected record!");
        //if (message == true) {
        //    //debugger;
        //    AjaxCall('/Api/Administration/DeleteBuilding', tableItem, 'POST', function () {
        //        toastr.success('Building deleted successfully!!')
        //        ApplyCustomBinding('buildings');
        //    });
        //}
        recordToDelete = tableItem;
    }
    self.DeleteConfirmed = function () {
        $('#myModal').modal('hide');
        $('.modal-backdrop').modal('show');
        $('.modal-backdrop').modal('hide');
        AjaxCall('/Api/Administration/DeleteBuilding', recordToDelete, 'POST', function (data) {
            if (data.Success == true) {
                toastr.clear();
                toastr.success(data.Message);
                ApplyCustomBinding('buildings');
            }
            else if (data.Success == false) {
                toastr.clear();
                toastr.warning(data.Message);
            }
        });
    }
    self.GlobalSearchEnter = function (data) {
        //debugger;
        self.GetAllBuildingData();
        console.log(event);
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
    self.GetAllBuildingData();
}
