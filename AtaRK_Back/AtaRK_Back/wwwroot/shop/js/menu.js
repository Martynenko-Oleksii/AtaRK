$("#exit").click(function (e) { 
    e.preventDefault()
    
    localStorage.removeItem("shop_date");
    localStorage.removeItem("shop_id");
    localStorage.removeItem("shop_email");
    localStorage.removeItem("shop_token");

    document.location.reload();
});

$("#delete").click(function (e) { 
    e.preventDefault();

    $.ajax({
        type: "GET",
        headers: {          
            Accept: "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["shop_token"]
        },
        url: "/api/shops/delete/" + localStorage["shop_email"],
        success: function (data, textStatus, xhr) {
            localStorage.removeItem("shop_date");
            localStorage.removeItem("shop_id");
            localStorage.removeItem("shop_email");
            localStorage.removeItem("shop_token");
        
            document.location.reload();
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});