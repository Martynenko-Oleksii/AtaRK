$(document).ready(function() {
    var fillTable = function(data, titles, hasInnerObject) {
        $(".table").empty();

        var header = "<tr>";
        for (i = 0; i < titles.length; i++) {
            header += ("<th>" + titles[i] + "</th>");
        }
        header += "</tr>";
        $(".table").append(header);

        for (i = 0; i < data.length; i++) {
            var row = "<tr class=\"row\">";
            for (j = 0; j < titles.length; j++) {
                if (hasInnerObject && j == titles.length - 1) {
                    if (data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)] != null)
                        row += ("<td>" + data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)]["id"] + "</td>");
                }
                else {
                    if (titles[j] == "State")
                        row += ("<td>" + (data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)] == 0 ? "No answer" : "Initial answer") + "</td>");
                    else
                        row += ("<td>" + data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)] + "</td>");
                }
            }
            row += "</tr>";
            $(".table").append(row);
        }
    }

    var getMessages = function (url, titles, hasInnerObject = false) {
        $.ajax({
            type: "GET",
            headers: {          
                Accept: "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: url,
            success: function (data, textStatus, xhr) {
                //console.log(data);
                fillTable(data, titles, hasInnerObject);
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    }

    $("#tables").click(function() {
        $("#tables").css({"border-color": "#2cace7"});
        $("#messages").css({"border-color": "#fff"});
        $("#answers").css({"border-color": "#fff"});
        $(".table__menu").show();
        $(".function__menu").show();
        $(".table").empty();
        
        $(".database__table").css({"left": ""});
    });

    $("#messages").click(function() {
        $("#tables").css({"border-color": "#fff"});
        $("#messages").css({"border-color": "#2cace7"});
        $("#answers").css({"border-color": "#fff"});
        $(".table__menu").hide();
        $(".function__menu").hide();
        
        $(".database__table").css({"left": "20%"});

        getMessages("/api/messages/notready", 
            ["Id", "State", "Title", "ClimateDevice"], true);
    });

    $("#answers").click(function() {
        $("#tables").css({"border-color": "#fff"});
        $("#messages").css({"border-color": "#fff"});
        $("#answers").css({"border-color": "#2cace7"});
        $(".table__menu").hide();
        $(".function__menu").hide();
        
        $(".database__table").css({"left": "20%"});
        
        getMessages("/api/answers/" + localStorage["email"], 
            ["Id", "Answer", "TechMessage"], true);
    });

    var id;

    $(".table").on("click", ".row", function () {

        id = $(this)[0]["cells"][0]["innerText"];

        $.ajax({
            type: "GET",
            headers: {          
                Accept: "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: "/api/messages/" + id,
            success: function (data, textStatus, xhr) {
                //console.log(data);

                $("#message-title").val(data["title"]);
                $("#message").val(data["message"]);
                $("#message-email").val(data["contactEmail"]);
                $("#message-admin").val(data["shopAdmin"]["email"]);
                if (data["techMessageAnswer"] != null)
                    $("#message-answer").val(data["techMessageAnswer"]["answer"]);
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });

        $(".answer-popup").show();
    });

    $(".exit").click(function (e) { 
        e.preventDefault();

        $(".answer-popup").hide();
    });

    $(".submit-answer").click(function (e) { 
        e.preventDefault();
        
        var data = {
            id: localStorage["id"],
            answer: $("#message-answer").val()
        };

        $.ajax({
            type: "POST",
            headers: {          
                Accept: "application/json; charset=utf-8",
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            data: JSON.stringify(data),
            url: "/api/messages/answer/" + id,
            success: function (data, textStatus, xhr) {
                $(".answer-popup").hide();
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    });

    $(".close-message").click(function (e) { 
        e.preventDefault();
        
        $.ajax({
            type: "GET",
            headers: {
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: "/api/messages/close/" + id,
            success: function (data, textStatus, xhr) {
                $(".answer-popup").hide();
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    });
});