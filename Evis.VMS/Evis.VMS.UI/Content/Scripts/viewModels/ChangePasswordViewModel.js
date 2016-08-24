function ChangePasswordViewModel() {
    //debugger;
    var self = this;

    self.Password = ko.observable('').extend({ required: true, minLength: 8 });
    self.NewPassword = ko.observable('').extend({ required: true, minLength: 8 });
    self.ConfirmNewPassword = ko.observable('').extend({ required: true, minLength: 8 });

    self.SaveChangePassword = function () {
        if (self.Password() == "") {
            toastr.clear();
            toastr.warning("Please enter the current password!");
            return false;
        }
        else if (self.NewPassword() == "") {
            toastr.clear();
            toastr.warning("Please enter the new password!");
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
            else if(result.Success == false){
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
}