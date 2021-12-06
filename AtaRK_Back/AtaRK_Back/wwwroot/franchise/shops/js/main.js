$(document).ready(function () {
    var start_li = `<li class="shop_list_row">
    <img src="img/shop_icon.png" alt="Shop">
    <div class="shop_info">
        <div class="shop_info_label">
            <p class="text city">Місто: </p>
            <p class="text street">Адреса: </p>
            <p class="text phone">Телефон: </p>
            <p class="text email">Пошта: </p>
        </div>
        <div class="shop_info_value">`;
    var end_li = `</div>
        </div>
    </li>`;

    $.ajax({
        type: "GET",
        url: "/api/franchises/shops/" + localStorage["fran_email"],
        headers: {          
            Accept: "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["fran_token"]
        },
        success: function (data, textStatus, xhr) {
            for (i = 0; i < data.length; i++) {
                var row = start_li;

                row += "<p class=\"text city\">" + data[i]["city"] +"</p>";
                row += "<p class=\"text street\">" + data[i]["street"] + ", " + data[i]["buildingNumber"] +"</p>";
                row += "<p class=\"text phone\">" + data[i]["contactPhone"] +"</p>";
                row += "<p class=\"text email\">" + data[i]["contactEmail"] +"</p>";
                row += end_li;

                $(".shops_list").append(row);
                $(".shop_list_row").attr("id", data[i]["id"]);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});