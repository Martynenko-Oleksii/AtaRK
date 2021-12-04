if (localStorage["shop_date"] != null) {
    if (Date.now() - localStorage["shop_date"] > 3600000) {
        $(".main").hide();
        $(".login").show();

        $(".footer").css("margin-top", "667px");
    }
    else {
        $(".login").hide();
        $(".main").show();
        $(".copyright").css("margin-top", "64px");

        getShop();
    }
}
else {
    $(".main").hide();
    $(".login").show();

    $(".footer").css("margin-top", "667px");
}

$(document).ready(function () {
    $("#shop__btn").css({"border-color": "#2cace7"});
});

$(".copyright-menu").css("margin-top", "0px");