$(".register__form").hide();

if (localStorage["fran_date"] != null) {
    if (Date.now() - localStorage["fran_date"] > 3600000) {
        $(".main").hide();
        $(".reg__log").show();
        $(".copyright").css("margin-top", "648px");
    }
    else {
        $(".reg__log").hide();
        $(".main").show();
        $(".copyright").css("top", "");

        getFranchise();
    }
}
else {
    $(".main").hide();
    $(".reg__log").show();
    $(".copyright").css("margin-top", "648px");
}

$(document).ready(function () {
    $("#franchise__btn").css({"border-color": "#2cace7"});
});

$(".copyright-menu").css("margin-top", "0px");