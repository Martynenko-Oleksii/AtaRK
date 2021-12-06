$(document).ready(function () {
    $(".popup").hide();

    var start_li = `<li class="devices_list_row">
    <img id="device-icon" src="img/device_icon.png" alt="Device">
    <div class="device_info">
        <div class="device_info_label">
            <p class="text city">Номер: </p>
            <p class="text street">Стан: </p>
        </div>
        <div class="device_info_value">`;
    var end_li = `</div>
        </div>
    </li>`;

    $.ajax({
        type: "GET",
        url: "/api/shops/devices/" + localStorage["shop_email"],
        headers: {          
            Accept: "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["shop_token"]
        },
        success: function (data, textStatus, xhr) {
            for (i = 0; i < data.length; i++) {
                var row = start_li;

                row += "<p class=\"text number\" id=\"number_" + data[i]["id"] + 
                    "\">" + data[i]["deviceNumber"] +"</p>";
                row += "<p class=\"text state\" style=\"color:" + (data[i]["isOnline"] ? "green" : "red") + 
                    "\">" + (data[i]["isOnline"] ? "online" : "offline") + "</p>";
                row += end_li;

                $(".devices_list").append(row);
                $(".devices_list li:last-child").attr("id", data[i]["id"]);

                if (data["picturePath"] != null) {
                    $("#device-icon").attr("src", "../../.." + data["picturePath"]);
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});