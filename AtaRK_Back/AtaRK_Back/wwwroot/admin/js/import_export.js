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

    $("#import").change(function() {
        var formData = new FormData();
        var files = $('#import')[0].files;

        if(files.length > 0 ) {
            formData.append("file", files[0]);
            formData.append("table", tableName);

            $.ajax({
                url: "/api/import",
                type: "POST",
                data: formData,
                contentType: false,
                headers: {
                    "Authorization": "Bearer " + localStorage["token"]
                },
                processData: false,
                success: function (data, textStatus, xhr) {
                    console.log("Ok!");
                },
                error: function (xhr, textStatus, errorThrown) {  
                    console.log(xhr.status);
                    console.log(textStatus);
                    console.log(errorThrown);
                }
            });
        }
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

    $("#climate__get").click(function() {
        var id = $("#climate__get__id").val();
        var fileUrl;
        var formData = new FormData();
        if (tableName == "FranchiseShops") {
            console.log(tableName);
            formData.append("object_name", "Shop");
            fileUrl = "/dataFiles/Shop_" + id + ".xlsx";
        } else if (tableName == "ClimateDevices") {
            formData.append("object_name", "Device");
            fileUrl = "/dataFiles/Device_" + id + ".xlsx";
        }
        
        $.ajax({
            type: "POST",
            contentType: false,
            processData: false,
            headers: {
                "Authorization": "Bearer " + localStorage["token"]
            },
            url: "/api/export/climate_statistic/" + id,
            data: formData,
            success: function (data, textStatus, xhr) {
                window.location.replace(fileUrl);
            },
            error: function (xhr, textStatus, errorThrown) {  
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    });
});