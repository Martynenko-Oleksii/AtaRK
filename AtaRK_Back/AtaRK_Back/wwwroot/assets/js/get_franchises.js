var fillList = function(data, searchString) {
    var franchise = ""; 

    //console.log(searchString.length);

    for (i = 0; i < data.length; i++) {
        if (searchString.length > 2 && 
            data[i]["title"].toLowerCase().includes(searchString.toLowerCase())) {
            franchise += "<li class=\"franchises_list__row\" ";
            franchise += "id=\"" + data[i]["id"] + "\">";
            franchise += "<a href=\"guest_franchise/?id=" + data[i]["id"] + "\">";

            var bannerPath = "";
            for (j = 0; j < data[i]["franchiseImages"].length; j++) {
                if (data[i]["franchiseImages"][j]["isBanner"]) {
                    bannerPath = data[i]["franchiseImages"][j]["path"];
                    break;
                }
            }

            if (bannerPath.length < 4) {
                bannerPath = "images/banner.png";
            }

            franchise += "<img src=\"" + bannerPath + "\" alt=\"" + data[i]["title"] + "\">";
            franchise += "</a></li>";
        } else if (searchString.length == 0) {
            franchise += "<li class=\"franchises_list__row\" ";
            franchise += "id=\"" + data[i]["id"] + "\">";
            franchise += "<a href=\"guest_franchise/?id=" + data[i]["id"] + "\">";

            var bannerPath = "";
            for (j = 0; j < data[i]["franchiseImages"].length; j++) {
                if (data[i]["franchiseImages"][j]["isBanner"]) {
                    bannerPath = data[i]["franchiseImages"][j]["path"];
                    break;
                }
            }

            if (bannerPath.length < 4) {
                bannerPath = "images/banner.png";
            }

            franchise += "<img src=\"" + bannerPath + "\" alt=\"" + data[i]["title"] + "\">";
            franchise += "</a></li>";
        }
    }

    if (franchise.length > 0) {
        $("#franchises_list").append(franchise);
    }
}

var getFranchises = function(searchString = "") {
    $.ajax({
        type: "GET",
        headers: {          
            Accept: "application/json; charset=utf-8"
        },
        url: "/api/franchises",
        success: function (data, textStatus, xhr) {
            $("#franchises_list").empty();
            //console.log(searchString);
            fillList(data, searchString);
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
}