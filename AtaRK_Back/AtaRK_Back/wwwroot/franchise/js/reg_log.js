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
        email: $("#email__log").val(), 
        password: $("#password__log").val()
    };

    $.ajax({
        type: "POST",
        headers: {          
            Accept: "application/json; charset=utf-8",         
            "Content-Type": "application/json; charset=utf-8"   
        },
        url: "/api/franchises/login",
        data: JSON.stringify(authDto),
        success: function (data, textStatus, xhr) {
            //console.log(xhr.status);
            localStorage.setItem("fran_id", data["id"]);
            localStorage.setItem("fran_email", data["email"]);
            localStorage.setItem("fran_token", data["token"]);
            localStorage.setItem("fran_date", Date.now());
            $(".reg__log").hide();
            $(".main").show();

            getFranchise();
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });

    return false;
});

$(".register__form").submit(function(){
    $(this).find(".submit i").removeAttr('class').addClass("fa fa-check").css({"color":"#fff"});
    $(".submit").css({"background":"#2ecc71", "border-color":"#2ecc71"});
    $(".feedback").show().animate({"opacity":"1", "bottom":"-80px"}, 400);
    $("input").css({"border-color":"#2ecc71"});

    var password = $("#password__reg").val();
    var confirmPass = $("#confirm__password").val();

    if (password != confirmPass) {
        console.log("Password Error");

        return false;
    }

    const authDto = {
        email: $("#email__reg").val(), 
        password: password
    };

    $.ajax({
        type: "POST",
        headers: {          
            Accept: "application/json; charset=utf-8",         
            "Content-Type": "application/json; charset=utf-8"   
        },
        url: "/api/franchises/register",
        data: JSON.stringify(authDto),
        success: function (data, textStatus, xhr) {
            //console.log(xhr.status);
            localStorage.setItem("fran_email", data["email"]);
            localStorage.setItem("fran_token", data["token"]);
            localStorage.setItem("fran_date", Date.now());
            $(".reg__log").hide();
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

$(".register__btn").click(function (e) { 
    e.preventDefault();
    
    $(".login__form").hide();
    $(".register__form").show();
});

$(".login__btn").click(function (e) { 
    e.preventDefault();
    
    $(".register__form").hide();
    $(".login__form").show();
});