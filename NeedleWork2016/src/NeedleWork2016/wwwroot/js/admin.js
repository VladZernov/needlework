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
        url: '/AdminUser/DeleteUserData/'+rowId,
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
        $('.deleteSuccess').dialog({
            autoOpen:false
        });

        $(".ui-dialog-titlebar").hide();

        $("#tabs").tabs();
        $("#localization").click(function () {
            $("#grid").hide();
        });


    });
    function addRow() {

        $('i.fa.fa-plus-circle').click(function () {
            $(this).parent().parent().append('<span class="minus"><input type="type" name="name" value=" " /><i class="fa fa-minus-circle"></i></span>');
        });
    }
    function minusRow() {
        $(document).on("click", 'i.fa.fa-minus-circle', function () {
            $(this).parent().css({ "display": "none" });
        });
    }

    function LoadTableCustomers() {

        $(window).bind("resize", function () {
            var tableWidth = $('.admin-page > .wrap').width();
            $("#usersList").setGridWidth(tableWidth, true);
        });

        var tableWidth = $('.admin-page > .wrap').width() + 48,
            tableHeight = $(".admin-page > .wrap").height()*3 + 50,
            lastSel = -1,
            isRowEditable = function (id) {
            // implement your criteria here 
            return true;
        };

        $('.admin-page').css({ height: ($(window).height() - $('footer').height()) + "px" });
        $.ajax({
            url: '/AdminUser/GetUserData/',
            datatype: "json",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            method: "GET",
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
                    
                    data: mydata,//JSON.parse(result),
                    rowNum: 15,
                    pager: '#gridpager',
                    width: tableWidth,
                    height: 510,//tableHeight,
                    rowList: [15, 30, 45, 60],
                    viewrecords: true,
                    search: true,
                    multiselect: true,
                    caption: "User list",

                    ondblClickRow: function (id, ri, ci) {
                        if (isRowEditable(id)) {
                            // edit the row and save it on press "enter" key
                            $("#usersList").jqGrid('editRow', id, true);
                        }
                    },

                    onSelectRow: function (id) {
                        if (id && id !== lastSel) {
                            // cancel editing of the previous selected row if it was in editing state.
                            // jqGrid hold intern savedRow array inside of jqGrid object,
                            // so it is safe to call restoreRow method with any id parameter
                            // if jqGrid not in editing state
                            $("#usersList").jqGrid('restoreRow', lastSel);
                            lastSel = id;
                        }
                    },        
                    
                }).hideCol('cb');

                $("#usersList").jqGrid('filterToolbar', { searchOnEnter: false });
            }
        })
    }

}(jQuery));