// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#nav").addClass("navbar-transparent");

    window.onscroll = function () { changeScroll() };

    HideFiltriAggiuntivi();

    //sul focus mostro i filtri aggiuntivi
    let txtRicerca = document.getElementById("txtRicerca");
    txtRicerca.addEventListener("focus", ShowFiltriAggiuntivi);
    txtRicerca.addEventListener("blur", HideFiltriAggiuntivi);

    let btnAccedi = document.getElementById("btnAccedi");
    btnAccedi.addEventListener("click", ShowModalLogin);
});

function changeScroll() {
    if (document.body.scrollTop > 170 || document.documentElement.scrollTop > 170) {
        $("#nav").removeClass("navbar-transparent");

    } else {
        $("#nav").addClass("navbar-transparent");
    }
}

function ShowFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").show();
}

function HideFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").hide();
}

function ShowModalLogin() {
    $("#modalLogin").modal("show");
}
