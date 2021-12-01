var getFranchise = function() {
    $.ajax({
        type: "GET",
        headers: {          
            Accept: "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["fran_token"]
        },
        url: "/api/franchises/" + localStorage["fran_email"],
        async: false,
        success: function (data, textStatus, xhr) {
            for (i = 0; i < data["franchiseImages"].length; i++) {
                if (data["franchiseImages"][i]["isBanner"]) {
                    $("#banner__img").attr("src", "../.." + data["franchiseImages"][i]["path"]);
                    //console.log($("#banner__img").attr("src"));
                    break;
                }
            }
            
            $("#title").val(data["title"]);
            $("#description").val(data["description"]);
            $("#mint").val(data["minTemperature"]);
            $("#maxt").val(data["maxTemperature"]);
            $("#minh").val(data["minHuumidity"]);
            $("#maxh").val(data["maxHuumidity"]);

            var j = 0;
                for (i = 0; i < data["franchiseImages"].length; i++) {
                    if (!data["franchiseImages"][i]["isBanner"]) {
                        j++;
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
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
}