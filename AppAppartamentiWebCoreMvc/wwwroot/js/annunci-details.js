﻿$(document).ready(function () {
    $('#virtual-assistent').load('/VirtualAssistent/GetAvatar?IsForBuy=true', function () {
    });
    //$("#secondnav").hide();

    //window.onscroll = function () { changeScroll() };

    var idAnnuncio = document.getElementById('id-annuncio').value;

    LoadCalendar(idAnnuncio,false);

    LoadReportModal();

    $(".btn-modal-prenota").click(function(){
        var now = new Date();
        var idAnnuncio = document.getElementById('id-annuncio').value;
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();
        GetAndShowAppuntamentiDisponibili(idAnnuncio, (yyyy) + '-' + (mm) + '-' + (dd));
    });
});


function LoadCalendar(IdAnnuncio) {

    let calendar =  document.getElementById('appointment-caledar');
    let today = new Date();
    let currentMonth = today.getMonth();
    let currentYear = today.getFullYear();

    $.ajax({
        type: "POST",
        url: '/Agenda/GetEnabledDate',
        data: { IdAnnuncio: IdAnnuncio, CurrentMonth: currentMonth, CurrentYear: currentYear },
        dataType: "json",
        cache: false,
        success: function (result, status, xhr) {

            let listDate = GetEnabledDate(result,currentYear,currentMonth);

            var calendar2 = new FullCalendar.Calendar(calendar, {
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
                        GetAndShowAppuntamentiDisponibili(IdAnnuncio, info.dateStr); //+'T00:00:00'
                    } else {
                        // Else part is for past dates: do nothing
                    }
                },
                selectable: true,
                events: listDate,
                showNonCurrentDates: false

            });

            calendar2.render();

            $('#appointmentModal').on('shown.bs.modal', function () {
                calendar2.render();

                $(".fc-next-button").click(function () {
                    let date = calendar2.getDate();
                    let month = date.getMonth();
                    let year = date.getFullYear();
                    $.ajax({
                        type: "POST",
                        url: '/Agenda/GetEnabledDate',
                        data: { IdAnnuncio: IdAnnuncio, CurrentMonth: month, CurrentYear: year},
                        dataType: "json",
                        cache: false,
                        success: function (result, status, xhr) {
                            let listDate = GetEnabledDate(result,year,month);
                            calendar2.addEventSource(listDate);
                            //calendar2.render();
                        },
                        error: function (xhr, status, error) {
                            console.log("error");
                        }
                    });
                });

            });
        },
        error: function (xhr, status, error) {
            console.log("error");
        }
    });

}

function GetEnabledDate(enabledDates,year,month) {
    let enabledDate = [];
    var listDate = [];
    for (var i = 0; i < enabledDates.length; i++) enabledDate.push(new Date(enabledDates[i]).valueOf());

    let numberOfDay = new Date(year, month + 1, 0).getDate();

    for (var numDay = 1; numDay < numberOfDay + 1; numDay++) {
        var d = new Date(year, month, numDay, 0, 1, 0);

        let dyear = d.getFullYear();
        let dmonth = d.getMonth() + 1;
        if (dmonth < 10) {
            dmonth = '0' + dmonth;
        }
        let dday = d.getDate();
        if (dday < 10) {
            dday = '0' + dday;
        }
        
        let isIncluded = enabledDate.includes(d.valueOf());

        if (isIncluded == false) {
            let tets = {
                start: d.getFullYear() + '-' + dmonth + '-' + dday,
                rendering: 'background'
            };

            listDate.push(tets);
        }
    }

    return listDate;

}

function LoadReportModal() {
    let url = '/Annunci/SignalAdReason';

    $("#report-modal").load(url, function () {
    });
}

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
            var innerOlHTML = "";

            for (var k in result) {
                if (!isActiveInserted) {
                    innerHTML += '<div class="carousel-item active" style="height: 500px"><img src="' + result[k] + '" class="w-100 h-100" alt="image" /></div>';
                    innerOlHTML += '<li data-target="#carouselExampleIndicators" data-slide-to="' + k + '" class="active"></li>';

                    isActiveInserted = true;
                }
                else {
                    innerHTML += '<div class="carousel-item" style="max-height: 500px"><img src="' + result[k] + '" class="d-block w-100" alt="image" /></div>';
                    innerOlHTML += '<li data-target="#carouselExampleIndicators" data-slide-to="' + k + '"></li>';
                }
            }

            $('.carousel-inner').html(innerHTML);
            $('.carousel-indicators').html(innerOlHTML);

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

function ReportAd() {
    let idAnnuncio = document.getElementById('id-annuncio').value;
    let idReason = $("#report-reason input[type=radio]:checked").val();
    let message = $("#report-test").val();

    if (idReason != undefined) {
        $.ajax({
            type: "POST",
            url: "/Annunci/ReportAd",
            data: { Id: idAnnuncio, ReportReasonId: idReason, ReportMessage: message },
            dataType: "json",
            cache: false,
            success: function (result, status, xhr) {
                alert("L'annuncio è stato segnalato.")
            },
            error: function (xhr, status, error) {
                TrapError("error "  + error);
            }
        });
    } else {
        alert("Devi selezionare un motivo prima di continuare");
    }
    
}