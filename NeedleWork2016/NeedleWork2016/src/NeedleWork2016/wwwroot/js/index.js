//---------------------------------------------------------
// Functions
//---------------------------------------------------------

//getting object with url params ('get')
function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}

//hide pop-up form
function PopUpHide(popUpObject, callback) {
    $(popUpObject).removeClass('openedPopUp');
    $(popUpObject).slideUp(function () {
        $(popUpObject).parent().css('display', 'none');
        if (typeof callback === 'function')
            callback();
    });
}

//show pop-up form
function PopUpShow(popUpObject) {
    popUpObject.addClass('openedPopUp');
    popUpObject.parent().css('display', 'block');
    //$('.popUpBack').css('display', 'block');
    popUpObject.slideDown();
}


//hide current pop-up form and show new 
function PopUpChange(oldPopUpObject, newPopUpObject) {
    PopUpHide(oldPopUpObject, function () {
        PopUpShow(newPopUpObject);
    });
}

//show specefic pop-up with information form (alert)
function PopUpShowInformation(title, message) {
    if ($(".openedPopUp").length == 0) {
        $("#informationTitle").html(title);
        $("#informationMessage").html(message);
        PopUpShow($("#popInformation"));
    } else {
        PopUpHide($(".openedPopUp"), function () {
            $("#informationTitle").html(title);
            $("#informationMessage").html(message);
            PopUpShow($("#popInformation"));
        });
    }
}

//show/hide menu in mobile mode
function MenuToggle() {
    $('header ul').slideToggle(500);
}

//toggle loading block
function ToggleLoading() {
    if ($('#loading').css('display') == 'table')
        $('#loading').css('display', 'none');
    else
        $('#loading').css('display', 'table');
}

//get message description by code
function GetMessageByCode(code, callback) {

    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '/Account/GetMessage/',
        data: { code: code },

        success: function (data) {
            console.log(data);
            if (typeof callback === 'function')
                callback(data.Name, data.Definition);
        },

        error: function (data) {
            console.log(data);
            if (typeof callback === 'function')
                callback(data.Name, data.Definition);
        }
    });
}


//--------------------------------------------------
// Functions for validations
//--------------------------------------------------

//email validation
function EmailIsValid(email) {
    //var regExp = /.+@.+\..+/;
    var regExp = /^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$/;
    return regExp.test(email);
}

function PasswordIsValid(password) {
    var regEx = /[a-z]{1,}/;
    if (password.length < 6)
        return false;
    if (!regEx.test(password))
        return false;
    regEx = /[A-Z]{1,}/;
    if (!regEx.test(password))
        return false;
    regEx = /[\W]{1,}/;
    if (!regEx.test(password))
        return false;
    return true;
}

function NameIsValid(name) {
    var regExp = /^[a-zA-Z]{2,20}$/;
    return regExp.test(name);
}

function PasswordIsConfirm(password1, password2) {
    return password1 === password2;
}


function ValidateRegistrationForm()
{
    var valid = true;

    if (!NameIsValid($('#registrationFirstName').prop('value'))) {
        $('#registrationFirstName').next().css('display', 'block');
        valid = false;
    }

    if (!NameIsValid($('#registrationLastName').prop('value'))) {
        $('#registrationLastName').next().css('display', 'block');
        valid = false;
    }

    if (!EmailIsValid($('#registrationEmail').prop('value'))) {
        $('#registrationEmail').next().css('display', 'block');
        valid = false;
    }

    if (!PasswordIsValid($('#registrationPassword').prop('value'))) {
        $('#registrationPassword').next().css('display', 'block');
        valid = false;
    }

    if (!PasswordIsConfirm($('#registrationPassword').prop('value'), $('#registrationConfirmPassword').prop('value'))) {
        $('#registrationConfirmPassword').next().css('display', 'block');
        valid = false;
    }
    
    return valid;
}

function ClearRegistrationForm()
{
    $('#registrationForm input[type^=text], #registrationForm input[type^=password]').prop('value', '');
}


