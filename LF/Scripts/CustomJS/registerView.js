//alert("hellow from register view JS")

$(document).ready(
        function () {

            $("#CountriesDDL").on('change', function () {
                var countryID = $("#CountriesDDL").val();
                $.ajax({
                    url: "/Account/GetRegions",
                    data: { countryId: countryID },
                    dataType: "text",
                    type: "GET"
                })
                    .done(function (result) {
                        debugger;
                        $(".RegionsDDL").html(result);
                        $("#Items").addClass("form-control");
                    });
            });

            $("#submitFormBtn").click(function () {
                debugger;
                var htmlElement = document.querySelector(".RegionsDDL" + " div" + " select");
                var selectedValue = htmlElement.options[htmlElement.selectedIndex].value;
                $("#RegionId").val(selectedValue);
                debugger;
                htmlElement = document.querySelector(".CitiesDDL" + " div" + " select");
                selectedValue = htmlElement.options[htmlElement.selectedIndex].value;
                $("#CityId").val(selectedValue);

            })


        });

$(document).on('change', '.RegionsDDL', function () {
    var regionID = $('option:selected', $(this)).val();
    $.ajax({
        url: "/Account/GetCities",
        data: { regionId: regionID },
        dataType: "text",
        type: "GET"
    })
        .done(function (result) {
            var citiesDDL = $(".CitiesDDL");
            citiesDDL.html(result);
            document.querySelector(".CitiesDDL" + " div" + " select").className += " form-control";
        });
});