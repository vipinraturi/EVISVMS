$(function () {
    //debugger;
    ApplyCustomBinding('dashboard');

    $('.side-menu li').click(function () {
       // $('.side-menu li').removeClass('active');
        AttachClickEvent($(this).attr('id'));
    });

    $('.myprofile').click(function () {
        AttachClickEvent($(this).attr('class'));
    });

    $('.myorganization').click(function () {
        AttachClickEvent($(this).attr('class'));
    });

    AttachClickEvent = function (currentItem) {
        if (currentItem != undefined && currentItem != "") {
            ApplyCustomBinding(currentItem);
        }
    }

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": false,
        "positionClass": "toast-custom",
        "onclick": null,
        "showDuration": "3000",
        "hideDuration": "1000",
        "timeOut": "10000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
});

