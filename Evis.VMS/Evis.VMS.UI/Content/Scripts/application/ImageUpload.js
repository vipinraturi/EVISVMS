LoadImage = function () {
    AjaxCall('/Api/MyProfile/GetMyProfile', null, 'GET', function (data) {
        debugger;
        var theme = data.Organization.ImagePath;
        $("#mainLogo").attr('src', theme);
        //changetheme(theme);
    });
}