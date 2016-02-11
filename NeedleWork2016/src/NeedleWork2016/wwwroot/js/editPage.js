var selectedPalette = "";
var addNotEditColor = true;
var changedColor = "";

//Ajax !!!

function CreatePaletteElements(Id, Name) {
    $('#containerMiddle').append("<div class='PaletteName' id='" + Id + "'>" + Name + "</div>");
    $('#containerMiddle').append("<input type='button' name='EditPalette' value='Edit name' class='EditPalette'>");
    $('#containerMiddle').append("<input type='button' name='Remove' value='Remove' class='RemovePalette'>");
};

function CreateColorElements(Id, Name, Hex) {
    $('#colorList').append("<div class='color' id='" + Id + "'><div class='Thumbnail'></div><div class='colorName'>" + Name + "</div><div class='colorCode'>" + Hex + "</div></div>");
    $('#colorList div:last-child').children('.Thumbnail').css({ backgroundColor: Hex });
};


function myAjax(myType, myUrl, myData, mySuccessFunc, myErrorFunc) {
    $.ajax({
        type: myType,
        dataType: 'json',
        url: myUrl,
        data: myData,
        success: mySuccessFunc,
        error: myErrorFunc
    });
};




$(document).ready(function () {
    $('#choiceColor').css({ left: $('#choiceColor').parent().width() / 2 - $('#choiceColor').width() / 2 + "px" });




    // Show inputs for creating new Palette
    $('#AddPalette').click(function () {
        $('#hidePalette').slideUp(500);
    });

    // Clear input for Palette Name
    $('#cancel').click(function () {
        $('#containerLeft label[for=PaletteName]').text('');
        $('#inputPaletteName').val('');
    });

    // Back to Palette List
    $('#backToPalette').click(function () {
        if ($(window).width() > 1100) {
            $('#colorList').animate({ left: "-5%" }, 500)
        }
        else {
            $('#colorList').animate({ left: "-105%" }, 500)
        }
    });


    // Show inputs for creating new Color (AddColor - clicked)
    $('#AddColor').click(function () {

        window.addNotEditColor = true;
        
        if ($(window).width() > 1100) {
            $('#hideColor').animate({ left: "100%" }, 500);
            $('#choiceColor').animate({ top: "15%" }, 500);
        }
        else {
            $('#containerRight').animate({ left: "0%" }, 500);
            $('#hideColor').animate({ left: "100%" }, 0);
            $('#choiceColor').animate({ top: "10%" }, 0);
        }

    });


    // Show inputs for creating new Color (EditColor - clicked)

    $('body').on('click', '.EditColor', function (event) {
        //$('.ChangeColor').children('#EditColor').click(function (event) {
        event.stopPropagation();
        window.addNotEditColor = false;
        window.changedColor = $(this).parent().parent().prop('id');
        
        

        if ($(window).width() > 1100) {
            $('#hideColor').animate({ left: "100%" }, 500);
            $('#choiceColor').animate({ top: "15%" }, 500);
        }
        else {
            $('#containerRight').animate({ left: "0%" }, 500);
            $('#hideColor').animate({ left: "100%" }, 0);
            $('#choiceColor').animate({ top: "10%" }, 0);
           
        }
    });






    // Replace Edit/Remove block to selected div.color

    $(".ChangeColor").slideUp();
    $('body').on('click', '.color', function () {

        if ($(this).children(".ChangeColor").length == 0) {
            //$(this).append($(".ChangeColor"));
            $(this).append(
                "<div class='ChangeColor'>" +
                 "<input type='button' name='EditColor' value='Edit' class='EditColor'>" +
                  "<input type='button' name='RemoveColor' value='Remove' class='RemoveColor'></div>"
                );

            $(this).children(".ChangeColor").slideUp(0);

        }

        $('.color').not(this).children(".ChangeColor").slideUp(500);
        $(this).children(".ChangeColor").slideToggle(500);

    });


    // Hide all pop-ups and div.ChangeColor(Edit/Remove), when "Esc" was pressed 
    $(window).keydown(function (e) {
        if (e.which == 27) {
            $(".ChangeColor").slideUp();

            $('.popUpBack').click();
        }
    });



    // Console help
    $(window).click(function (e) {
        console.log(e.target);
    });




    // Get all Palettes (first page loading)

    function GetPalettes(data) {
        myAjax('get', '/Palettes/GetPalettes', data, Success, Error);

        function Success(data) {
            console.log("success123");
            for (var i = 0; i < data.Palettes.length; i++) {
                CreatePaletteElements(data.Palettes[i].Id, data.Palettes[i].Name);
            }

        }
        function error(data) {
            console.log(data);
        }
    };

    GetPalettes();



    // Create new Palette

    $('#sentPaletteName').on('click', function () {

        if (CheckName('#inputPaletteName')) {
            console.log("Yep yep");
            var data = {
                name: $('#inputPaletteName').val()
            };
            myAjax('post', '/Palettes/CreatePalette', data, Success, Error);
        }
        else {
            console.log('Oooops');
        }

        function Success(data) {
            console.log("New Palette was created !");
            CreatePaletteElements(data.Id, data.Name);
            console.log(data);
        }

        function Error(data) {
            console.log(data);
        }

        return false;

    });




    //Get Palette's colors && show effect for getting them

    $('body').on('click', '.PaletteName', function (e) {

        $(".color").remove();

        if ($(window).width() > 1100) {
            $('#colorList').animate({ left: "33.33%" }, 500)
            $('#colorList h2').text($(this).text());
        }
        else {
            $('#colorList').animate({ left: "0%" }, 500)
            $('#colorList h2').text($(this).text());
        };



        selectedPalette = this.id;
        var data = {
            idpalette: selectedPalette
        };
        function Success(data) {
            for (var i = 0; i < data.Colors.length; i++)
                CreateColorElements(data.Colors[i].Id, data.Colors[i].Name, data.Colors[i].Hex);
            console.log(data);
        }
        function Error(data) {
            console.log(data);
            alert("fail");
        }

        myAjax('get', '/Palettes/GetColors', data, Success, Error);

        return false;

    });

    //Remove Palette----------------------------------------

    $('body').on('click', '.RemovePalette', function (e) {

        var removedPalette = $(e.target).prev().prev().prop('id');
        var data = {
            id: removedPalette
        };
        
        function Success(data) {
           
            console.log(removedPalette);
            $(".PaletteName#" + removedPalette).nextAll("input:eq(0), input:eq(1)").remove();
            $(".PaletteName#" + removedPalette).remove();
            console.log("Yep yep, Removed!");
            console.log(data);
        }
        function Error(data) {
            console.log(data);
            alert("fail Remove");
        }

        myAjax('delete', '/Palettes/RemovePalette', data, Success, Error);

        return false;

    });

    //Remove Color----------------------------------------

    $('body').on('click', '.RemoveColor', function (event) {

        event.stopPropagation();
        var removedColor = $(event.target).parent().parent().prop('id');
        alert(removedColor);
        var data = {
            id: removedColor
        };
         
        function Success(data) {
                                  
            $('.color#' + removedColor).remove();
            console.log("Yep yep, delete!");
            console.log(data);
        }
        function Error(data) {
            console.log(data);
            alert("fail");
        }

        myAjax('delete', '/Palettes/RemoveColor', data, Success, Error);

    });


});