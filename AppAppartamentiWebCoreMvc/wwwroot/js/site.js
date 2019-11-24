// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    //sul focus mostro i filtri aggiuntivi
    let searchbarInput = document.getElementById("searchbarInput");
    searchbarInput.addEventListener("focus", ShowSearchbarInput);
    searchbarInput.addEventListener("blur", HideSearchbarInput);
    searchbarInput.addEventListener("keyup", EnableSearch);
});

function EnableSearch() {
    if (document.getElementById("searchbarInput").value == "") {
        $("#buttonSearch").addClass("disabled");
    } else {
        $("#buttonSearch").removeClass("disabled");
    }
}

function ShowSearchbarInput() {
    $("#searchBarFilters").show();
}

function HideSearchbarInput() {
    $("#searchBarFilters").hide();
}

function NavigateToDetail(Url) {
    window.location.href = Url;
}
