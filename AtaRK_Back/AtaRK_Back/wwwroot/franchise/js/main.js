$(".register__form").hide();

if (localStorage["fran_date"] != null) {
    if (Date.now() - localStorage["fran_date"] > 3600000) {
        $(".main").hide();
        $(".reg__log").show();
    }
    else {
        $(".reg__log").hide();
        $(".main").show();

        getFranchise();
    }
}
else {
    $(".main").hide();
    $(".reg__log").show();
}

$(document).ready(function () {
    $("#franchise__btn").css({"border-color": "#2cace7"});
});