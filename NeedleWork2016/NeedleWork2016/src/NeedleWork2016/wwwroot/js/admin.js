
function ReloadJQGrid() {
    $('#usersList').jqGrid('clearGridData');
    $.ajax({
        url: '/AdminUser/GetUserData/',
        datatype: "json",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        mathod: 'GET',
        async: true,
        success: function (result) {

            jQuery("#usersList")
            .jqGrid('setGridParam',
                {
                    datatype: 'local',
                    data: JSON.parse(result)
                })
        .trigger("reloadGrid");
        }
    });
}

function DeleteCustomer(rowId) {
    $.ajax({
        url: '/AdminUser/DeleteUserData/' + rowId,
        data: {
            id: rowId
        },
        contentType: 'application/json; charset=utf-8',
        type: 'GET',
        success: function (result) {
            $('.deleteSuccess').dialog("open");
            $('.deleteSuccess').fadeOut(2000);
            setTimeout(function () { $('.deleteSuccess').dialog("close") }, 2000);
            ReloadJQGrid();
        }
    });
}

(function ($) {

    $(document).ready(function () {

        LoadTableCustomers();
        addRow();
        minusRow();
        getLocalizationData();
        updateLocalizationData();
        selectLang();
        $('.deleteSuccess').dialog({
            autoOpen: false
        });
    });

    function selectLang() {
        $("#selectLanguage").change(function () {

            var lang = $(this).val();

            $.ajax({
                url: 'Localization/GetLocalizationData/',
                method: "POST",
                data: { lang: lang },
                success: function () {
                    getLocalizationData();
                    alert(lang);
                }
            })
        });
    }

    function getLocalizationData() {
        console.log($("#selectLanguage").val());
        var defaultLang = $("#selectLanguage").val();

        $.ajax({
            url: 'Localization/GetLocalizationData/',
            datatype: "json",
            data: { lang:defaultLang },
            method: "POST",
            success: function (result) {
                result = JSON.parse(result);
            }
        })
    }

    function updateLocalizationData() {

        var getFormJson = $("#localizationForm").serializeArray(),
            firstTextList = [],
            secondTextList = [],
            thirdTextList = [];

        $("#updateBtn").click(function (event) {
            
            event.preventDefault();

            $(".multiple input.FirstBlockText").each(function () {
                firstTextList.push($(this).val());
            });

            $(".multiple input.SecondBlockText").each(function () {
                secondTextList.push($(this).val());
            });

            $(".multiple input.ThirdBlockText").each(function () {
                thirdTextList.push($(this).val());
            });

           var Object = {
                "Title": $('#title').val(),
                "FirstBlockName": $('#FirstBlockName').val(),
                "FirstBlockText": firstTextList,
                "SecondBlockName": $('#SecondBlockName').val(),
                "SecondBlockText": secondTextList,
                "ThirdBlockName": $('#ThirdBlockName').val(),
                "ThirdBlockText": thirdTextList
            };
            
            console.log(Object);
             
            $.ajax({
                url: 'Localization/Update',
                type: 'POST',
                data: { name: 'Home', json: JSON.stringify(Object) },
                success: function (result) {
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        });
    }

    function addRow() {

        var FirstBlock = "second-level FirstBlock",
            SecondBlock = "second-level SecondBlock",
            ThirdBlock = "second-level ThirdBlock",
            appendElement = '',
            setClass = "";

        $('i.fa.fa-plus-circle').click(function () {

            getCurrentBlock = $(this).parents().eq(2).attr("class");

            if (getCurrentBlock == FirstBlock) {
                setClass = "FirstBlockText";
            }
            else
                if (getCurrentBlock == SecondBlock) {
                    setClass = "SecondBlockText";
                } else
                    if (getCurrentBlock == ThirdBlock) {
                        setClass = "ThirdBlockText";
                    }

            appendElement = "<span id='item' class='minus'><input type='type' class='" + setClass + "'/><i class='fa fa-minus-circle'></i></span>";
            $(this).parent().parent().append(appendElement);
        });
    }

    function minusRow() {
        $(document).on("click", 'i.fa.fa-minus-circle', function () {
            $(this).parent().remove();
        });
    }

    function LoadTableCustomers() {

        $(window).bind("resize", function () {
            var tableWidth = $('.admin-page > .wrap').width();
            $("#usersList").setGridWidth(tableWidth, true);
        });
        $(".ui-dialog-titlebar").hide();
        $("#tabs").tabs();

        var tableWidth = $('.admin-page > .wrap').width() + 48,
            tableHeight = $(".admin-page > .wrap").height() * 3 + 50,
            lastSel = -1,
            isRowEditable = function (id) {
                return true;
            };

        $('.admin-page').css({ height: ($(window).height() - $('footer').height()) + "px" });
        $.ajax({
            url: '/AdminUser/GetUserData/',
            datatype: "json",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            method: "POST",
            success: function (result) {
                var mydata = [
                    { id: "0", FirstName: "Test name", LastName: "Test surname", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "1", FirstName: "Test name", LastName: "Test surname", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "2", FirstName: "Ivan", LastName: "Pupkin", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "3", FirstName: "Petr", LastName: "Ivanov", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "4", FirstName: "German", LastName: "Nikolaev", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "5", FirstName: "Test name", LastName: "Test surname", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "6", FirstName: "Ivan", LastName: "Pupkin", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "7", FirstName: "Petr", LastName: "Ivanov", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "8", FirstName: "German", LastName: "Nikolaev", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "9", FirstName: "Test name", LastName: "Test surname", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "10", FirstName: "Ivan", LastName: "Pupkin", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "11", FirstName: "Petr", LastName: "Ivanov", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "12", FirstName: "German", LastName: "Nikolaev", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "13", FirstName: "Test name", LastName: "Test surname", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "14", FirstName: "Ivan", LastName: "Pupkin", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "15", FirstName: "Petr", LastName: "Ivanov", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "16", FirstName: "German", LastName: "Nikolaev", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "17", FirstName: "Ivan", LastName: "Pupkin", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "18", FirstName: "Petr", LastName: "Ivanov", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "19", FirstName: "German", LastName: "Nikolaev", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "20", FirstName: "Ivan", LastName: "Pupkin", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "21", FirstName: "Petr", LastName: "Ivanov", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" },
                    { id: "22", FirstName: "Vetal", LastName: "Nikolaev", Email: "mail@example.com", Remove: "<input type='button' value='Remove'>" }
                ];
                $("#usersList").jqGrid({

                    url: '/AdminUser/GetUserData/',
                    datatype: "local",
                    colNames: ['Id', 'First name', 'Last name', 'Email', 'Delete user'],
                    colModel: [
                        {
                            name: 'Id',
                            index: 'Id',
                            key: true,
                            hidden: true,
                            sortable: false,
                        },
                        {
                            name: 'FirstName',
                            index: 'FirstName',
                            width: 80,
                            search: true,
                            sortable: false,
                            editable: true
                        },
                        {
                            name: 'LastName',
                            index: 'LastName',
                            width: 80,
                            search: true,
                            sortable: false,
                            editable: true
                        },
                        {
                            name: 'Email',
                            index: 'Email',
                            width: 80,
                            sortable: true,
                            sorttype: "text",
                            search: true,
                            sortable: false,
                            editable: true
                        },
                        {
                            name: "Remove",
                            index: "Remove",
                            formatter: function (cellvalue, options, rowobject) {
                                return "<button class='deleteBtn' onclick='DeleteCustomer(\"" + rowobject.Id + "\")' ><i class='fa fa-trash'></i></button>";
                            },
                            search: false,
                            align: 'center'
                        },
                    ],

                    data: JSON.parse(result),//mydata
                    rowNum: 15,
                    pager: '#gridpager',
                    width: tableWidth,
                    height: 510,//tableHeight,
                    rowList: [15, 30, 45, 60],
                    viewrecords: true,
                    search: true,
                    multiselect: true,
                    caption: "User list",

                    editurl: "/AdminUser/GridSave/",
                    ondblClickRow: function (id) {
                        if (isRowEditable(id)) {
                            $("#usersList").jqGrid('editRow', id, true);
                            console.log(id);
                        }
                    },

                    onSelectRow: function (id) {
                        if (id && id !== lastSel) {
                            $("#usersList").jqGrid('restoreRow', lastSel);
                            lastSel = id;
                        }
                    },

                }).navGrid("#gridpager", { edit: false, add: true, del: false, search: false, refresh: false },
                 //add options
                 {
                     zIndex: 100,
                     url: 'Test/AddGridData',
                     closeOnEscape: true,
                     closeAfterEdit: true,
                     afterComplete: function (result) { }
                 },
                 {
                     beforeShowForm: function (form) {
                         var parent = {
                             width: $("#gbox_usersList").width(),
                             height: $("#gbox_usersList").height()
                         };

                         console.log(parent);
                         $("#editmodusersList").css({ "top": parent.height / 2 + "px", "left": parent.width / 2 - 175 + "px" });
                     },
                 }).hideCol('cb');
                $("#usersList").jqGrid('filterToolbar', { searchOnEnter: false });
            }
        })
    }

}(jQuery));