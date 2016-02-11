// Сheck Name
function CheckName(inputId) {
    var testColorName = /^[0-9a-zA-Z ]{1,30}$/;

    if (testColorName.test($(inputId).val())) {
        $(inputId).next().text("Success").css({ color: "green" });
        return $(inputId).val();
    }
    else {
        $(inputId).next().text("Incorrect name").css({ color: "red" });
        console.log("I see u");
    }

}


// Сheck RGB
function CheckRGB() {
    var testRGB = /^([0-9]{1,3}),([0-9]{1,3}),([0-9]{1,3})$/;
    var match = testRGB.exec($('#inputColorRGB').val());

    if (match !== null && match[1] <= 255 && match[2] <= 255 && match[3] <= 255) {
        $('#inputColorRGB').next().text('Success').css({ color: "green" });
        if ($('#inputColorHex').val() == "") {
            var hex = rgbToHex(match[1], match[2], match[3]);
            //$('#inputColorHex').val("#" + hex);
            return $('#inputColorHex').val("#" + hex);

        }
        return $('#inputColorRGB').val();

    }
    else if ($('#inputColorRGB').val() == "" && CheckHEX()) {
        return true;
    }
    else {
        $('#inputColorRGB').next().text("Incorrect number").css({ color: "red" });
    };
};


// Сheck HEX
function CheckHEX() {
    var testHEX = /^#[0-9a-fA-F]{0,6}$/;

    if (testHEX.test($('#inputColorHex').val()) && $('#inputColorHex').val().length == 7) {
        $('#inputColorHex').next().text("Success").css({ color: "green" });
        return $('#inputColorHex').val();
    }
    else if ($('#inputColorHex').val().length != 7) {
        $('#inputColorHex').next().text("Incorrect length").css({ color: "red" });
    }
    else {
        $('#inputColorHex').next().text("Incorrect char").css({ color: "red" });
    }
};



// Create new Color---------------------------------------------------

var createNewColor = (function () {
    var data = {
        idpalette: selectedPalette,
        hex: CheckHEX(),
        name: CheckName('#inputColorName')
    };

    function SuccessAdd(data) {
        CreateColorElements(data.AddedColor.Id, data.AddedColor.Name, data.AddedColor.Hex);

        if ($(window).width() > 1100) {
            $('#hideColor').animate({ left: "10%" }, 500);
            $('#choiceColor').animate({ top: "-100%" }, 500);
        }
        else {
            $('#containerRight').animate({ left: "105%" }, 500);
        }
    }

    function ErrorAdd(data) {
        console.log('Error of add new color');
    }

    myAjax('post', '/Palettes/CreateColor', data, SuccessAdd, ErrorAdd);
   
});



// Edit Color---------------------------------------------------

var editColor = (function () {   
    var data = {
        color: {
            id: window.changedColor,
            name: CheckName('#inputColorName'),
            hex: CheckHEX(),
            idpalette: selectedPalette
        }
    };

    function SuccessEdit(data) {
        console.log("Color was changed!!!");
        $('.color#' + changedColor).children('.Thumbnail').css({ backgroundColor: CheckHEX() });
        $('.color#' + changedColor).children('.colorName').text(CheckName('#inputColorName'));
        $('.color#' + changedColor).children('.colorCode').text(CheckHEX());

        if ($(window).width() > 1100) {
            $('#hideColor').animate({ left: "10%" }, 500);
            $('#choiceColor').animate({ top: "-100%" }, 500);
        }
        else {
            $('#containerRight').animate({ left: "105%" }, 500);
        }
    }

    function ErrorEdit(data) {
        console.log(data);
    }

    myAjax('post', '/Palettes/EditColor', data, SuccessEdit, ErrorEdit);
    $(".ChangeColor").slideUp();
});


$(document).ready(function () {

    $('#containerRight input[type=submit]').click(function AddColor(e) {

        if (CheckRGB() && CheckHEX() && CheckName('#inputColorName')) {
            console.log("All inputs are correct!");

            if (window.addNotEditColor)
                createNewColor();
            else
                editColor();
        }

        return false;

    });
});