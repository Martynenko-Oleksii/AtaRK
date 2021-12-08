$("#add_foto").change(function (e) { 
    e.preventDefault();
    
    var formData = new FormData();
    var files = $('#add_foto')[0].files;

    if(files.length > 0 ) {
        formData.append("banner", files[0]);
        formData.append("email", localStorage["fran_email"]);

        $.ajax({
            url: "/api/franchises/banner",
            type: "POST",
            data: formData,
            contentType: false,
            headers: {
                Accept: "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["fran_token"]
            },
            processData: false,
            success: function (data, textStatus, xhr) {
                for (i = 0; i < data["franchiseImages"].length; i++) {
                    if (data["franchiseImages"][i]["isBanner"]) {
                        $("#banner__img").attr("src", "../.." + data["franchiseImages"][i]["path"]);
                        //console.log($("#banner__img").attr("src"));
                        break;
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            }
        });
    }
});

$(".general_info__form").submit(function (e) { 
    e.preventDefault();

    var data_fran = {
        email: localStorage["fran_email"],
        title: $("#title").val(),
        description: $("#description").val(),
        minTemperature: parseFloat($("#mint").val()),
        maxTemperature: parseFloat($("#maxt").val()),
        minHuumidity: parseFloat($("#minh").val()),
        maxHuumidity: parseFloat($("#maxh").val()),
    };

    $.ajax({
        type: "POST",
        headers: {          
            Accept: "application/json; charset=utf-8",         
            "Content-Type": "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["fran_token"]
        },
        url: "/api/franchises/info",
        data: JSON.stringify(data_fran),
        success: function (data, textStatus, xhr) {
            $("#title").val(data["title"]);
            $("#description").val(data["description"]);
            $("#mint").val(data["minTemperature"]);
            $("#maxt").val(data["maxTemperature"]);
            $("#minh").val(data["minHuumidity"]);
            $("#maxh").val(data["maxHuumidity"]);
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});

$("#add_img").change(function (e) { 
    e.preventDefault();
    
    var formData = new FormData();
    var files = $('#add_img')[0].files;

    if(files.length > 0 ) {
        formData.append("images", files[0]);
        formData.append("email", localStorage["fran_email"]);

        $.ajax({
            url: "/api/franchises/images",
            type: "POST",
            data: formData,
            contentType: false,
            headers: {
                Accept: "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["fran_token"]
            },
            processData: false,
            async: false,
            success: function (data, textStatus, xhr) {
                var j = 0;
                for (i = 0; i < data["franchiseImages"].length; i++) {
                    if (!data["franchiseImages"][i]["isBanner"]) {
                        j++;
                        var slide = "<div id=\"" + data["franchiseImages"][i]["id"] + "\" class=\"mySlides fade\">";
                        slide += "<div class=\"numbertext\">" + j + " / 3</div>";
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
});

$("#contact_info__button").click(function (e) { 
    e.preventDefault();

    $(".popup").css("visibility", "visible");
});

$("#info_type").change(function (e) { 
    e.preventDefault();

    var selectedValue = $(this).val();
    if (selectedValue == "url") {
        $(".url__visibility").css("visibility", "visible");
    }
});

$(".info__close").click(function (e) { 
    e.preventDefault();
    
    $(".popup").css("visibility", "hidden");
});

$(".contact_info__form").submit(function (e) { 
    e.preventDefault();
    
    var infoType = $("#info_type").val();
    var urlType = $("#url_type").val();
    var value = $("#value").val();

    //console.log(infoType + " " + urlType + " " + value);

    var isPhone = false, isEmail = false, isUrl = false;
    switch (infoType) {
        case "phone":
            isPhone = true;
            urlType = null;
            break;
        case "email":
            isEmail = true;
            urlType = null;
            break;
        case "url":
            isUrl = true;
            break;
    }

    var infoData = {
        value: value,
        isPhone: isPhone,
        isEmail: isEmail,
        isUrl: isUrl,
        urlType: urlType
    };

    $.ajax({
        type: "POST",
        headers: {          
            Accept: "application/json; charset=utf-8",         
            "Content-Type": "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["fran_token"]
        },
        url: "/api/franchises/contactinfo/" + localStorage["fran_email"],
        data: JSON.stringify(infoData),
        success: function (data, textStatus, xhr) {
            $(".popup").css("visibility", "hidden");

            document.location.reload();
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});

$(".delete__row").click(function (e) { 
    e.preventDefault();
    
    var id = parseInt($(this).parent().attr("id"), 10);
    var email = localStorage["fran_email"];

    var formData = new FormData();
    formData.append("email", email);
    formData.append("id", id);

    $.ajax({
        url: "/api/franchises/contactinfo/delete",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        headers: {
            Accept: "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["fran_token"]
        },
        success: function (data, textStatus, xhr) {
            $("#" + id).remove();
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        }
    });
});

var imageId = 0;

$("#delete_img").click(function (e) { 
    e.preventDefault();
    
    if (imageId != 0) {
        var lastImageId = $(".mySlides").last()[0]["attributes"]["id"]["value"];
        var nextId = 0;
        if (imageId != lastImageId) {
            nextId = $("#" + imageId).next()[0]["attributes"]["id"]["value"];
        } else {
            nextId = $(".mySlides")[0]["attributes"]["id"]["value"];
        }
        $.ajax({
            type: "GET",
            headers: {
                "Authorization": "Bearer " + localStorage["fran_token"]
            },
            url: "/api/franchises/images/delete/" + imageId,
            success: function (data, textStatus, xhr) {
                $("#" + nextId).css("display", "block");
                $("#" + imageId).remove();

                if (nextId == imageId) {
                    $(".next").remove();
                    $(".prev").remove();
                }

                imageId = nextId;
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    }
});