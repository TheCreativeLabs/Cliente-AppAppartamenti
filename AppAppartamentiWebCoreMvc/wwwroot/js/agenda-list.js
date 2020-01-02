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
            $('#ListAppointment').load(currentUrl, function () {
                isLoading = false;
                HideLoader()
            });
        }
    });

    calendar2.render();
});

function LoadAppointmentDetail(IdAppuntamento) {
    let url = DetailAppointmentUrl + "?SelectedAppointment=" + IdAppuntamento;

    $("#AppointmentDetail").load(url, function () {
    });

    $("#appointmentDetailModal").modal("show");
}

function AcceptAppointment(IdAppuntamento) {
}

function DeleteAppointment(IdAppuntamento) {
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




