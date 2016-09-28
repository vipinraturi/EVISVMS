$(function () {
    //
    ApplyCustomBinding('dashboard');

    $('.side-menu li').click(function () {
       
        

        var $lis = $(this).parents('.child_menu').find('li');
        if ($lis.length > 0) {
            $lis.removeClass('active');//removing old active class
            $(this).addClass('active');//adding new active class on current element
        }
        

        AttachClickEvent($(this).attr('id'));
    });

    $('.myprofile').click(function () {
        AttachClickEvent($(this).attr('class'));
    });

    $('.changepassword').click(function () {
        AttachClickEvent($(this).attr('class'));
    });

    $('.myorganization').click(function () {
        AttachClickEvent($(this).attr('class'));
    });

    $('.themeSelection').click(function () {
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

