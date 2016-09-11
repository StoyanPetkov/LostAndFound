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

    /* Set the width of the side navigation to 250px */
    function openNav() {
        debugger;
        $('.DDL').css('margin-top', '.DDL', '25px')
        document.getElementById("mySidenav").style.width = "250px";
        
    }

    /* Set the width of the side navigation to 0 */
    function closeNav() {
        document.getElementById("mySidenav").style.width = "0";
    }
});
