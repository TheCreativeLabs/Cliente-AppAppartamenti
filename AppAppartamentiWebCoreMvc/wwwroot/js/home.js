$(document).ready(function () {
    $("#nav").addClass("navbar-transparent");
    $("#navSearchBar").hide();

    window.onscroll = function () { changeScroll() };

    HideFiltriAggiuntivi();

    //sul focus mostro i filtri aggiuntivi
    let txtRicerca = document.getElementById("txtRicerca");
    txtRicerca.addEventListener("focus", ShowFiltriAggiuntivi);
    txtRicerca.addEventListener("blur", HideFiltriAggiuntivi);

});

function changeScroll() {
    if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 80) {
        $("#nav").removeClass("navbar-transparent");
        $("#navSearchBar").show();
    } else {
        $("#nav").addClass("navbar-transparent");
        $("#navSearchBar").hide();
    }
}

function ShowFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").show();
}

function HideFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").hide();
}
