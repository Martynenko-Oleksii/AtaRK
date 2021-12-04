$( ".input" ).focusin(function() {
    $( this ).find( "span" ).animate({"opacity":"0"}, 200);
});
  
$( ".input" ).focusout(function() {
    $( this ).find( "span" ).animate({"opacity":"1"}, 300);
});

$(".login__form").submit(function (e) { 
    e.preventDefault();

    $(this).find(".submit i").removeAttr('class').addClass("fa fa-check").css({"color":"#fff"});
    $(".submit").css({"background":"#2ecc71", "border-color":"#2ecc71"});
    $(".feedback").show().animate({"opacity":"1", "bottom":"-80px"}, 400);
    $("input").css({"border-color":"#2ecc71"});

    $(".copyright").css("margin-top", "64px");
    
    const authDto = {
        email: $("#email__log").val(), 
        password: $("#password__log").val()
    };

    $.ajax({
        type: "POST",
        headers: {          
            Accept: "application/json; charset=utf-8",         
            "Content-Type": "application/json; charset=utf-8"   
        },
        url: "/api/shops/login",
        data: JSON.stringify(authDto),
        success: function (data, textStatus, xhr) {
            //console.log(xhr.status);
            localStorage.setItem("shop_id", data["id"]);
            localStorage.setItem("shop_email", data["email"]);
            localStorage.setItem("shop_token", data["token"]);
            localStorage.setItem("shop_date", Date.now());
            $(".login").hide();
            $(".main").show();
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });

    return false;
});