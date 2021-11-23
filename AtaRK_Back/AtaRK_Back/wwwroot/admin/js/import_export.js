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
});