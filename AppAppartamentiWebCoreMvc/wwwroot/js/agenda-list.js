$(document).ready(function () {
    var calendarEl2 = document.getElementById('calendar2');

    var calendar2 = new FullCalendar.Calendar(calendarEl2, {
        plugins: ['dayGrid', 'timeGrid', 'interaction'],
        selectable: true,
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
            let currentUrl = ListAppointmentUrl + "?SelectedDate=" + info.dateStr;
            LoadAppointmentList(currentUrl);
        }
    });

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    let urlToday = ListAppointmentUrl + "?SelectedDate=" + (yyyy + "-" + mm + "-" + dd);
    LoadAppointmentList(urlToday);
    
    calendar2.render();
});

//Caricamento lista appuntamenti
function LoadAppointmentList(Url) {
    $("#ListAppointment").empty();
    $("#agenda-spinner-loading").show();
    $('#ListAppointment').load(Url, function () {
        $("#agenda-spinner-loading").hide();
    });
}

function LoadAppointmentDetail(IdAppuntamento) {
    let url = DetailAppointmentUrl + "?SelectedAppointment=" + IdAppuntamento;

    $("#agenda-detail-spinner-loading").show();
    $("#AppointmentDetail").empty();
    $("#appointmentDetailModal").modal("show");

    $("#AppointmentDetail").load(url, function () {
        $("#agenda-detail-spinner-loading").hide();
    });
}

function AcceptAppointment(IdAppuntamento) {
    $.ajax({
        type: "POST",
        url: "/Agenda/Accept",
        data: { Id: IdAppuntamento},
        dataType: "json",
        cache: false,
        success: function (result, status, xhr) {
            LoadAppointmentDetail(IdAppuntamento);
        },
        error: function (xhr, status, error) {
            TrapError("Error during SendMessage");
        }
    });
}

function DeleteAppointment(IdAppuntamento) {
    $.ajax({
        type: "POST",
        url: "/Agenda/Accept",
        data: { Id: IdAppuntamento },
        dataType: "json",
        cache: false,
        success: function (result, status, xhr) {
            $("#appointmentDetailModal").modal("hide");
        },
        error: function (xhr, status, error) {
            TrapError("Error during SendMessage");
        }
    });
}

function setMap() {
    var coordinates = $("#googleMap").data("latlng");
    var splittedCoordinates = coordinates.split(";");
    var lat = splittedCoordinates[0];
    var lng = splittedCoordinates[1];
    var mapProp = {
        zoom: 12,
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




