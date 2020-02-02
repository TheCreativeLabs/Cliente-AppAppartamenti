//Lista dei comuni
var listaComuni = [];

$(document).ready(function () {
    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)')
            .exec(window.location.href.replace("#", "?"));
        if (results == null) {
            return 0;
        }
        return results[1] || 0;
    }

    var providerError = $.urlParam('provider_error');

    if (providerError == 1) {
        $("#modal-provider-error").modal("show");
    }

    //Se l'utente è loggato carico le informazioni
    if (loggedUser != null && loggedUser.length > 0) {
        GetUserInfo();

        //Carico la modale dei filtri 
        LoadFilterModal($("#divFilterModal"));

        //sul focus mostro i filtri aggiuntivi
        $("#searchbarInput").keyup(function () {
            EnableSearch(this);
        })

        //Carica gli appuntamenti di oggi
        LoadTodayAppointment();
    }

    //Login con Facebook
    $("#btnFacebookLogin").click(function (e) {
        FacebookLogin(this);
    });

    //Login con Google
    $("#btnGoogleLogin").click(function (e) {
        GoogleLogin(this);
    });

    $("#btn-login").click(function (evt) {
        evt.preventDefault();
        if ($("#form-login").valid()) {
            Login($(this).data("url"));
        }
    });

    $(".dropdown-agenda").click(function (e) {
        e.stopPropagation();
    });

    $(".btn-show-password").click(function () {
        let passwordInput = $(this).parent().parent().children("input");

        if ($(passwordInput).attr("type") == "text") {
            $($(this).children()[0]).removeClass("d-none");
            $($(this).children()[1]).addClass("d-none");
            $(passwordInput).attr("type", "password");
        } else {
            $($(this).children()[0]).addClass("d-none");
            $($(this).children()[1]).removeClass("d-none");
            $(passwordInput).attr("type", "text");
        }
    });



   
});

//Carica la lista degli appuntamenti di oggi
function LoadTodayAppointment() {
    let url = UrlTodayAppointment;

    $('#TodayAppointment').load(url, function () {
        isLoading = false;
        HideLoader()
    });
}

//Gestisce l'autenticazione con facebook
function FacebookLogin(button) {
    $(button).children(".spinner").removeClass("d-none");
    $(button).attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/Login/GetFacebookExternalLogin",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            window.location.href = result;
        },
        error: function (xhr, status, error) {
            $(button).children(".spinner").addClass("d-none");
            $(button).removeAttr("disabled");
            TrapError("Error during FacebookLogin");
        }
    });
}

//Gestisce l'autenticazione con google
function GoogleLogin(button) {
    $(button).children(".spinner").removeClass("d-none");
    $(button).attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/Login/GetGoogleExternalLogin",
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            window.location.href = result;
        },
        error: function (xhr, status, error) {
            $(button).children(".spinner").addClass("d-none");
            $(button).removeAttr("disabled");
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
    window.open(Url, '_blank');
}

function Login(url) {
    $("#spinner-login").removeClass("d-none");
    $("#btn-login").attr("disabled","disabled");

    $.ajax({
        type: "POST",
        url: url,
        data: { Email: $("#email").val(), Password: $("#password").val()},
        dataType: "json",
        cache: false,
        success: function (result, status, xhr) {
            location.reload();
        },
        error: function (xhr, status, error) {
            $("#spinner-login").addClass("d-none");
            $("#btn-login").removeAttr("disabled");
            $("#login-error").removeClass("d-none");
            console.log("Error in Login function: " + error)
        }
    });
}

//Aggiunge l'annuncio ai preferiti
function AddPreferred(btn, id) {
    event.stopPropagation();

    $(btn).addClass("text-primary");
    $(btn).attr("data-preferred", "True");

    $.ajax({
        type: "POST",
        url: '/AnnunciPreferiti/Add',
        data: { Id: id },
        dataType: "json",
        success: function (result, status, xhr) {
            
        },
        error: function (xhr, status, error) {
            TrapError("Error during AddPreferred");
        }
    });
}

function AddOrRemovePreferred(btn, id) {
    event.stopPropagation();

    if ($(btn).data("preferred") == "True") {
        RemovePreferred(btn, id);
    } else {
        AddPreferred(btn, id);
    }
}

//Rimuove l'annuncio ai preferiti
function RemovePreferred(btn, id) {
    event.stopPropagation();

    if (window.location.href.indexOf("/AnnunciPreferiti") > -1) {
        $(btn).closest("div.card-annuncio").remove();
    } else {
        $(btn).removeClass("text-primary");
        $(btn).attr("data-preferred", "False");
    }

    $.ajax({
        type: "POST",
        url: '/AnnunciPreferiti/Remove',
        data: { Id: id },
        dataType: "json",
        success: function (result, status, xhr) {
        },
        error: function (xhr, status, error) {
            TrapError("Error during ButtonPlusIncrement");
        }
    });
}

//Abilita il pulsante di ricerca dei comuni
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
                        nomiComuni.push(result[i].NomeComune + " (" + result[i].NomeProvincia + ")");
                    }

                    $(searchTextbox).autocomplete({
                        source: nomiComuni,
                        select: function (event, ui) {
                            searchButton.removeClass("disabled");
                            var value = ui.item.value;

                            if (value != null) {
                                value = value.split(" (")[0];
                                console.log("selected-place: " + value);
                            }
                           
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