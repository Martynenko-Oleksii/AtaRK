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
            var row = "<tr>";
            for (j = 0; j < titles.length; j++) {
                if (hasInnerObject && j == titles.length - 1) {
                    if (data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)] != null)
                        row += ("<th>" + data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)]["id"] + "</th>");
                }
                else {
                    row += ("<th>" + data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)] + "</th>");
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
});