$(document).ready(function () {
    $("#navPrincipale").hide();

    HideFiltriAggiuntivi();

    //sul focus mostro i filtri aggiuntivi
    let txtRicerca = document.getElementById("txtRicerca");
    txtRicerca.addEventListener("focus", ShowFiltriAggiuntivi);
    txtRicerca.addEventListener("blur", HideFiltriAggiuntivi);

    let btnAccedi = document.getElementById("btnAccedi");
    btnAccedi.addEventListener("click", ShowModalLogin);
});

function ShowFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").show();
}

function HideFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").hide();
}

function ShowModalLogin() {
    $("#modalLogin").modal("show");
}
