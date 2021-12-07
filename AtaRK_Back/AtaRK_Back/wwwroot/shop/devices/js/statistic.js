$(function () {
    $(".devices_list").on("click", "li", function() {

        var deviceNumber = local_device + $("#number_" + $(this).attr("id")).text();
        var title_temp = local_temperature;
        var title_hum = local_humidity;
        var dataPoints_temp = [];
        var dataPoints_hum = [];
    
        $.ajax({
            type: "GET",
            headers: {          
                Accept: "application/json; charset=utf-8",
                "Authorization": "Bearer " + localStorage["shop_token"]
            },
            url: "/api/devices/climate/" + $(this).attr("id"),
            success: function (data, textStatus, xhr) {
                console.log(data);
                $(".popup").show();
    
                for (i = 0; i < data.length; i++) {
                    dataPoints_temp.push({ y: data[i]["temperature"] });
                    dataPoints_hum.push({ y: data[i]["huumidity"] });
                }

                /*
                var y = 20;
    
                var limit = 100;
    
                for ( var i = 0; i < limit; i++ ) {
                    y += Math.round( 10 + Math.random() * (-10 -10));	
                    dataPoints_temp.push({ y: y });
                    dataPoints_hum.push({ y: y });
                }
                */
                
                var options_temp = {
                    animationEnabled: true,
                    zoomEnabled: true,
                    title:{
                        text: title_temp
                    }, 
                    subtitles:[{
                        text: deviceNumber
                    }],
                    data: [{
                        type: "line",
                        dataPoints: dataPoints_temp
                    }]
                };
                $("#temp").CanvasJSChart(options_temp);
    
                var options_hum = {
                    animationEnabled: true,
                    zoomEnabled: true,
                    title:{
                        text: title_hum
                    }, 
                    subtitles:[{
                        text: deviceNumber
                    }],
                    data: [{
                        type: "line",
                        dataPoints: dataPoints_hum
                    }]
                };
                $("#hum").CanvasJSChart(options_hum);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.status);
                console.log(textStatus);
                console.log(errorThrown);
            } 
        });
    });
    
    $("#close").click(function (e) { 
        e.preventDefault();
    
        $(".popup").hide();
    });
});
