$(document).ready(function () {

    $.ajax({
        url: "/item/getsidemenu",
        //data: { name: username, type: usertype,id: id},
        datatype: "json",
        type: "get",
        success: function (data) {
            var menu = $('#main');
            menu.append(data);
        },
        error: function (data) {
            alert(data);
        }
    });

    $('#isLost').on('click', function () {
        debugger;
        if ($(this).is(":checked")) {
            alert('Clicked')
        } else {
            alert('notChecked')
        }
    })
});
