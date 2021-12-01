$(document).ready(function() {
    $("#export").click(function() {
        $.ajax({
            type: "POST",
            headers: {     
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: "/api/export",
            data: JSON.stringify({email: localStorage["email"]}),
            success: function (data, textStatus, xhr) {
                var url = "/dataFiles/export_" + localStorage["id"] + ".xlsx";
                window.location.replace(url);
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    });

    // TODO
    $("#import").click(function() {
        /*$.ajax({
            type: "POST",
            headers: {     
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: "/api/import",
            data: "",
            success: function (data, textStatus, xhr) {
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });*/

        //console.log(tableName);
    });

    $("#copy").click(function() {
        if (tableName.length > 0) {
            var url = "/api/copy";
            var fileUrl;
            var formData = new FormData();
            formData.append("email", localStorage["email"]);

            if (tableName == "FastFoodFranchises") {
                url += "/franchises";

                $("tbody tr").each(function(index, tr) {
                    var tds = $(tr).find('td');
                    if (tds.length > 1) {
                        formData.append("franchise_ids", parseInt(tds[0].textContent, 10));
                    }
                });

                formData.append("tables", tableName);
                formData.append("tables", "FranchiseImages");
                formData.append("tables", "FranchiseContactInfos");
                formData.append("tables", "FranchiseShops");
                formData.append("tables", "ShopApplications");

                fileUrl = "/dataFiles/copy_franchises_" + localStorage["id"] + ".xlsx";
            } else if (tableName == "FranchiseShops") {
                url += "/shops";

                $("tbody tr").each(function(index, tr) {
                    var tds = $(tr).find('td');
                    if (tds.length > 1) {
                        formData.append("shop_ids", parseInt(tds[0].textContent, 10));
                    }
                });
                
                formData.append("tables", tableName);
                formData.append("tables", "ClimateDevices");

                fileUrl = "/dataFiles/copy_shops_" + localStorage["id"] + ".xlsx";
            } else if (tableName == "ClimateDevices") {
                url += "/devices";

                $("tbody tr").each(function(index, tr) {
                    var tds = $(tr).find('td');
                    if (tds.length > 1) {
                        formData.append("device_ids", parseInt(tds[0].textContent, 10));
                    }
                });

                formData.append("tables", tableName);
                formData.append("tables", "ClimateStates");
                formData.append("tables", "TechMessages");

                fileUrl = "/dataFiles/copy_devices_" + localStorage["id"] + ".xlsx";
            } else {
                formData.append("table", tableName);

                fileUrl = "/dataFiles/copy_" + tableName + "_" + localStorage["id"] + ".xlsx";
            }

            $.ajax({
                type: "POST",
                contentType: false,
                processData: false,
                headers: {
                    "Authorization": "Bearer " + localStorage["token"]
                },
                url: url,
                data: formData,
                async: true,
                success: function (data, textStatus, xhr) {
                    window.location.replace(fileUrl);
                },
                error: function (xhr, textStatus, errorThrown) {  
                    console.log(xhr.status);
                    console.log(textStatus);
                    console.log(errorThrown);
                } 
            });
        }
    });

    // TODO
    $("#add").click(function() {
        $.ajax({
            type: "POST",
            headers: {     
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: "/api/",
            data: "",
            success: function (data, textStatus, xhr) {
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    });

    // TODO
    $("#climate__get").click(function() {
        $.ajax({
            type: "POST",
            headers: {     
                "Content-Type": "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: "/api/",
            data: "",
            success: function (data, textStatus, xhr) {
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    });
});