$(document).ready(function () {


    //running as soon as page has loaded to get 'get' params and show pop-up

    var getParams = getUrlVars();
    if (getParams['successCode'] == '2007') {
        var code = getUrlVars()['code'];
        PopUpShow($('#popResetPasswordBack'));
    }

    if (getParams['successCode'] == '2003') {
        GetMessageByCode('2003', function (name, definition) {
            PopUpShowInformation(name, definition);
            setTimeout(function () {
                PopUpHide($(".openedPopUp"), function () {
                    window.location.href = '/';
                });
            }, 3000);
        });
    }





    //-----------------------------------------------------------
    // General event handlers
    //-----------------------------------------------------------

    //hide current opened pop-up
    $('.popUpBack').on('click', function (e) {
        if (e.target == this) {
            console.log(this);
            PopUpHide($(this).children(".popUp"));
        }
    });

    //hide current opened pop-up by click on 'esc' key
    $(window).keydown(function (e) {
        if (e.which == 27) {
            $('.popUpBack').click();
        }
    });

    //move the screen from 'welcome image' to content    //////must be removed to informationPage.js
    $('#welcome img').on('click', function () {
        $('body').animate({ scrollTop: $('#content').offset().top }, 1500);
    });


    //show/hide menu in mobile mode by click on '#showMenu' button
    $('#showMenu').on('click', function () {
        MenuToggle();
    });

    //change desktop/mobile mode of menu
    //ps: showing/hiding '#showMenu' button is controlled by media css style
    $(window).on('resize', function () {
        if ($('#showMenu').css('display') == 'none')
            $('header ul').css('display', 'block');
        else
            $('header ul').css('display', 'none');
    });




    //-----------------------------------------------------------
    // Event handlers for pop-up forms
    //-----------------------------------------------------------

    //show pop-up for login form
    $('#signIn').on('click', function () {
        PopUpShow($('#popUpLogin'));
    });

    //show pop-up for feedback form
    $('#feedback').on('click', function () {
        PopUpShow($('#popFeedback'));
    });

    //show pop-up for change password form
    $('#changePassword').on('click', function () {
        PopUpShow($('#popChangePassword'));
    });

    //show pop-up for forgot password form
    $('#forgotPassword').on('click', function () {
        PopUpChange($("#popUpLogin"), $("#popForgotPassword"));
    })

    //switch pop-up login form to pop-up registrtion form
    $('#registration').on('click', function () {
        $('#loginForm').slideUp(function () {
            $('#registrationForm').slideDown();
        })
    });

    //switch pop-up registrtion form to pop-up login form
    $('#login').on('click', function () {
        $('#registrationForm').slideUp(function () {
            $('#loginForm').slideDown();
        })
    });



    //-----------------------------------------------------------
    // Event handlers for send Ajax queries
    //-----------------------------------------------------------

    //send data to login user
    $('#loginSubmit').on('click', function () {

        ToggleLoading();

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
                ToggleLoading();
                console.log(data);
                location.reload();
            },

            error: function (data) {
                ToggleLoading();
                console.log(data);
                location.reload();
            }
        });
    });

    //send data to registratin new user
    $('#registrationSubmit').on('click', function () {

        ToggleLoading();
        $('.validationError').css('display','none');

        if (!ValidateRegistrationForm()){
            ToggleLoading();
            grecaptcha.reset();
            return false;
        }

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
                ToggleLoading();
                grecaptcha.reset();
                console.log(data);
                if (data.Code == '1001') {
                    $('#registrationReCaptcha').next().css('display', 'block');                   
                    return false;
                }
                ClearRegistrationForm();
                PopUpShowInformation(data.Name, data.Definition);
                //location.reload();
            },

            error: function (data) {
                ToggleLoading();
                grecaptcha.reset();
                console.log(data);
                if (data.Code == '1001') {
                    $('#registrationReCaptcha').next().css('display', 'block');                   
                    return false;
                }
                ClearRegistrationForm();
                PopUpShowInformation(data.Name, data.Definition);
                //location.reload();
            }
        });
    });

    //send data to log out current user
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


    //send feedback data ////////add information about successfully feedback
    $('#feedBackSubmit').on('click', function () {
        console.log($("#feedBackArea").val());

        ToggleLoading();

        var data = {
            text: $("#feedBackArea").val()
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Feedbacks/CreateFeedBack/',
            data: data,
            success: function () {
                ToggleLoading();
                text: $("#feedBackArea").val('');
                $('.popUpBack').click();
            },
            error: function (data) {
                ToggleLoading();
                alert("error");
            }
        });
    });


    //send email, then user will get the link on email to reset his password
    $('#forgotPasswordSubmit').on('click', function () {

        ToggleLoading();

        var data = {
            email: $('#forgotPasswordEmail').prop('value')
        };

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Account/ForgotPassword/',
            data: data,
            success: function (data) {
                ToggleLoading();
                console.log(data);
                PopUpShowInformation(data.Name, data.Definition);
            },
            error: function (data) {
                ToggleLoading();
                console.log(data);
                PopUpShowInformation(data.Name, data.Definition);
            }
        });
    });

    //send data to reset user password
    $('#resetPasswordSubmit').on('click', function () {

        ToggleLoading();

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
                ToggleLoading();
                console.log(data);
                PopUpShowInformation(data.Name, data.Definition);
            },

            error: function (data) {
                ToggleLoading();
                console.log(data);
                PopUpShowInformation(data.Name, data.Definition);
            }
        });
    });


    //send data to change password
    $('#changePasswordSubmit').on('click', function () {

        ToggleLoading();

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
                ToggleLoading();
                console.log(data);
                PopUpShowInformation(data.Name, data.Definition);
            },
            error: function (data) {
                ToggleLoading();
                console.log(data);
                PopUpShowInformation(data.Name, data.Definition);
            }
        });
    });


    //send data to change culture
    $(".langChange").on("click", function () {

        ToggleLoading();
        var data = { lang: $(this).prop("name") }

        $.ajax({
            type: 'post',
            dataType: 'json',
            url: "/Home/ChangeCulture",
            data: data,
            success: function (data) {
                ToggleLoading();
                console.log(data);
                location.reload();
            },

            error: function (data) {
                ToggleLoading();
                console.log(data);
            }
        });
    });
});