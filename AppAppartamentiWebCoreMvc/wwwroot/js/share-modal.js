$(document).ready(function () {
    $("#btnShareFacebook").click(function () {
        OpenUrl($(this).data("url"));
    })

    $("#btnEmail").click(function() {
        OpenUrl($(this).data("url"));
    })

    $("#btnShareWhatsapp").click(function() {
        OpenUrl($(this).data("url"));
    })
});

//Apre l'url in una nuova scheda
function OpenUrl(url) {
    var win = window.open(url, '_blank');
    win.focus();
}

