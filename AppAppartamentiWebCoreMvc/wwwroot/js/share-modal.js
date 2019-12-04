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



