$(document).ready(function () {


    //var url = window.location.href.split('/');
    //if (url[url.length - 1] == "emailconfirmed") {
    //    alert("your email was confirmed, thanks");
    //    window.location.href = "/"
    //}

    if (getUrlVars()['parametr'] == 'emailconfirmed') {
        alert("your email was confirmed, thanks");
    } else if (getUrlVars()['parametr'] == 'resetPassword') {
        var code = getUrlVars()['code'];
        PopUpShow($('#popResetPasswordBack'));
    }


    function getUrlVars() {
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }


    function PopUpHide(popUpObject, callback) {
        $(popUpObject).children().slideUp(function () {
            $(popUpObject).css('display', 'none');
            callback();
        });
    }

    function PopUpShow(popUpObject) {
        popUpObject.css('display', 'block');
        popUpObject.children().slideDown();
    }

    function PopUpChange(newPopUpObject) {
        PopUpHide($(".popUpBack"), function () {
            PopUpShow(newPopUpObject);
        });
    }

    function PopUpShowInformation(title, message) {
        PopUpHide($(".popUpBack"), function () {
            $("#informationTitle").html(title);
            $("#informationMessage").html(message);
            PopUpShow($("#popInformationBack"));
        });
    }

    function MenuToggle() {
        $('header ul').slideToggle(500);
    }

    $('.popUpBack').on('click', function (e) {
        if (e.target == this) {
            console.log(this);
            PopUpHide(this);
        }
    });

    $('#welcome img').on('click', function () {
        $('body').animate({ scrollTop: $('#content').offset().top }, 1500);
    });

    $('#showMenu').on('click', function () {
        MenuToggle();
    });

    $(window).on('resize', function () {
        if ($('#showMenu').css('display') == 'none')
            $('header ul').css('display', 'block');
        else
            $('header ul').css('display', 'none');
    });

    $('#signIn').on('click', function () {
        PopUpShow($('#popUpLoginBack'));
    });

    $('#feedback').on('click', function () {
        PopUpShow($('#popFeedbackBack'));
    });

    $('#changePassword').on('click', function () {
        PopUpShow($('#popChangePasswordBack'));
    });

    $('#forgotPassword').on('click', function () {
        PopUpChange($("#popForgotPasswordBack"));
    })


    $('#registration').on('click', function () {
        $('#loginForm').slideUp(function () {
            $('#registrationForm').slideDown();
        })
    });

    $('#login').on('click', function () {
        $('#registrationForm').slideUp(function () {
            $('#loginForm').slideDown();
        })
    });


    $(window).keydown(function (e) {
        if (e.which == 27) {
            $('.popUpBack').click();
        }
    });

    $('#loginSubmit').on('click', function () {

        var data = {
            email: $('#loginEmail').prop('value'),
            password: $('#loginPassword').prop('value'),
            remember: $('#loginRemember').prop('checked')
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Account/Login/',
            data: data,

            success: function (data) {
                alert("success");
                console.log(data);
                location.reload();
            },

            error: function (data) {
                console.log(data);
                //location.reload();
            }
        });
    });

    $('#resetPasswordSubmit').on('click', function () {

        var data = {
            email: $('#resetPasswordEmail').prop('value'),
            password: $('#resetPasswordNewPassword').prop('value'),
            code: code
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Account/ResetPassword/',
            data: data,

            success: function (data) {
                console.log(data);
            },

            error: function (data) {
                console.log(data);
            }
        });
    });


    $('#registrationSubmit').on('click', function () {

        var data = {
            firstName: $('#registrationFirstName').prop('value'),
            lastName: $('#registrationLastName').prop('value'),
            email: $('#registrationEmail').prop('value'),
            password: $('#registrationPassword').prop('value'),
            captchaResponse: grecaptcha.getResponse()
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Account/Register/',
            data: data,

            success: function (data) {
                //alert("success");
                console.log(data);
                PopUpShowInformation("Registration message", "Registration was success. Please, check your email");
                //location.reload();
            },

            error: function (data) {
                console.log(data);
                $('#registrationReCaptcha').append('<p>Please verify that you are not a robot</p>');
                //location.reload();
            }
        });
    });


    $('#signOut').on('click', function () {

        var data = {
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Account/LogOff/',
            data: data,

            error: function (data) {
                location.reload();
            }
        });
    });

    $('#feedBackSubmit').on('click', function () {
        console.log($("#feedBackArea").val());

        var data = {
            text: $("#feedBackArea").val()
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Feedbacks/CreateFeedBack/',
            data: data,
            success: function () {
                text: $("#feedBackArea").val('');
                $('.popUpBack').click();
            },
            error: function (data) {
                alert("error");
            }
        });
    });

    $('#changePasswordSubmit').on('click', function () {

        var data = {
            oldPassword: $("#changePasswordOldPassword").prop("value"),
            newPassword: $("#changePasswordNewPassword").prop("value")
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Account/ChangePassword/',
            data: data,
            success: function (data) {
                console.log(data);
            },
            error: function (data) {
                console.log(data);
            }
        });
    });


    $('#forgotPasswordSubmit').on('click', function () {

        var data = {
            email: $('#forgotPasswordEmail').prop('value')
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Account/ForgotPassword/',
            data: data,
            success: function (data) {
                console.log(data);
            },
            error: function (data) {
                console.log(data);
            }
        });
    });

    $(".langChange").on("click", function () {
        var data = { lang: $(this).prop("name") }

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: "/Home/ChangeCulture",
            data: data,
            success: function (data) {
                console.log(data);
                location.reload();
            },

            error: function (data) {
                console.log(data);
            }
        });

    })

});