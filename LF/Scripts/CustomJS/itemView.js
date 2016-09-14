$(document).ready(function () {

    $('#RegionsDDL').on('change', function () {
        var id = $(this).val();
        $.ajax({
            url: "/item/getCities",
            data: { regionId: id },
            datatype: "json",
            type: "get",
            success: function (data) {
                if (data !== null) {
                    $('#CitiesDDL').html('')
                    $('#CitiesDDL').append("<select>--Град--</select>");
                    for (var i in data) {
                        var text = data[i].Text;
                        var value = data[i].Value;
                        $('#CitiesDDL').append("<option value='" + value + "'>" + text + "</option>")
                    }
                }
            },
            error: function (data) {
                alert(data.error);
            }
        });
    });
    

    $.ajax({
        url: "/item/getsidemenu",
        //data: { name: username, type: usertype,id: id},
        datatype: "json",
        type: "get",
        success: function (data) {
            var menu = $('#main');
            menu.append(data);

            $('#regionsDDL').on('change', (function () {
                debugger;
                var id = $(this).val();
                $.ajax({
                    url: "/item/getCities",
                    data: { regionId: id },
                    datatype: "json",
                    type: "get",
                    success: function (data) {
                        if (data !== null) {
                            $('#cityDDL').html('')
                            $('#cityDDL').append("<select>--Град--</select>");
                            for (var i in data) {
                                var text = data[i].Text;
                                var value = data[i].Value;
                                $('#cityDDL').append("<option value='" + value + "'>" + text + "</option>")
                            }
                        }
                    },
                    error: function (data) {
                        alert(data.error);
                    }
                });
            }));
        },
        error: function (data) {
            alert(data);
        }
    });

   
    //$('#isLost').on('click', function () {
    //    debugger;
    //    if ($(this).is(":checked")) {
    //        alert('Clicked')
    //    } else {
    //        alert('notChecked')
    //    }
    //})
});
