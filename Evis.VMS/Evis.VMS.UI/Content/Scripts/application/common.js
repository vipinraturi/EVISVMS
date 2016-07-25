
function AjaxCall(apiUrl, formData, ajaxType, callback) {

    ////debugger;
    $.ajax({
        type: ajaxType,
        url: apiUrl,
        data: JSON.stringify(formData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            //debugger;
            callback(data);
        }
    });
}



