var setLocalValues = function() {
    $("#tables").attr("value", local_tables);
    $("#messages").attr("value", local_techMessagesH);
    $("#answers").attr("value", local_answers);

    $(".legend")[0]["innerText"] = local_entering;
    $(".legend")[1]["innerText"] = local_header;

    $("#sysadmin").attr("value", local_sysadmins);
    $("#shop_admin")[0]["innerText"] = local_shopadmins;
    $("#franchise")[0]["innerText"] = local_franchises;
    $("#application")[0]["innerText"] = local_applications;
    $("#shop")[0]["innerText"] = local_shops;
    $("#device")[0]["innerText"] = local_devicesM;
    $("#climate")[0]["innerText"] = local_sclimateStates;
    $("#tech_message")[0]["innerText"] = local_techMessages;
    
    $("#export").attr("value", local_export);
    $("#import").attr("value", local_import);
    $("#copy").attr("value", local_copy);
    $("#add").attr("value", local_addAdmin);
    $("#climate__get").attr("value", local_getStaatistic);

    $("#email").attr("placeholder", local_email);
    $("#password").attr("placeholder", local_password);

    $("#email__reg").attr("placeholder", local_email);
    $("#password__reg").attr("placeholder", local_password);
    $("#confirm__password").attr("placeholder", local_repeatPassword);

    $(".general_info__label")[0]["innerText"] = local_messageTitle;
    $(".general_info__label")[1]["innerText"] = local_messageDescription;
    $(".general_info__label")[2]["innerText"] = local_contactEmail;
    $(".general_info__label")[3]["innerText"] = local_messageAdmin;
    $(".general_info__label")[4]["innerText"] = local_messageAnswer;
    $("#message-answer").attr("placeholder", local_messageAnswer);
    $(".exit").attr("value", local_close);
    $(".submit-answer")[0]["innerText"] = local_answerBtn;
    $(".close-message").attr("value", local_closeMessage);
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
    });
});