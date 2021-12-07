var setLocalValues = function() {
    $("#main__btn")[0]["innerText"] = local_main;
    $("#franchise__btn")[0]["innerText"] = local_franchise;
    $("#shop__btn")[0]["innerText"] = local_shop;
    
    $(".legend")[0]["innerText"] = local_entering;
    $(".legend")[1]["innerText"] = local_registeration;
    $("#email__log").attr("placeholder", local_email);
    $("#email__reg").attr("placeholder", local_email);
    $("#password__log").attr("placeholder", local_password);
    $("#password__reg").attr("placeholder", local_password);
    $("#confirm__password").attr("placeholder", local_repeatPassword);
    $(".register__btn").attr("value", local_register);
    $(".login__btn").attr("value", local_enter);
    
    $("#exit").attr("value", local_exit);
    $("#shops")[0]["innerText"] = local_shops;
    $("#delete").attr("value", local_delete);

    $(".custom-file-upload")[0]["innerHTML"] = "<i class=\"fa fa-cloud-upload\"></i> " + local_addBanner;
    $(".custom-file-upload")[1]["innerHTML"] = "<i class=\"fa fa-cloud-upload\"></i> " + local_addImage;

    $(".general_info__label")[0]["innerText"] = local_franTitle;
    $("#title").attr("placeholder", local_franTitle);
    $(".general_info__label")[1]["innerText"] = local_franDescription;
    $("#description").attr("placeholder", local_franDescription);
    $(".general_info__label")[2]["innerText"] = local_mitTemperature;
    $("#mint").attr("placeholder", local_mitTemperature);
    $(".general_info__label")[3]["innerText"] = local_maxTemperature;
    $("#maxt").attr("placeholder", local_maxTemperature);
    $(".general_info__label")[4]["innerText"] = local_minHumidity;
    $("#minh").attr("placeholder", local_minHumidity);
    $(".general_info__label")[5]["innerText"] = local_maxHumidity;
    $("#maxh").attr("placeholder", local_maxHumidity);
    $(".info__submit")[0]["innerText"] = local_save;
    $(".info__submit")[1]["innerText"] = local_save;

    $("#contact_info__button").attr("value", local_addContacts);
    $(".general_info__label")[6]["innerText"] = local_typeLabel;
    $("#info_type")[0][0]["innerText"] = local_typePhone;
    $("#info_type")[0][1]["innerText"] = local_typeEmail;
    $("#info_type")[0][2]["innerText"] = local_typeUrl;
    $(".general_info__label")[7]["innerText"] = local_urlLabel;
    $("#url_type")[0][0]["innerText"] = local_urlWeb;
    $("#url_type")[0][1]["innerText"] = local_urlSocial;
    $(".general_info__label")[8]["innerText"] = local_value;
    $("#value").attr("placeholder", local_value);
    $(".info__close")[0]["innerText"] = local_close;

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
        var localScript = "<script id=\"localScript\" src=\"/assets/js/local_" + lang + ".js\"></script>";
        $("#jq").after(localScript);

        $("#" + lang + "__btn").css("border-bottom", "3px solid #2cace7");
        $("#" + lang + "__btn").css("color", "#2cace7");
        setLocalValues();
    }

    $("#ua__btn").click(function (e) { 
        e.preventDefault();

        setLocalLang("ua");
        $("#localScript").remove();
        var localScript = "<script id=\"localScript\" src=\"/assets/js/local_ua.js\"></script>";
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
        var localScript = "<script id=\"localScript\" src=\"/assets/js/local_en.js\"></script>";
        $("#jq").after(localScript);

        $("#en__btn").css("border-bottom", "3px solid #2cace7");
        $("#en__btn").css("color", "#2cace7");
        $("#ua__btn").css("border-bottom", "");
        $("#ua__btn").css("color", "");
        setLocalValues();
        
        //window.location.reload();
    });
});