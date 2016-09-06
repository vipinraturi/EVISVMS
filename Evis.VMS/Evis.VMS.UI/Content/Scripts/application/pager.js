var RIT = window.RIT || {};
RIT.eW = RIT.eW || {};
var gblParam = [];
RIT.eW.Services = RIT.eW.Services || {};
RIT.eW.Services.AjaxPostCall = function (fullUrl, dataObj, callbackFunction, search) {
    
    gblParam = dataObj;

    var urlToPass = '';
    if (search != "") {
        fullUrl = fullUrl + search;
    }
    if (fullUrl.indexOf('?') == -1) {
        fullUrl = fullUrl + '?';
    }
    else {
        fullUrl = fullUrl + '&';
    }
    urlToPass = fullUrl + 'pageIndex=' + gblParam.pageIndex() + '&pageSize=' + gblParam.pageSize() + '&sortField=' + gblParam.sortField() + '&sortOrder=' + gblParam.sortOrder();

    $.ajax({
        url: urlToPass,
        cache: false,
        type: 'post',
        //data: JSON.stringify(dataObj),
        success: function (data)
        {
            ////debugger;
            callbackFunction(RIT.eW.Utils.GetJson(data));
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("error :" + XMLHttpRequest.responseText);
            alert('There was an error in performing this operation.');
        }
    });
};
RIT.eW.Services.AjaxJsonPostCall = function (fullUrl, dataObj, callbackFunction) {
    $.ajax({
        type: 'post',
        url: fullUrl,
        data: JSON.stringify(dataObj),
        dataType: 'json',
        cache: false,
        success: function (data) { callbackFunction(RIT.eW.Utils.GetJson(data)); },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("error :" + XMLHttpRequest.responseText);
            alert('There was an error in performing this operation.');
        }
    });
};
RIT.eW.DataGridAjax = (function () {
    var getDataUrl = '';
    function DataGridAjax(url, pageSize) {
        var self = this;
        //////debugger;
        getDataUrl = url;
        self.GridParams = {
            pageIndex: ko.observable(1),
            pageSize: ko.observable(pageSize),
            sortField: ko.observable(''),
            sortOrder: ko.observable('ASC'),
            totalRows: ko.observable(0),
            totalPages: ko.observable(0),
            requestedPage: ko.observable(0),
            pageSizeOptions: [5, 7, 10, 20, 30, 50, 100]
        };
        self.DataRows = ko.observableArray();
        self.search = ko.observable('');
        self.SelectedPageSizeOption = ko.observable(pageSize);
        self.GridParams.requestedPage.subscribe(self.FlipPageDirect, self);
        self.SelectedPageSizeOption.subscribe(self.ChangePageSize, self);
    }

    DataGridAjax.prototype.UpdateSearchParam = function (searchParam) {
        var self = this;
        self.search(searchParam);
    }

    DataGridAjax.prototype.GetData = function () {
        var self = this;
        ////debugger;
        RIT.eW.Services.AjaxPostCall(getDataUrl, self.GridParams, $.proxy(self.OnGetDataDone, this), self.search());
    };

    DataGridAjax.prototype.OnGetDataDone = function (data) {
        var self = this;
        if (data != null) {
            self.DataRows(RIT.eW.Utils.GetJson(data.result));
            self.GridParams.totalRows(RIT.eW.Utils.GetJson(data.totalRows));
            var totalPages = Math.ceil(self.GridParams.totalRows() / self.GridParams.pageSize());
            self.GridParams.totalPages(totalPages);
            self.GridParams.requestedPage(self.GridParams.pageIndex());
        }
    };

    DataGridAjax.prototype.FlipPage = function (newPageNo) {
        var self = this;
        //////debugger;
        if (parseInt(newPageNo) > 0 && parseInt(newPageNo) <= self.GridParams.totalPages()) {
            self.GridParams.pageIndex(newPageNo);
            self.GetData();
        }
    };

    DataGridAjax.prototype.FlipPageDirect = function (newValue) {
        var self = this;
        var ri = parseInt(self.GridParams.requestedPage());
        if ( ri == NaN) {
            self.GridParams.requestedPage(self.GridParams.pageIndex());
            return;
        }
        if (ri > 0 && ri <= self.GridParams.totalPages()) {
            self.GridParams.pageIndex(ri);
            self.GetData();
            return;
        }
        self.GridParams.requestedPage(self.GridParams.pageIndex());
        return;
    };

    DataGridAjax.prototype.ChangePageSize = function () {
        var self = this;
        if (self.GridParams.pageSize() != self.SelectedPageSizeOption()) {
            self.GridParams.pageSize(self.SelectedPageSizeOption());
            self.GridParams.pageIndex(1);
            self.GridParams.requestedPage(1);
            self.GetData();
        }
    };

    DataGridAjax.prototype.Sort = function (col) {
        var self = this;
        //console.log(col);
        $('.icons').remove();
        if (self.GridParams.sortField() === col) {
            if (self.GridParams.sortOrder() === 'ASC') {
                self.GridParams.sortOrder('DESC');
                
                $('.' + col).after('<i class="fa fa-chevron-down icons"></i>');
            } else {
                self.GridParams.sortOrder('ASC');
                $('.' + col).after('<i class="fa fa-chevron-up icons"></i>');
            }
        } else {
            self.GridParams.sortOrder('ASC');
            $('.' + col).after('<i class="fa fa-chevron-up icons"></i>');
            self.GridParams.sortField(col);
        }
        self.GetData();
    };
    return DataGridAjax;
})();
RIT.eW.DataGridBasic = (function () {
    var getDataUrl = '';
    var allDataRows = new Array();
    function DataGridBasic(url, pageSize) {
        var self = this;
        getDataUrl = url;
        self.GridParams = {
            pageIndex: ko.observable(1),
            pageSize: ko.observable(pageSize),
            sortField: ko.observable(''),
            sortOrder: ko.observable('ASC'),
            totalRows: ko.observable(0),
            totalPages: ko.observable(0),
            requestedPage: ko.observable(0),
            pageSizeOptions: [5, 7, 10, 20, 30, 50, 100]
        };
        self.DataRows = ko.observableArray();
        self.SelectedPageSizeOption = ko.observable(pageSize);
        self.GridParams.requestedPage.subscribe(self.FlipPageDirect, self);
        self.SelectedPageSizeOption.subscribe(self.ChangePageSize, self);
    }
    DataGridBasic.prototype.GetData = function () {
        var self = this;
        RIT.eW.Services.AjaxPostCall(getDataUrl, '', $.proxy(self.OnGetDataDone, this),'');
    };
    DataGridBasic.prototype.OnGetDataDone = function (data) {
        var self = this;
        allDataRows = RIT.eW.Utils.GetJson(data.result);
        self.GridParams.totalRows(RIT.eW.Utils.GetJson(data.totalRows));
        self.UpdateData();
    };
    DataGridBasic.prototype.UpdateData = function () {
        var self = this;
        //////debugger;

        self.DataRows(self.GetPagedData());
        var totalPages = Math.ceil(self.GridParams.totalRows() / self.GridParams.pageSize());
        self.GridParams.totalPages(totalPages);
        //////debugger;
        self.GridParams.requestedPage(self.GridParams.pageIndex());
    };
    DataGridBasic.prototype.FlipPage = function (newPageNo) {
        var self = this;
        if (parseInt(newPageNo) > 0 && parseInt(newPageNo) <= self.GridParams.totalPages()) {
            self.GridParams.pageIndex(newPageNo);
            self.UpdateData();
        }
    };
    DataGridBasic.prototype.FlipPageDirect = function (newValue) {
        var self = this;
        var ri = parseInt(self.GridParams.requestedPage());
        if (ri == NaN) {
            self.GridParams.requestedPage(self.GridParams.pageIndex());
            return;
        }
        if (ri > 0 && ri <= self.GridParams.totalPages()) {
            self.GridParams.pageIndex(ri);
            self.UpdateData();
            return;
        }
        self.GridParams.requestedPage(self.GridParams.pageIndex());
        return;
    };
    DataGridBasic.prototype.ChangePageSize = function () {
        var self = this;
        if (self.GridParams.pageSize() != self.SelectedPageSizeOption()) {
            self.GridParams.pageSize(self.SelectedPageSizeOption());
            self.GridParams.pageIndex(1);
            self.GridParams.requestedPage(1);
            self.UpdateData();
        }
    };
    DataGridBasic.prototype.Sort = function (col) {
        var self = this;
        if (self.GridParams.sortField() === col) {
            if (self.GridParams.sortOrder() === 'ASC') {
                self.GridParams.sortOrder('DESC');
            } else {
                self.GridParams.sortOrder('ASC');
            }
        } else {
            self.GridParams.sortOrder('ASC');
            self.GridParams.sortField(col);
        }
        allDataRows.sort(self.dynamicSort(self.GridParams.sortField(), self.GridParams.sortOrder()));
        self.UpdateData();
    };
    DataGridBasic.prototype.GetPagedData = function() {
        var self = this;
        var size = self.GridParams.pageSize();
        var start = (self.GridParams.pageIndex()-1)*size;
        return allDataRows.slice(start, start + size);
    };
    DataGridBasic.prototype.dynamicSort = function (sortProperty, direction) {
        //////debugger;
        var thisMethod = function(a, b) {
            var valueA = a[sortProperty];
            var valueB = b[sortProperty];
            if (typeof valueA != "number" && typeof valueA != "object") {
                valueA = a[sortProperty].toLowerCase();
                valueB = b[sortProperty].toLowerCase();
            }
            if (direction.toLowerCase() == "asc") {
                if (valueA < valueB) {
                    return -1;
                }
                if (valueA > valueB) {
                    return 1;
                }
            } else {
                if (valueA > valueB) {
                    return -1;
                }
                if (valueA < valueB) {
                    return 1;
                }
            }
            return 0;
        };
        return thisMethod;
    };

    return DataGridBasic;
})();
RIT.eW.Utils = RIT.eW.Utils || {};
RIT.eW.Utils.GetJson = function (data) {
    ////debugger;
    if (data == '' || data == 'undefined')
        return null;
    //debugger;
    return (JSON && JSON.parse(data) || $.parseJSON(data));
};