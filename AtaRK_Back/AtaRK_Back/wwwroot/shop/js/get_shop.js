var getShop = function() {
    $.ajax({
        type: "GET",
        headers: {          
            Accept: "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["shop_token"]
        },
        url: "/api/shops/" + localStorage["shop_email"],
        success: function (data, textStatus, xhr) {
            $("#franchise").val(data["fastFoodFranchise"]["title"]);
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
}