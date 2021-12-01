var table;

$("#add").click(function (e) { 
    e.preventDefault();
    
    table = tableName;
    tableName = null;

    $(".database__table").css("visibility", "hidden");
    $(".popup").show();
});

$(".register__form").submit(function (e) { 
    e.preventDefault();
    
    if ($("#password__reg").val() == $("#confirm__password").val()) {
        var url = "/api/";

        if (table == "SystemAdmins") {
            url += "sysadmins/register";
        } else if (table == "ShopAdmins") {
            url += "shopadmins/register";
        }

        var authData = {
            email: $("#email__reg").val(),
            password: $("#password__reg").val()
        };

        $.ajax({
            type: "POST",
            headers: {          
                Accept: "application/json; charset=utf-8",         
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: url,
            data: JSON.stringify(authData),
            success: function (data, textStatus, xhr) {
                document.location.reload();
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    }
});