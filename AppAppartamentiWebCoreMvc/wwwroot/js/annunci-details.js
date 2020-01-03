$(document).ready(function () {
    $("#secondnav").hide();

    window.onscroll = function () { changeScroll() };


    var calendarEl2 = document.getElementById('appointment-caledar');
    var idAnnuncio = document.getElementById('id-annuncio').value;

    var calendar2 = new FullCalendar.Calendar(calendarEl2, {
        plugins: ['dayGrid', 'timeGrid', 'interaction'],
        locale: selectedLanguage,
        themeSystem: 'bootstrap',
        header: {
            left: 'title',
            center: '',
            right: 'prev,next'
        },
        footer: {
            left: '',
            center: '',
            right: ''
        },
        dateClick: function (info) {
            var now = new Date();
            if ((now.getFullYear()) + '-' + (now.getMonth() + 1) + '-' + (now.getDate()) == info.dateStr || info.date > (now)) {
                // This allows today and future date
                $('#giorno-selected').val(info.dateStr);
                //alert('Clicked on: ' + info.dateStr);
                //$(info.dayEl).addClass('fc-state-highlight');
                $('.fc-today').css('blackground', 'transparent !important'); //not working
                GetAndShowAppuntamentiDisponibili(idAnnuncio, info.dateStr); //+'T00:00:00'
            } else {
                // Else part is for past dates: do nothing
            }
        },
        selectable: true

    });

    calendar2.render();



    $(".btn-modal-prenota").click(function(){
        var now = new Date();
        var idAnnuncio = document.getElementById('id-annuncio').value;
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();
        GetAndShowAppuntamentiDisponibili(idAnnuncio, (yyyy) + '-' + (mm) + '-' + (dd));
    });

    $(".btn-prenota-appuntamento").click(function () {
        //todo prenotare appuntamento
        PrenotaAppuntamento();
    });

    
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

function GetAndShowAppuntamentiDisponibili(idAnnuncio, giorno) {
    var containerOrari = document.getElementById('appointment-available');
    $('.btn-prenota-appuntamento').hide();
    containerOrari.innerHTML = '';
    $.when(GetAppuntamentiDisponibiliAjax(idAnnuncio, giorno)).done(function (orari) {
        //var containerOrari = $('#appointment-available');
        
        if (orari != null && orari.length > 0) {

            orari.forEach(function (orario, index) {
                containerOrari.innerHTML += '<label class="btn btn-light btn-orario-appuntamento" "aria-pressed=\"true\""  style="margin:0.5rem">' +
                    '<input type="checkbox">' + orario +
                    '</label>';
            });

            $('.btn-prenota-appuntamento').show();

        } else {
            containerOrari.innerHTML += '<div class="col-12 alert bg-light"><label class="h6">Non ci sono appuntamenti disponibili per il giorno selezionato.</label></div>';
        }

        $('.btn-orario-appuntamento').click(function () {
            var cliccato = $(this);
            //deseleziono gli altri
            $('.btn-orario-appuntamento').each(function () {
                //se current != this, tolgo active
                if ($(this) != cliccato) {
                    $(this).removeClass('active');
                }
            });
        });
    });

}

function GetAppuntamentiDisponibiliAjax(idAnnuncio, giorno) {
    return $.ajax({
        type: "GET",
        url: '/AnnunciPersonali/AppuntamentiDisponibili',
        data: { IdAnnuncio: idAnnuncio, Giorno: giorno },
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    });

}

function PrenotaAppuntamento() {
    var ora = $('.btn-orario-appuntamento.active').text();
    var idAnnuncio = document.getElementById('id-annuncio').value;
    var giorno = $('#giorno-selected').val();
    var ora = ora.split('-')[0];
    $.when(PrenotaAppuntamentoAjax(idAnnuncio, giorno, ora)).done(function (result) {
        //popup success, chiudere modale
        alert('appuntamento done');
    });
}

function PrenotaAppuntamentoAjax(idAnnuncio, giorno, ora) {
    return $.ajax({
        type: "GET",
        url: '/AnnunciPersonali/PrenotaAppuntamento',
        data: { IdAnnuncio: idAnnuncio, Giorno: giorno, Ora: ora },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            //FIXME GESTIRE SUCCESS
            //alert('Appuntamento success');
            var i = status;
            var b = error;
            var c = xhr;
        },
        error: function (xhr, status, error) {
            //alert('Appuntamento fail');
            var i = status;
            var b = error;
            var c = xhr;
            $('#appointmentModal').modal('toggle');
        }
    });

}



function GetImmaginiAnnuncioAjax(idAnnuncio) {
    return $.ajax({
        type: "GET",
        url: '/Annunci/ImmaginiAnnuncio',
        data: { IdAnnuncio: idAnnuncio },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            //alert('Caricamento immagini  success');
            var isActiveInserted = false;

            var innerHTML = "";

            for(var k in result)
            {
                if (!isActiveInserted) {
                    innerHTML += '<div class="carousel-item active" style="height: 500px"><img src="' + result[k]+ '" class="w-100 h-100" alt="image" /></div>';

                    isActiveInserted = true;
                }
                else {
                    innerHTML += '<div class="carousel-item" style="max-height: 500px"><img src="' + result[k] + '" class="d-block w-100" alt="image" /></div>';
                }
            }

            $('.carousel-inner').html(innerHTML);

        },
        error: function (xhr, status, error) {
            //alert('Caricamento immagini fail');  
            
        }
    });

}

