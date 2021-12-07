var setLocalValues = function() {
    $("#main__btn")[0]["innerText"] = local_main;
    $("#franchise__btn")[0]["innerText"] = local_franchise;
    $("#shop__btn")[0]["innerText"] = local_shop;

    $(".legend")[0]["innerText"] = local_entering;
    $("#email__log").attr("placeholder", local_email);
    $("#password__log").attr("placeholder", local_password);

    $("#exit").attr("value", local_exit);
    $("#shops")[0]["innerText"] = local_devices;
    $("#delete").attr("value", local_delete);

    $(".general_info__label")[0]["innerText"] = local_franchise;
    $(".general_info__label")[1]["innerText"] = lcoal_city;
    $("#city").attr("placeholder", lcoal_city);
    $(".general_info__label")[2]["innerText"] = local_street;
    $("#street").attr("placeholder", local_street);
    $(".general_info__label")[3]["innerText"] = local_building;
    $("#buiding").attr("placeholder", local_building);
    $(".general_info__label")[4]["innerText"] = local_contactEmail;
    $("#contact_email").attr("placeholder", local_contactEmail);
    $(".general_info__label")[5]["innerText"] = local_contactPhone;
    $("#contact_phone").attr("placeholder", local_contactPhone);
    $(".info__submit")[0]["innerText"] = local_save;

    $("#footerMain")[0]["innerText"] = local_main;
    $("#footerFran")[0]["innerText"] = local_franchise;
    $("#footerShop")[0]["innerText"] = local_shop;
};

var setLocalLang = function(lang) {
    if (localStorage["local"] != undefined) {
        localStorage["local"] = lang;
    } else {
        localStorage.setItem("local", lang);
    }
};

$(function () {
    if (localStorage["local"] === undefined) {
        setLocalLang("en");
    } else {
        var lang = localStorage["local"];
        var localScript = "<script id=\"localScript\" src=\"../../assets/js/local_" + lang + ".js\"></script>";
        $("#jq").after(localScript);

        $("#" + lang + "__btn").css("border-bottom", "3px solid #2cace7");
        $("#" + lang + "__btn").css("color", "#2cace7");
        setLocalValues();
    }

    $("#ua__btn").click(function (e) { 
        e.preventDefault();

        setLocalLang("ua");
        $("#localScript").remove();
        var localScript = "<script id=\"localScript\" src=\"../../assets/js/local_ua.js\"></script>";
        $("#jq").after(localScript);

        $("#ua__btn").css("border-bottom", "3px solid #2cace7");
        $("#ua__btn").css("color", "#2cace7");
        $("#en__btn").css("border-bottom", "");
        $("#en__btn").css("color", "");
        setLocalValues();

        //window.location.reload();
    });
    
    $("#en__btn").click(function (e) { 
        e.preventDefault();

        setLocalLang("en");
        $("#localScript").remove();
        var localScript = "<script id=\"localScript\" src=\"../../assets/js/local_en.js\"></script>";
        $("#jq").after(localScript);

        $("#en__btn").css("border-bottom", "3px solid #2cace7");
        $("#en__btn").css("color", "#2cace7");
        $("#ua__btn").css("border-bottom", "");
        $("#ua__btn").css("color", "");
        setLocalValues();
        
        //window.location.reload();
    });
});