// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    //sul focus mostro i filtri aggiuntivi
    let searchbarInput = document.getElementById("searchbarInput");
    searchbarInput.addEventListener("focus", ShowSearchbarInput);
    searchbarInput.addEventListener("blur", HideSearchbarInput);
    searchbarInput.addEventListener("keyup", EnableSearch);

    let btnSignIn = document.getElementById("btnSignIn");
    btnSignIn.addEventListener("click", ShowModalSignIn);

    let btnSignUp = document.getElementById("btnSignUp");
    btnSignUp.addEventListener("click", ShowModalSignUp);

    // This button will increment the value
    $('[data-quantity="plus"]').click(function (e) {
        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        fieldName = $(this).attr('data-field');
        // Get its current value
        var currentVal = parseInt($('input[name=' + fieldName + ']').val());
        // If is not undefined
        if (!isNaN(currentVal)) {
            // Increment
            $('input[name=' + fieldName + ']').val(currentVal + 1);
        } else {
            // Otherwise put a 0 there
            $('input[name=' + fieldName + ']').val(0);
        }
    });
    // This button will decrement the value till 0
    $('[data-quantity="minus"]').click(function (e) {
        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        fieldName = $(this).attr('data-field');
        // Get its current value
        var currentVal = parseInt($('input[name=' + fieldName + ']').val());
        // If it isn't undefined or its greater than 0
        if (!isNaN(currentVal) && currentVal > 0) {
            // Decrement one
            $('input[name=' + fieldName + ']').val(currentVal - 1);
        } else {
            // Otherwise put a 0 there
            $('input[name=' + fieldName + ']').val(0);
        }
    });
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

function ShowModalSignIn(){
    $("#modalLogin").modal("show");
    $("#modalSignUp").modal("hide");
}

function ShowModalSignUp(){
    $("#modalLogin").modal("hide");
    $("#modalSignUp").modal("show");
}
