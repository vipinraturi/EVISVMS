function ChangePasswordViewModel() {
    //debugger;
    var self = this;

    self.Password = ko.observable('').extend({ required: true, minLength: 8 });
    self.NewPassword = ko.observable('').extend({ required: true, minLength: 8 });
    self.ConfirmNewPassword = ko.observable('').extend({ required: true, minLength: 8 });

    self.SaveChangePassword = function () {
        var data = new Object();
        data.Password = self.Password(),
        data.NewPassword = self.NewPassword();
        //debugger;
        AjaxCall('/Api/User/ChangePassword', data, 'POST', function (result) {
            //debugger;
            if (result.Success) {
                toastr.success(result.Message);
                ApplyCustomBinding('dashboard');
            }
            toastr.clear();
            toastr.warning(result.Message);
        })
    }

    self.ResetChangePassword = function () {
        self.Password('');
        self.NewPassword('');
        self.ConfirmNewPassword('');
        ApplyCustomBinding('changepassword');
    }
}