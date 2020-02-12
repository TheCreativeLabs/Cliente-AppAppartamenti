var listPage = 1;
var isLoading = false;

$(document).ready(function () {
   
    ReloadList();

    $("#btn-show-map").click(function () {
        $(this).hide();
        $("#btn-hide-map").show();
        setHeight();
    });

    $("#btn-hide-map").click(function () {
        $(this).hide();
        $("#map-container").hide();
        $("#main-container").addClass("container");
        $("#main-container").removeClass("container-fluid");
        $("#container-annunci").removeClass();
        $("#container-annunci").addClass("col-xs-12 col-sm-10 offset-sm-1 col-md-8 offset-md-2");
        $("#container-annunci").height('auto');
        $("#btn-show-map").show();
    });

    //$('#loadFromMainFrame').click(function (e) {
    //    e.preventdefault;

    //    $("#listLoader").show();
    //    if (CurrentListPage == undefined || CurrentListPage == null || CurrentListPage == "") {
    //        CurrentListPage = 1;
    //    }
    //    UrlRefresh = UrlRefresh.replace(CurrentListPage.toString(), (new Number(CurrentListPage) + 1).toString());
    //    //listPage = listPage + 1;
    //    ReloadList();
    //});

    $('#select-order').change(function () {
        ReloadList();
    });

    var previousScrollPoint = 0;

    $(window).scroll(function () {
        //raggiungimentoFondo = numero che identifica altezza a 200 dalla fine della pagina: voglio che se sto scendendo
        var raggiungimentoFondo = $(document).height() - 200; 
        var posizioneAttuale = $(window).scrollTop() + $(window).height();
        //discesaAttraversamento = true se sto passando per il punto "raggiungimentoFondo" scorrendo dall'alto al basso; 
        //discesaAttraversamento = false se sto passando per il punto "raggiungimentoFondo" scorrendo dal basso all'alto
        var discesaAttraversamento = previousScrollPoint < raggiungimentoFondo;

        //se sono oltre il punto di "raggiungimentoFondo" per la prima volta e stavo andando dall'alto al basso, allora chiamo API
        if (posizioneAttuale > raggiungimentoFondo && discesaAttraversamento) { 
            if (!isLoading) {  //se non sto già caricando, chiedo una nuova pagina alle API
                //alert("near bottom! ricarico");
                isLoading = true;

                UrlRefresh = UrlRefresh.replace(CurrentListPage.toString(), (new Number(CurrentListPage) + 1).toString());
                //listPage = listPage + 1;
                ReloadList();
            }
        }
        previousScrollPoint = posizioneAttuale;
    });

    
});


function setHeight() {
    $("#map-container").show();
    $("#main-container").removeClass("container");
    $("#main-container").addClass("container-fluid");
    $("#container-annunci").removeClass();
    $("#container-annunci").addClass("col-xs-12 col-sm-6 scroll-y");
    $("#container-annunci").height($(window).height() - 100);
}

function setMap() {
    var mapProp = {
        zoom: 10,
        mapTypeControl: false,
        streetViewControl: false,
    };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

    $(".card-annuncio").each(function () {
        let coordinates = $(this).data("coordinate");
        let idAd = $(this).data("id");
        var splittedCoordinates = coordinates.split(";");
        var lat = splittedCoordinates[0];
        var lng = splittedCoordinates[1];
        var LatLng = { lat: parseFloat(lat), lng: parseFloat(lng) };
        map.setCenter(LatLng);
        var marker = new google.maps.Marker({
            position: LatLng,
            map: map
        });

        google.maps.event.addListener(marker, 'click', function () {
            scrollToDiv(idAd);
        });
    });

    $("#map-container").height($(window).height() - 100);
}

function scrollToDiv(Id) {
    $(".card-annuncio").each(function (index) {
        $(this).removeClass("card-ad-active");
        if (Id == $(this).data("id")) {
            $(this).addClass("card-ad-active");
            let height = $(this).height();
            //let top = $(".card-annuncio[data-id='" + IdAnnuncio + "']").parent().offset().top;

            $("#container-annunci").animate({
                scrollTop: height * index
            }, 500);
        }
    })
}

function ReloadList() {
    let url = UrlRefresh;

    if ($("#select-order").lenght > 0) {
        let order = $("#select-order").val();
        if (order != "") {
            url = url + "&Order=" + $("#select-order").val();
        }
    }

    $('#annunci').load(url, function () {
        setMap();
    });
}



