var dataToSend = '';


ApplyCustomBinding = function (elementName) {
    $('.loader-div').show();
    switch (elementName) {

        //Administration module  
        case 'organization':
            BindingViewModel("/Administration/_Organization", OrganizationViewModel());
            break;
        case 'myorganization':
            BindingViewModel("/Administration/_MyOrganization", MyOrganizationViewModel());
            break;
        case 'buildings':
            BindingViewModel("/Administration/_Building", BuildingViewModel());
            break;
        case 'gates':
            BindingViewModel("/Administration/_Gates", GatesViewModel());
            break;
        case 'newuser':
            BindingViewModel("/Administration/_Users", UsersViewModel());
            break;
        case 'newshiftcreate':
            BindingViewModel("/Administration/_Shifts", ShiftViewModel());
            break;
        case 'shiftassignment':
            BindingViewModel("/Administration/_ShiftAssignment", ShiftAssignmentViewModel());
            break;
        case 'myprofile':
            BindingViewModel("/Administration/_Myprofile", MyProfileViewModel());
            break;
        case 'changepassword':
            BindingViewModel("/Administration/_ChangePassword", ChangePasswordViewModel());
            break;
        case 'themeSelection':
            BindingViewModel("/Administration/_ThemeSelection", ThemeSelectionViewModel());
            break;
            //Visitor module  
        case 'scanvisitor':
            BindingViewModel("/Visitor/_ScanVisitor", ScanVisitorViewModel());
            break;
        case 'managevisitor':
            var scanVisitorViewModel = ScanVisitorViewModel();
            BindingViewModel("/Visitor/_ManageVisitorManually", VisitorViewModel(scanVisitorViewModel.split('_')[0], scanVisitorViewModel.split('_')[1], scanVisitorViewModel.split('_')[2], scanVisitorViewModel.split('_')[3], scanVisitorViewModel.split('_')[4], scanVisitorViewModel.split('_')[5]));
            break;
        case 'visitorcheckin':
            BindingViewModel("/Visitor/_VisitorCheckInCheckout", VisitorCheckInCheckOutViewModel());
            break;
            //Report module  
        case 'visitordetailsreport':
            BindingViewModel("/Report/_VisitorDetailsReport", VisitorDetailsViewModel());
            break;
        case 'shiftdetailsreport':
            BindingViewModel("/Report/_ShiftDetailsReport", ShiftDetailsViewModel());
            break;

            //Dashboard
        case 'dashboard':
            BindingViewModel("/Dashboard/_Dashboard", DashboardViewModel());
            break;

        default:
    }
}

BindingViewModel = function (controllerUrl, viewModel) {
    // debugger;
    $('#content').load(controllerUrl, function () {
        ko.cleanNode($('#content')[0]);
        ko.applyBindings(viewModel, $('#content')[0]);
        $('.loader-div').hide();
        DashboardBindEvent();

        if (controllerUrl == "/Visitor/_ManageVisitorManually") {
            $('#dateDOB').datepicker();
        }
        if (controllerUrl == "/Visitor/_VisitorCheckInCheckout") {
            BindAutoCompleteEvent();
        }
        if (controllerUrl == "/Administration/_ShiftAssignment") {
            $('#dateFrom').datepicker();
        }
        if (controllerUrl == "/Administration/_ShiftAssignment") {
            $('#dateTo').datepicker();
        }
        if (controllerUrl == "/Administration/_Shifts") {
            $('#txtToTime').timepicker();
        }
    });
}

DashboardBindEvent = function () {
    $('#lnkVisitorCheckIn').unbind('click').bind('click', function () {
        ApplyCustomBinding('visitorcheckin');
    });

    $('#lnkTotalVisitor').unbind('click').bind('click', function () {
        ApplyCustomBinding('managevisitor');
    });

    $('#lnkBuilding').click(function () {
        ApplyCustomBinding('buildings');
    });

    $('#lnkGates').click(function () {
        ApplyCustomBinding('gates');

    });

    //$('#btnCompleteRegister').click(function () {
    //    ApplyCustomBinding('managevisitor');
    //});

}

