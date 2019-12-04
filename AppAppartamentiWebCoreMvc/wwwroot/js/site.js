//Lista dei comuni
var listaComuni = [];

$(document).ready(function () {
    //Se l'utente è loggato carico le informazioni
    if (loggedUser != null && loggedUser.length > 0) {
        GetUserInfo();

        //Carico la modale dei filtri 
        LoadFilterModal($("#divFilterModal"));

        //sul focus mostro i filtri aggiuntivi
        $("#searchbarInput").keyup(function () {
            EnableSearch(this);
        })
    }

    //Login con Facebook
    $("#btnFacebookLogin").click(function (e) {
        FacebookLogin();
    });

    //Login con Google
    $("#btnGoogleLogin").click(function (e) {
        GoogleLogin();
    });
});

//Gestisce l'autenticazione con facebook
function FacebookLogin(){
    $.ajax({
        type: "POST",
        url: "/Login/GetFacebookExternalLogin",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            window.location.href = result;
        },
        error: function (xhr, status, error) {
            TrapError("Error during FacebookLogin");
        }
    });
}

//Gestisce l'autenticazione con google
function GoogleLogin(){
    $.ajax({
        type: "POST",
        url: "/Login/GetGoogleExternalLogin",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            window.location.href = result;
        },
        error: function (xhr, status, error) {
           TrapError("Error during GoogleLogin");
        }
    });
}

//Incrementa il valore dei number picker
function ButtonPlusIncrement(e, buttonPlus){
    try{
        //Stop acting like a button
        e.preventDefault();

        //Get the field name
        fieldName = $(buttonPlus).attr('data-field');
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
    }
    catch(ex){
        TrapError("Error during ButtonPlusIncrement" + ex);
    }
};

//Decrementa il valore dei number picker
function ButtonPlusDecrement(e, buttonMinus){
    try{
        //Stop acting like a button
        e.preventDefault();

        //Get the field name
        fieldName = $(buttonMinus).attr('data-field');

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
    }
    catch(ex){
        TrapError("Error during ButtonPlusIncrement" + ex);
    }
};

//Carica la modale dei filtri
function LoadFilterModal(divFilterModal){
    try{
        var filterModalUrl =$(divFilterModal).data("url")
        $(divFilterModal).load(filterModalUrl);
     }
    catch(ex){
        TrapError("Error during ButtonPlusIncrement");
    }
}

//Ottiene le informazioni sull'utente
function GetUserInfo() {
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
            TrapError("Error during GetUserInfo");
        }
    });
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

//Aggiunge l'annuncio ai preferiti
function AddPreferred(btn, url, id) {
    event.stopPropagation();
    alert(id);
    alert(url);
    $.ajax({
        type: "POST",
        url: url,
        data: { Id: id },
        dataType: "json",
        success: function (result, status, xhr) {
            $(btn).addClass("text-primary");
        },
        error: function (xhr, status, error) {
            TrapError("Error during AddPreferred");
        }
    });
}

//Rimuove l'annuncio ai preferiti
function RemovePreferred(btn, url, id) {
    event.stopPropagation();

    $.ajax({
        type: "POST",
        url: url,
        data: { Id: id },
        dataType: "json",
        success: function (result, status, xhr) {
            $(btn).addClass("text-primary");
        },
        error: function (xhr, status, error) {
            TrapError("Error during ButtonPlusIncrement");
        }
    });
}

//Abilita il pulsante di ricerca deigli annunci
function EnableSearch(searchTextbox) {
    try{
        let searchButton = $(".search-button");
        let nomeComune = $(searchTextbox).val();

        if (listaComuni.length > 0 && nomeComune.length < 3) {
            listaComuni = [];
            searchButton.addClass("disabled");
            $(searchTextbox).autocomplete({
                source: []
            });
        } else if (listaComuni.length == 0 && nomeComune.length >= 3) {
            $(".search-spinner").show();

            $.ajax({
                type: "POST",
                url: "/Home/ListaComuni",
                data: { NomeComune: nomeComune },
                dataType: "json",
                cache: false,
                success: function (result, status, xhr) {
                    listaComuni = result;

                    var nomiComuni = [];
                    for (var i = 0; i < result.length; i++) {
                        nomiComuni.push(result[i].NomeComune);
                    }

                    $(searchTextbox).autocomplete({
                        source: nomiComuni,
                        select: function (event, ui) {
                            searchButton.removeClass("disabled");
                            var value = ui.item.value;
                            for (var i = 0; i < listaComuni.length; i++) {
                                if (listaComuni[i].NomeComune == value) {
                                    $("#codiceComune").val(listaComuni[i].CodiceComune);
                                    $(".search-textbox").val(listaComuni[i].NomeComune);

                                    SaveFilter();
                                    break;
                                }
                            }
                        }
                    });

                    $(".search-spinner").hide();

                    $(searchTextbox).autocomplete("search");
                },
                error: function (xhr, status, error) {
                           TrapError("Error during EnableSearch");
                }
            });
        }
    }
    catch(ex){
        TrapError("Error during EnableSearch");
    }
}

//Gestisce gli errori
function TrapError(message) {
    console.log(message);
    $("#errorContainer").show();
}

//Apre l'url in una nuova scheda
function OpenUrl(url) {
    var win = window.open(url, '_blank');
    win.focus();
}