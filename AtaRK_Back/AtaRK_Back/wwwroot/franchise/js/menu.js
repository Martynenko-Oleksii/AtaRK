$("#exit").click(function (e) { 
    e.preventDefault()
    
    localStorage.removeItem("fran_date");
    localStorage.removeItem("fran_id");
    localStorage.removeItem("fran_email");
    localStorage.removeItem("fran_token");

    document.location.reload();
});

$("#delete").click(function (e) { 
    e.preventDefault();

    $.ajax({
        type: "GET",
        headers: {          
            Accept: "application/json; charset=utf-8",
            "Authorization": "Bearer " + localStorage["fran_token"]
        },
        url: "/api/franchises/delete/" + localStorage["fran_email"],
        success: function (data, textStatus, xhr) {
            localStorage.removeItem("fran_date");
            localStorage.removeItem("fran_id");
            localStorage.removeItem("fran_email");
            localStorage.removeItem("fran_token");
        
            document.location.reload();
        },
        error: function (xhr, textStatus, errorThrown) {  
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});