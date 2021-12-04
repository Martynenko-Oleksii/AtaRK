$(".general_info__form").submit(function (e) { 
    e.preventDefault();

    var data_shop = {
        id: localStorage["shop_id"],
        city: $("#city").val(),
        street: $("#street").val(),
        buildingNumber: $("#buiding").val(),
        contactPhone: $("#contact_phone").val(),
        contactEmail: $("#contact_email").val()
    };

    $.ajax({
        type: "POST",
        headers: {          
            Accept: "application/json; charset=utf-8",         
            "Content-Type": "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["shop_token"]
        },
        url: "/api/shops/update",
        data: JSON.stringify(data_shop),
        success: function (data, textStatus, xhr) {
            $("#city").val(data["city"]);
            $("#street").val(data["street"]);
            $("#buiding").val(data["buildingNumber"]);
            $("#contact_phone").val(data["contactPhone"]);
            $("#contact_email").val(data["contactEmail"]);
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});