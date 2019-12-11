var numImage = 0;
var numPlanimetria = 0;

$(document).ready(function () {
    $("#smallTitle").hide();
    $("#navbar-create").addClass("mt-5");
    window.onscroll = function () { changeScroll() };

    $("#input-planimetria").change(function () {
        GetPlanimetrie(this, $(this).data("list-name"), $(this).data("input-name"));
    });

    $("#input-immagini").change(function () {
        GetImmagini(this, $(this).data("list-name"), $(this).data("input-name"));
    });

    $("#Indirizzo").blur(function () {
        setMap()
    });

    //sul focus mostro i filtri aggiuntivi
    $("#input-comune").keyup(function () {
        EnableCitySearch(this);
    })

    $(".btn-time-slot").click(function () {
        CreateNewTimeSlot(this);
    })

    $(".btn-toggle-slot").click(function () {
        ToggleSlot(this);
    });

    $(".btn-remove-slot").click(function () {
        RemoveSlot(this);
    })

    $("#btn-virtual-agent").dropdown('toggle');

    $("#create-pills-tab .nav-link").click(function() {
        if (!($("#form-create").valid())) {
            $("#create-pills-tab .active").addClass("nav-error-link");
        } else {
            $("#create-pills-tab .active").removeClass("nav-error-link");
        }
    });
});

function RemoveSlot(button) {
    $(button).parent().remove();
}

function ToggleSlot(button) {
    var target = $(button).data("target");

    if ($(target).css("display") == "none") {
        $(target).show();
        
    } else {
        $(target).hide();
    }
}

function CreateNewTimeSlot(timeslotButton) {
    var target = $(timeslotButton).data("target");
    var currentIndex = $(target).data("index");

    var newElement = $(target).clone();
    newElement.removeAttr("id");
    newElement.children(".btn-remove-slot").show();
    newElement.removeAttr("data-index");
    newElement.appendTo($(timeslotButton).parent().parent());
}

function changeScroll() {
    if (document.body.scrollTop > 70 || document.documentElement.scrollTop > 70) {
        $("#smallTitle").show();
        $("#navbar-create").removeClass("mt-5");

    } else {
        $("#smallTitle").hide();
        $("#navbar-create").addClass("mt-5");
    }
}

async function GetImmagini(input,listname,inputname) {
    const file = input.files[0];

    let toBase64 = new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });

    // wait until the promise returns us a value
    let base64 = await toBase64; 

    let imageBytes = base64.slice(base64.indexOf(",")).substring(1, base64.lenght);

    document.getElementById(listname).innerHTML += '<div class="col-xs-12 col-sm-4 p-0 ad-create-image"><button onclick="RemoveFile(this)" class="btn-remove btn btn-light rounded-circle position-fixed border"><i class="fas fa-trash-alt"></i></button><input style="display:none" name="' +inputname+'['+numImage+']" value="'+imageBytes+'"/><div class="border p-2 m-2"><img id="ad-image" src="' + base64 + '" alt="image" class="d-block w-100 " /></div></div>';

    numImage += 1;
}

async function GetPlanimetrie(input, listname, inputname) {
    const file = input.files[0];

    let toBase64 = new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });

    // wait until the promise returns us a value
    let base64 = await toBase64;

    let imageBytes = base64.slice(base64.indexOf(",")).substring(1, base64.lenght);

    document.getElementById(listname).innerHTML += '<div class="col-xs-12 col-sm-4 p-0 ad-create-image"><button onclick="RemoveFile(this)" class="btn-remove btn btn-light rounded-circle position-fixed border"><i class="fas fa-trash-alt"></i></button><input style="display:none" name="' + inputname + '[' + numPlanimetria + ']" value="' + imageBytes + '"/><div class="border p-2 m-2"><img id="ad-image" src="' + base64 + '" alt="image" class="d-block w-100 " /></div></div>';

    numPlanimetria += 1;
}

function setMap() {
    var comune = $("#input-comune").val();
    var indirizzo = $("#Indirizzo").val();
    
    if ((comune != null && comune.length > 0) && (indirizzo != null && indirizzo.length > 0)) {
        var address = indirizzo + "," + comune;
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == 'OK') {
                var mapProp = {
                    zoom: 10,
                    mapTypeControl: false,
                    streetViewControl: false,
                };
                var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });
                map.setCenter(results[0].geometry.location);

                $("#CoordinateGeografiche").val(results[0].geometry.location.lat() + ";" + results[0].geometry.location.lng());

                $("#googleMap").show();
            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        });
    }
}

var comuni = [];

//Abilita il pulsante di ricerca deigli annunci
function EnableCitySearch(searchTextbox) {
    try {
        let nomeComune = $(searchTextbox).val();

        if (listaComuni.length > 0 && nomeComune.length < 3) {
            listaComuni = [];
            $(searchTextbox).autocomplete({
                source: []
            });
        } else if (listaComuni.length == 0 && nomeComune.length >= 3) {
            $("#city-spinner").show();

            $.ajax({
                type: "POST",
                url: "/Home/ListaComuni",
                data: { NomeComune: nomeComune },
                dataType: "json",
                cache: false,
                success: function (result, status, xhr) {
                    comuni = result;

                    var nomiComuni = [];
                    for (var i = 0; i < result.length; i++) {
                        nomiComuni.push(result[i].NomeComune);
                    }

                    $(searchTextbox).autocomplete({
                        source: nomiComuni,
                        select: function (event, ui) {
                            $("#Indirizzo").removeAttr("disabled");
                            var value = ui.item.value;
                            for (var i = 0; i < comuni.length; i++) {
                                if (comuni[i].NomeComune == value) {
                                    $("#CodiceComune").val(comuni[i].CodiceComune);
                                    break;
                                }
                            }
                        }
                    });

                    $("#city-spinner").hide();

                    $(searchTextbox).autocomplete("search");
                },
                error: function (xhr, status, error) {
                    TrapError("Error during EnableSearch");
                }
            });
        }
    }
    catch (ex) {
        TrapError("Error during EnableSearch");
    }
}

function RemoveFile(button) {
    $(button).parent().remove();
}

