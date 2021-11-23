if (localStorage["date"] != null) {
    if (Date.now() - localStorage["date"] > 3600000) {
        $(".body__content").hide();
        $(".login").show();
    }
    else {
        $(".login").hide();
        $(".body__content").show();
        $("#add").hide();
        $("#climate__get").hide();
    }
}
else {
    $(".body__content").hide();
    $(".login").show();
}