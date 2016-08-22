function DashboardViewModel() {
    debugger;
    var self = this;
    self.GlobalSearch = ko.observable('');

    self.DataGrid = new RIT.eW.DataGridAjax('/Api/DashBoard/DisplayAllShift', 7);
    //self.DataGrid.GetData();
    self.GetAllGateData = function () {
        self.DataGrid.UpdateSearchParam('?globalSearch=' + JSON.stringify(new Object()));
        self.DataGrid.GetData();
    }

    self.GetAllGateData();

    //AjaxCall('/Api/DashBoard/DisplayAllShift', data, 'GET', function (data) {

    //})
    //changetheme('theme7');
}