//$(".body__content").hide();
$( ".input" ).focusin(function() {
    $( this ).find( "span" ).animate({"opacity":"0"}, 200);
});
  
$( ".input" ).focusout(function() {
    $( this ).find( "span" ).animate({"opacity":"1"}, 300);
});
  
$(".login__form").submit(function(){
    $(this).find(".submit i").removeAttr('class').addClass("fa fa-check").css({"color":"#fff"});
    $(".submit").css({"background":"#2ecc71", "border-color":"#2ecc71"});
    $(".feedback").show().animate({"opacity":"1", "bottom":"-80px"}, 400);
    $("input").css({"border-color":"#2ecc71"});

    const authDto = {
        email: $("#email").val(), 
        password: $("#password").val()
    };

    $.ajax({
        type: "POST",
        headers: {          
            Accept: "application/json; charset=utf-8",         
            "Content-Type": "application/json; charset=utf-8"   
        },
        url: "/api/sysadmins/login",
        data: JSON.stringify(authDto),
        success: function (data, textStatus, xhr) {
            console.log(xhr.status);
            localStorage.setItem("email", data["email"]);
            localStorage.setItem("token", data["token"]);
            localStorage.setItem("date", data[Date.now()]);
            $(".login").hide();
            $(".header").show();
            $(".body__content").show();
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });

    return false;
});