$(".popup").hide();
$(".answer-popup").hide();

if (localStorage["date"] != null) {
    if (Date.now() - localStorage["date"] > 3600000) {
        $(".header").hide();
        $(".body__content").hide();
        $(".login").show();
    }
    else {
        $(".login").hide();
        $(".header").show();
        $(".body__content").show();
        $("#tables").css({"border-color": "#2cace7"});
        $("#messages").css({"border-color": "#fff"});
        $("#answers").css({"border-color": "#fff"});
        $("#add").hide();
        $("#climate__get").hide();
        $("#climate__get__id").hide();
    }
}
else {
    $(".header").hide();
    $(".body__content").hide();
    $(".login").show();
}