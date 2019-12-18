$(document).ready(function () {
    $("#secondnav").hide();

    window.onscroll = function () { changeScroll() };
});

function changeScroll() {
    if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
        $("#secondnav").addClass("d-sm-block ");
        $("#nav").removeClass("shadow-sm");
       
    } else {
        $("#secondnav").removeClass("d-sm-block ");
        $("#nav").addClass("shadow-sm");
    }
}

function setMap() {
    var coordinates = $("#googleMap").data("latlng");
    var splittedCoordinates = coordinates.split(";");
    var lat = splittedCoordinates[0];
    var lng = splittedCoordinates[1];
    var mapProp = {
        zoom: 10,
        mapTypeControl: false,
        streetViewControl: false,
    };
    var LatLng = { lat: parseFloat(lat), lng: parseFloat(lng) };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    map.setCenter(LatLng); 
    var marker = new google.maps.Marker({
        position: LatLng,
        map: map
    });
   
    //if (address != null && address.length > 0) {
    //        var geocoder = new google.maps.Geocoder();
    //        geocoder.geocode({ 'address': address }, function (results, status) {
    //        if (status == 'OK') {
    //            var mapProp = {
    //                zoom: 10,
    //                mapTypeControl: false,
    //                streetViewControl: false,
    //            };
    //            var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    //            var marker = new google.maps.Marker({
    //                map: map,
    //                position: results[0].geometry.location
    //            });
    //            map.setCenter(results[0].geometry.location);
    //        } else {
    //            alert('Geocode was not successful for the following reason: ' + status);
    //        }
    //    });
    //}
}

