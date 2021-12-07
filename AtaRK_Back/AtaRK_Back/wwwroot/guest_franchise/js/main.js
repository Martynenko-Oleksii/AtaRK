var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};

$(function () {
    $(".popup").hide();

    var id = getUrlParameter("id");
    var email = localStorage["franchise_" + id];

    $.ajax({
        type: "GET",
        headers: {          
            Accept: "application/json; charset=utf-8"
        },
        url: "/api/franchises/" + email,
        async: false,
        success: function (data, textStatus, xhr) {
            for (i = 0; i < data["franchiseImages"].length; i++) {
                if (data["franchiseImages"][i]["isBanner"]) {
                    $("#banner__img").attr("src", "../.." + data["franchiseImages"][i]["path"]);
                    break;
                }
            }
            
            $("#title").append(data["title"]);
            $("#description").append(data["description"]);
            $("#mint").append(data["minTemperature"] + "&#176;");
            $("#maxt").append(data["maxTemperature"] + "&#176;");
            $("#minh").append(data["minHuumidity"] + "%");
            $("#maxh").append(data["maxHuumidity"] + "%");

            var j = 0;
            for (i = 0; i < data["franchiseImages"].length; i++) {
                j++;
                if (!data["franchiseImages"][i]["isBanner"]) {
                    var slide = "<div class=\"mySlides fade\">";
                    slide += "<div class=\"numbertext\">" + j + "</div>";
                    slide += "<img src=\"" + "../.." + data["franchiseImages"][i]["path"] + "\">";
                    slide += "</div>";
                    $(".imgs").append(slide);
                }
            }

            if (j > 1) {
                var prevButton = "<a class=\"prev\" onclick=\"plusSlides(-1)\">&#10094;</a>";
                var nextButton = "<a class=\"next\" onclick=\"plusSlides(1)\">&#10095;</a>";
                $(".imgs").append(prevButton);
                $(".imgs").append(nextButton);
            }
            
            for (i = 0; i < data["franchiseContactInfos"].length; i++) {
                var row = "<li class=\"contact_info__row\" id=\"" + data["franchiseContactInfos"][i]["id"] + "\"><p>";

                if (data["franchiseContactInfos"][i]["isPhone"]) {
                    row += data["franchiseContactInfos"][i]["value"] + "</p>";
                    row += "<span id=\"phone__i\"><i class=\"fa fa-phone\"></i></span></li>";

                    $("#contact_phone").append(row);
                } else if (data["franchiseContactInfos"][i]["isEmail"]) {
                    row += data["franchiseContactInfos"][i]["value"] + "</p>";
                    row += "<span id=\"email__i\"><i class=\"fa fa-envelope-o\"></i></span></li>";

                    $("#contact_email").append(row);
                } else if (data["franchiseContactInfos"][i]["isUrl"]) {
                    row += "<a href=\"" + data["franchiseContactInfos"][i]["value"] + "\">" + local_url + "</a></p>";
                    row += "<span id=\"url__i\"><i class=\"fa fa-link\"></i></i></span></li>";

                    $("#contact_url").append(row);
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});