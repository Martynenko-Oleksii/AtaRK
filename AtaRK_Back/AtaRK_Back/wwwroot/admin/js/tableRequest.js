var tableName;

$(document).ready(function() {
    var fillTable = function(data, titles, hasInnerObject) {
        $(".table").empty();

        var header = "<thead><tr>";
        for (i = 0; i < titles.length; i++) {
            header += ("<th>" + titles[i] + "</th>");
        }
        header += "</tr></thead>";
        $(".table").append(header);

        var tbody = "<tbody>";
        for (i = 0; i < data.length; i++) {
            var row = "<tr>";
            for (j = 0; j < titles.length; j++) {
                if (hasInnerObject && j == titles.length - 1) {
                    if (data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)] != null)
                        row += ("<td>" + data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)]["id"] + "</td>");
                }
                else {
                    row += ("<td>" + data[i][titles[j].charAt(0).toLowerCase() + titles[j].slice(1)] + "</td>");
                }
            }
            row += "</tr>";
            tbody += row;
        }
        tbody += "</tbody>";
        $(".table").append(tbody);
    }

    var getData = function(titles, url, table, hasInnerObject = false) {
        $.ajax({
            type: "GET",
            headers: {          
                Accept: "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: url,
            success: function (data, textStatus, xhr) {
                tableName = table;
                fillTable(data, titles, hasInnerObject);
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    }

    $("#sysadmin").click(function() {
        getData(["Id", "Email", "Name", "IsMaster"], "/api/sysadmins", "SystemAdmins");
        
        $("#climate__get").hide();
        $("#climate__get__id").hide();
        $("#add").show();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });

    $("#shop_admin").click(function() {
        getData(["Id", "Email"], "/api/shopadmins", "ShopAdmins");
        
        $("#climate__get").hide();
        $("#climate__get__id").hide();
        $("#add").show();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });

    $("#franchise").click(function() {
        getData(["Id", "Email", "Title"], "/api/franchises", "FastFoodFranchises");

        $("#add").hide();
        $("#climate__get").hide();
        $("#climate__get__id").hide();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });

    $("#application").click(function() {
        getData(["Id", "Name", "Surname", "City"], "/api/applications", "ShopApplications");
    
        $("#add").hide();
        $("#climate__get").hide();
        $("#climate__get__id").hide();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });

    $("#shop").click(function() {
        getData(["Id", "City", "Street", "BuildingNumber", "FastFoodFranchise"], "/api/shops", "FranchiseShops", true);
    
        $("#add").hide();
        $("#climate__get").show();
        $("#climate__get__id").show();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });

    $("#device").click(function() {
        getData(["Id", "DeviceNumber", "IsOnline", "FranchiseShop"], "/api/devices", "ClimateDevices", true);
    
        $("#add").hide();
        $("#climate__get").show();
        $("#climate__get__id").show();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });

    $("#climate").click(function() {
        getData(["Id", "Temperature", "Huumidity", "ClimateDevice"], "/api/climates", "ClimateStates", true);
    
        $("#add").hide();
        $("#climate__get").hide();
        $("#climate__get__id").hide();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });

    $("#tech_message").click(function() {
        getData(["Id", "State", "Title", "ShopAdmin"], "/api/messages", "TechMessages", true);
    
        $("#add").hide();
        $("#climate__get").hide();
        $("#climate__get__id").hide();
        $(".popup").hide();
        $(".database__table").css("visibility", "visible");
    });
});