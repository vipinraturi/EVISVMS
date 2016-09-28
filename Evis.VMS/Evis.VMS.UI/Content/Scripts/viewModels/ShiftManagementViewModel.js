
function ShiftManagementViewModel() {
    var self = this;

    self.ChangeShift = function () {
        if (gblTableId != '' && gblTableId != undefined) {
            if ($('#' + gblTableId).html().indexOf('fa-check-unique') != -1) {
                $('#' + gblTableId).html('');
            }
            else {
                $('#' + gblTableId).html(' <i class="fa fa-check fa-check-unique" aria-hidden="true"></i>')
            }

            $('#myModal').modal('hide');
            toastr.success('Shift changed successfully!!');
        }
       
    }
}