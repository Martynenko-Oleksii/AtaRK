$(".application__form").submit(function (e) { 
    e.preventDefault();
    
    var franchiseEmail = localStorage["franchise_" + getUrlParameter("id")];
    var name = $("#name").val();
    var surname = $("#surname").val();
    var phone = $("#phone").val();
    var email = $("#email").val();
    var city = $("#city").val();
    var message = $("#message").val();

    var application_data = {
        name: name,
        surname: surname,
        contactPhone: phone,
        contactEmail: email,
        city: city,
        message: message,
        fastFoodFranchise: {
            email: franchiseEmail
        }
    };

    $.ajax({
        type: "POST",
        headers: {
            "Content-Type": "application/json; charset=utf-8"  
        },
        url: "/api/applications/send",
        data: JSON.stringify(application_data),
        async: false,
        success: function (data, textStatus, xhr) {
            if (xhr.status == 200) {
                $(".popup").show();

                setTimeout(() => {
                    $(".popup").hide();
                    document.location.reload();
                }, 2000);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log(xhr.status);
            console.log(textStatus);
            console.log(errorThrown);
        } 
    });
});