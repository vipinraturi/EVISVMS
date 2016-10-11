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
    self.Address = ko.observable('').extend({ required: true });
    self.ZipCode = ko.observable('').extend({
        required: true,
        pattern: {
            message: "Invalid zip code.",
            params: /^([0-9\(\)\/\+ \-\.]*)$/
        },
        minLength: {
            params: 4,
            message: 'Enter minimum of 4-length number'
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
        },
        maxLength: {
            params: 12,
            message: 'Enter maximum of 12-length number'
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
    self.WebSite = ko.observable('');//.extend({ url: true });
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

    self.StateId = ko.observable('');
    self.CityId = ko.observable('');
    self.CountryId = ko.observable('');


    ko.validation.registerExtenders();
    self.Countrydlltxt = ko.observable();
    self.statedlltxt = ko.observable();
    self.citydlltxt = ko.observable();
    self.StateId = ko.observable();
    self.CityId = ko.observable();
    self.OrganizationName = ko.observable();
    self.CountryId = ko.observable(undefined);
    self.OrganizationId = ko.observable(undefined).extend({ required: true });
    self.GlobalSearch = ko.observable('');
    self.IsInsert = ko.observable(true);
    self.State = ko.observableArray();
    self.errors = ko.validation.group(
        {
            BuildingName: this.BuildingName,
            Address: this.Address,
            ZipCode: this.ZipCode,
            StateId: this.StateId,
            CityId: this.CityId,
            CountryId: this.CountryId,

            OrganizationId: this.OrganizationId,
            EmailId: this.EmailId,
            ContactNumber: this.ContactNumber,
            FaxNumber: this.FaxNumber,
            statedlltxt: this.statedlltxt,
            citydlltxt: this.citydlltxt
        });
    self.Organizations = ko.observableArray();
    AjaxCall('/Api/User/GetAllOrganizations', null, 'GET', function (data) {

        self.Organizations(data);
        if (data.length == 1) {
            self.OrganizationId(data[0].Id);
            $("#Org").attr('disabled', false);
        }
    });
    self.DataGrid = new RIT.eW.DataGridAjax('/Api/Administration/GetBuildingData', 7);
    self.GetAllBuildingData = function () {

        self.DataGrid.UpdateSearchParam('?globalSearch=' + self.GlobalSearch());
        self.DataGrid.GetData();
    }
    self.Countries = ko.observableArray();
    AjaxCall('/Api/Users/GetAllCountries', null, 'GET', function (data) {
        self.Countries(data);

    })
    self.City = ko.observableArray();

    self.ChangeState = function () {
        if (self.StateId() != undefined && self.StateId() != 0) {
            AjaxCall('/Api/Administration/GetAllCity?id=' + self.StateId(), null, 'GET', function (data) {

                if (data.length == 1 && data[0].Name == "Others") {
                    $("#dropcity").show();
                    self.City(data);
                    self.CityId(cityId);

                }
                else {
                    $('#dropcity').hide();
                    self.City(data);
                    self.CityId(cityId);
                }
            })
        }
    }
    self.LoadStates = function () {


        if (self.CountryId() != 11) {
            if (self.CountryId() != undefined && self.CountryId() != 0) {
                AjaxCall('/Api/Administration/GetAllStateOrCity?id=' + self.CountryId(), null, 'GET', function (data) {

                    if (data.length > 0) {
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



        else {

        }
    }
    self.InsertCity = function () {
        if (self.CityId() == 11) {
            $("#dropcity").show();
        }
        else {
            $("#dropcity").hide();
        }
    }
    self.SaveBuilding = function () {
        var abc = self.BuildingName();
        abc = self.WebSite();
        abc = self.Address();
        abc = self.ZipCode();
        abc = self.StateId();
        abc = self.CityId();
        abc = self.CountryId();


        abc = self.OrganizationId();
        abc = self.EmailId;
        abc = self.Countrydlltxt;
        abc = self.statedlltxt;
        abc = self.citydlltxt;

        var a = $("#selectCountries option:selected").text();

        if (a == "Others") {
            var txtcountry = $("#txtcountry").val();
            if (txtcountry == "") {
                $(".loginErrorCountrydlltxt").show();
                return false;
            }
            else {
                $(".loginErrorCountrydlltxt").hide();
            }
            var txtstatedl = $("#txtstatedl").val();
            if (txtstatedl == "") {
                $(".loginErrorstatedlltxt").show();
                return false;
            }
            else {
                $(".loginErrorstatedlltxt").hide();
            }
            var txtcitydl = $("#txtcitydl").val();
            if (txtcitydl == "") {
                $(".loginErrorcitydlltxt").show();
                return false;
            }
            else {
                $(".loginErrorcitydlltxt").hide();
            }
        }

        var a = $("#selectCountries option:selected").text();
        if (a != "Others") {

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
        if ($.trim(self.WebSite()) != '' && $.trim(self.WebSite()).match(/^(www\.)[a-zA-Z0-9./]+$/gim) == null) {
            $("#spnWebsite").html("Invalid webiste url").show();
        }
        else {
            $("#spnWebsite").html("").hide();
        }
        if (self.errors().length > 0 || ($("#txtWebsite").val() != '' && $("span.validationMessage").text() != '')) {
            self.errors.showAllMessages(true);
            if (a == "-- Select Country --") {
                $('.ErrorCountryd').show();
            }
            else {
                $('ErrorCountryd').hide();
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
            data.CityId = self.CountryId == 11 ? null : self.CityId(),
            data.CountryId = self.CountryId(),

            data.StateId = self.StateId(),
            data.txtcountry = self.Countrydlltxt();
            data.txtstate = self.statedlltxt();
            data.txtcity = self.citydlltxt();

            //// display any error messages if we have them
            AjaxCall('/Api/Administration/SaveBuilding', data, 'POST', function (data) {

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
                    //  self.CityId('');
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

            self.CountryId(tableItem.CountryId);
            stateId = (tableItem.StateId);
            cityId = (tableItem.CityId);

            if (tableItem.txtcity != null && tableItem.CityId == 11) {

                $("#city").show();
                $("#dropcity").show();
                self.citydlltxt(tableItem.txtcity);

            }
            else {
                $("#city").show();
                $("#dropcity").hide();
            }
            self.IsInsert(false);
            $("#btnSaveBuilding").text("Update");
            $('#Org').attr('disabled', true);


            //state loadcode
            if (self.CountryId() != 11) {
                if (self.CountryId() != undefined && self.CountryId() != 0) {
                    AjaxCall('/Api/Administration/GetAllStateOrCity?id=' + self.CountryId(), null, 'GET', function (data) {

                        if (data.length > 0) {

                            self.State(data);
                            self.StateId(stateId);
                            $('#dropcountry').hide();
                            $('#dropcity').hide();
                            $('#dropstate').hide();
                            $('#state').show();
                            $('#city').show();
                            $(".ErrorCountryd").hide();

                            //city load code
                            if (self.StateId() != undefined && self.StateId() != 0) {
                                AjaxCall('/Api/Administration/GetAllCity?id=' + self.StateId(), null, 'GET', function (data) {
                                    if (data.length == 1 && data[0].Name == "Others") {
                                        $("#dropcity").show();
                                        self.City(data);
                                        self.CityId(cityId);
                                    }
                                    else {
                                        $('#dropcity').hide();
                                        self.City(data);
                                        self.CityId(cityId);
                                    }
                                    self.InsertCity();
                                })
                            }

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
    }
    self.DeleteBuilding = function (tableItem) {
        //var message = confirm("Are you sure, you want to delete selected record!");
        //if (message == true) {
        //    
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

        self.GetAllBuildingData();
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
    self.GetAllBuildingData();
}
