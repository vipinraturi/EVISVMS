function ChangePasswordViewModel() {
    //debugger;
    var self = this;

    self.Password = ko.observable('');//.extend({ required: true, minLength: 8 });
    self.NewPassword = ko.observable('');//.extend({ required: true, minLength: 8 });
    self.ConfirmNewPassword = ko.observable('');//.extend({ required: true, minLength: 8 });

    self.SaveChangePassword = function () {
        if (self.Password() == "") {
            toastr.clear();
            toastr.warning("Please enter the current password!");
            return false;
        }
        else if (self.Password().length < 8) {
            toastr.clear();
            toastr.warning("Current password is having less than 8 characters!");
            return false;
        }
        else if (self.NewPassword() == "") {
            toastr.clear();
            toastr.warning("Please enter the new password!");
            return false;
        }
        else if (self.NewPassword().length < 8) {
            toastr.clear();
            toastr.warning("New password is having less than 8 characters!");
            return false;
        }
        else if (self.ConfirmNewPassword() == "") {
            toastr.clear();
            toastr.warning("Please enter the confirm password!");
            return false;
        }
        else if (self.ConfirmNewPassword() != self.NewPassword()) {
            toastr.clear();
            toastr.warning("Confirm password is mismatched with new password!");
            return false;
        }

        var data = new Object();
        data.Password = self.Password(),
        data.NewPassword = self.NewPassword();
        AjaxCall('/Api/User/ChangePassword', data, 'POST', function (result) {
            if (result.Success) {
                toastr.clear();
                toastr.success(result.Message);
                ApplyCustomBinding('dashboard');
            }
            else if (result.Success == false) {
                toastr.clear();
                toastr.warning(result.Message);
            }
        })
    }

    self.ResetChangePassword = function () {
        self.Password('');
        self.NewPassword('');
        self.ConfirmNewPassword('');
        ApplyCustomBinding('changepassword');
    }

    self.PasswordChecker = function (data) {
        debugger;
        $("#txtPassword").val();
        var strongRegex = new RegExp("^(?=.{8,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");
        var mediumRegex = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");
        var enoughRegex = new RegExp("(?=.{7,}).*", "g");

        //if (false == enoughRegex.test($("#txtNewPassword").val())) {
        //    $("#spnNewPassword").css('color', 'red');
        //    $("#spnNewPassword").html('Please enter at least 8 characters!');
        //} else
        if (strongRegex.test($("#txtNewPassword").val())) {
            $("#spnNewPassword").css('color', 'green');
            $("#spnNewPassword").html('Strong!');
        } else if (mediumRegex.test($("#txtNewPassword").val())) {
            $("#spnNewPassword").css('color', 'orange');
            $("#spnNewPassword").html('Medium!');
        } else {
            $("#spnNewPassword").css('color', 'red');
            $("#spnNewPassword").html('Weak!');
        }
        return true;
    }
}