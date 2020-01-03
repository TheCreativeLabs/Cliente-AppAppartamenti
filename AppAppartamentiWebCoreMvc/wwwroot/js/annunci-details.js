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

    $('#appointmentModal').on('shown.bs.modal', function () {
        calendar2.render();
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
}

function ToggleTimeSlot(btn) {
    var cliccato = $(btn);

    //deseleziono gli altri
    $('.btn-orario-appuntamento').each(function () {
        //se current != this, tolgo active
        if ($(this) != cliccato) {
            $(this).removeClass('active');
        }
    });
};

function GetAndShowAppuntamentiDisponibili(idAnnuncio, giorno) {
    $("#detail-agenda-spinner-loading").show();
    $("#appointment-available").empty();
    let url = '/Agenda/AppuntamentiDisponibili?IdAnnuncio=' + idAnnuncio + '&Giorno=' + giorno;

    $("#appointment-available").load(url, function () {
        $("#detail-agenda-spinner-loading").hide();
        $('.btn-prenota-appuntamento').show()
    });
}

function PrenotaAppuntamento() {
    var ora = $('.btn-orario-appuntamento.active').text();
    var idAnnuncio = document.getElementById('id-annuncio').value;
    var giorno = $('#giorno-selected').val();
    var ora = ora.split('-')[0];

    $.ajax({
        type: "GET",
        url: '/AnnunciPersonali/PrenotaAppuntamento',
        data: { IdAnnuncio: idAnnuncio, Giorno: giorno, Ora: ora },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            alert('appuntamento done');
        },
        error: function (xhr, status, error) {
            $('#appointmentModal').modal("hide");
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

function OpenModal(IdAnnuncio, IdPersonToChat, PersonToChat) {
    let url = '/Messaggi/GetChatDetail?IdChat=&IdAnnuncio='+IdAnnuncio+'&IdPersonToChat='+IdPersonToChat;

    //mostro il loader e rimuovo il content
    $("#chat-spinner-loading").show();
    $("#messages-list").empty();

    $("#chat-modal-title").text(PersonToChat);
    $("#chat-modal").modal("show");

    ReloadChatContentModal(url, true);

    //refresh della lista ogni 30 sec
    reloadInterval = setInterval(function () {
        ReloadChatContentModal(url, false)
    }, 40000)
}


//Reload del contenuto modal
function ReloadChatContentModal(UrlToLoad, FirstTime) {
    $("#messages-list").load(UrlToLoad, function () {
        $("#chat-spinner-loading").hide();

        if (FirstTime == true || $("#chat-modal-body").scrollTop() == $("#chat-modal-body").height()) {
            $("#chat-modal-body").scrollTop($("#chat-modal-body")[0].scrollHeight);
        }
    });
}


//Invio del messaggio
function SendMessage() {
    let idChat = $("#chat-modal .row").data("idchat");
    let idPerson = $("#chat-modal .row").data("idperson");
    let message = $("#chat-textarea").val();

    let content = '<div class="offset-2 mt-2 mb-2 col-10 bg-primary rounded p-2">';
    content += '<span style="color:white">' + message + '</span>';
    content += '</div>';

    $("#messages-list .row").append(content);
    $("#chat-modal-body").scrollTop($("#chat-modal-body")[0].scrollHeight);
    $("#chat-textarea").val("");

    $.ajax({
        type: "POST",
        url: "/Messaggi/Send",
        data: { IdChat: idChat, IdPersonToChat: idPerson, Message: message },
        dataType: "json",
        cache: false,
        success: function (result, status, xhr) {

        },
        error: function (xhr, status, error) {
            TrapError("Error during SendMessage");
        }
    });
}