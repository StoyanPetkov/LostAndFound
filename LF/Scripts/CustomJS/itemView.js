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

});
