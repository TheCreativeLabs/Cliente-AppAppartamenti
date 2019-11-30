// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(document).ready(function () {

    $('#divFilterModal').load($("#divFilterModal").data("url"));

    //sul focus mostro i filtri aggiuntivi
    let searchbarInput = document.getElementById("searchbarInput");
    searchbarInput.addEventListener("keyup", EnableSearch);

    //This button will increment the value
    $('[data-quantity="plus"]').click(function (e) {
        //Stop acting like a button
        e.preventDefault();

        //Get the field name
        fieldName = $(this).attr('data-field');
        //Get its current value

        var currentVal = parseInt($('input[name=' + fieldName + ']').val());
        // If is not undefined

        if (!isNaN(currentVal)) {
            //Increment
            $('input[name=' + fieldName + ']').val(currentVal + 1);
        } else {
            //Otherwise put a 0 there

            $('input[name=' + fieldName + ']').val(0);
        }
    });

    //This button will decrement the value till
    $('[data-quantity="minus"]').click(function (e) {

        //Stop acting like a button
        e.preventDefault();

        //Get the field name
        fieldName = $(this).attr('data-field');

        //Get its current value
        var currentVal = parseInt($('input[name=' + fieldName + ']').val());

        //If it isn't undefined or its greater than 0
        if (!isNaN(currentVal) && currentVal > 0) {

            //Decrement one
            $('input[name=' + fieldName + ']').val(currentVal - 1);
        } else {

            //Otherwise put a 0 there
            $('input[name=' + fieldName + ']').val(0);
        }
    });

    $("#btnFacebookLogin").click(function (e) {
        $.ajax({
            type: "POST",
            url: "/Login/GetFacebookExternalLogin",
            contentType: "application/json; charset=utf-8",
            success: function (result, status, xhr) {
                window.location.href = result;
            },
            error: function (xhr, status, error) {
                alert("error");
            }
        });
    });

    $("#btnGoogleLogin").click(function (e) {
        $.ajax({
            type: "POST",
            url: "/Login/GetGoogleExternalLogin",
            contentType: "application/json; charset=utf-8",
            success: function (result, status, xhr) {
                window.location.href = result;
            },
            error: function (xhr, status, error) {
                alert("error");
            }
        });
    });

    //TODO:capire se l'utente è loggato.
    GetUserInfo();
});

function GetUserInfo() {
    //GetUserInfoAsync

    $.ajax({
        type: "POST",
        url: "/Account/GetUserInfo",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            var userInfo = JSON.parse(result);
            if (userInfo.FotoProfilo != undefined) {
                document.getElementById("profileImage").src = "data:image/png;base64," + userInfo.FotoProfilo;
            }
            else {
                document.getElementById("profileImage").src = userInfo.PhotoUrl;
            }

            $("#lblNomeCognome").text(userInfo.Nome + " " + userInfo.Cognome);
            $("#lblEmail").text(userInfo.Email);

        },
        error: function (xhr, status, error) {
            
        }
    });
}

function EnableSearch() {
    if (document.getElementById("searchbarInput").value == "") {
        $("#buttonSearch").addClass("disabled");
    } else {
        $("#buttonSearch").removeClass("disabled");
       var nomeComune = $("#searchbarInput").val();

        $.ajax({
            type: "POST",
            url: "/Home/ListaComuni",
            data: { NomeComune:nomeComune},
            dataType: "json",
            cache: false,
            success: function (result, status, xhr) {
                var nam = [];
                for (var i = 0; i < result.length; i++) {
                    nam.push(result[i].NomeComune);
                }

                $("#searchbarInput").autocomplete({
                    source: nam
                });
            },
            error: function (xhr, status, error) {
                //console.log("Error during EnableSearch");
            }
        });
    }
}

function NavigateToDetail(Url) {
    window.location.href = Url;
}

const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});

