var numImage = 0;
var numPlanimetria = 0;
var numFasceLun = 0;

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

    $('.select-orario').change(function () {
        ComponiFasceOrarie(this);
    });
});

function ComponiFasceOrarie(input) {
    var superParent = $(input).parent().parent();

    //$(".div-fasce-by-day").each(function (i) {
    var idValoreFasceGiornaliere = $(superParent).data("for");
    var fasce = null;
    if ($(superParent).children(".input-time-slot").length > 0) {
        fasce = "";
        $(superParent).children(".input-time-slot").each(
            function (j) {
                if ($(this).children("#oreFasciaOrariaFrom").val() != null && $(this).children("#oreFasciaOrariaFrom").val() != "" &&
                    $(this).children("#minutiFasciaOrariaFrom").val() != null && $(this).children("#minutiFasciaOrariaFrom").val() != "" &&
                    $(this).children("#oreFasciaOrariaTo").val() != null && $(this).children("#oreFasciaOrariaTo").val() != "" &&
                    $(this).children("#minutiFasciaOrariaTo").val() != null && $(this).children("#minutiFasciaOrariaTo").val() != "" ) {
                    fasce += $(this).children("#oreFasciaOrariaFrom").val();
                    fasce += ":";
                    fasce += $(this).children("#minutiFasciaOrariaFrom").val();
                    fasce += "-";
                    fasce += $(this).children("#oreFasciaOrariaTo").val();
                    fasce += ":";
                    fasce += $(this).children("#minutiFasciaOrariaTo").val();
                    fasce += ";";
                }
            }
        );
    }
    $(idValoreFasceGiornaliere).val(fasce);
        
    //});


}



function RemoveSlot(button) {
    var superParent = $(button).parent().parent();
    var fasciaSorella = $(superParent).children(".input-time-slot").first();
    if (fasciaSorella != null) { //devo ricalcolare la stringa delle fasce, perchè va rimossa quella che sto eliminando
        var input = $(fasciaSorella).children(".select-orario");
    }
    $(button).parent().remove();
    ComponiFasceOrarie($(input));
}

function ToggleSlot(button) {
    var target = $(button).data("target");
    var idItems = $(target).data("id-items");

    if ($(target).css("display") == "none") {
        $(target).show();
        
    } else {
        var inputPrimo;
        //$(target).children("#slot-time-monday").each(function (i) {
        //$(target).children(idItems).each(function (i) {
        $(target).children(".input-time-slot").each(function (i) {
            if ($(this).children(".btn-remove-slot").length > 0 && $(this).children(".btn-remove-slot").css("display") != "none") {
                $(this).remove();
            } else {
                $(this).children(".select-orario").each(function (j) {
                    $(this).val(null);
                    inputPrimo = $(this);
                });
                
            }
        });
        if (inputPrimo != null) {
            ComponiFasceOrarie(inputPrimo);
        }
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
    newElement.children(".select-orario").change(function () {
        ComponiFasceOrarie(this);
    });
    newElement.children(".select-orario").val(null);
    newElement.appendTo($(timeslotButton).parent());
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

    document.getElementById(listname).innerHTML += '<div class="col-xs-12 col-sm-4 p-0 ad-create-image appartamento-image"><button onclick="RemoveFile(this,\'' + listname + '\',' + '\'appartamento-image\'' + ',\'' + inputname+'\')" class="btn-remove btn btn-light rounded-circle position-absolute border"><i class="fas fa-trash-alt"></i></button><input style="display:none"  value="'+imageBytes+'"/><div class="border p-2 m-2"><img id="ad-image" src="' + base64 + '" alt="image" class="d-block w-100 " /></div></div>';
    refreshNames(listname, 'appartamento-image', inputname);

    //name = "' +inputname+'['+numImage+']"
    //numImage += 1;
}

function refreshNames(listname, childrensName, inputname) {
    var idListName = "#" + listname;
    var classChildrens = "." + childrensName;
    $(idListName).find(classChildrens).each(function (i) {
        $(this).find("input").attr('name', inputname+'[' + i + ']');
        //$(this).find("label").attr('input', 'SubModels[' + i + '].SomeProperty');
    });
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

    document.getElementById(listname).innerHTML += '<div class="col-xs-12 col-sm-4 p-0 ad-create-image planimetria-image"><button onclick="RemoveFile(this,\'' + listname + '\',' + '\'planimetria-image\'' + ',\'' + inputname +'\')" class="btn-remove btn btn-light rounded-circle position-absolute border"><i class="fas fa-trash-alt"></i></button><input style="display:none" name="' + inputname + '[' + numPlanimetria + ']" value="' + imageBytes + '"/><div class="border p-2 m-2"><img id="ad-image" src="' + base64 + '" alt="image" class="d-block w-100 " /></div></div>';
    refreshNames(listname, 'planimetria-image', inputname);

    //numPlanimetria += 1;
}

function setMap() {

    var coordinates = $("#googleMap").data("latlng");
    if (coordinates != null && coordinates != "") { //significa che sono appena entrato in MODIFICA, setto la mappa con lat e lng lette dal db
        var splittedCoordinates = coordinates.split(";");
        var lat = splittedCoordinates[0];
        var lng = splittedCoordinates[1];
        var mapProp = {
            zoom: 10,
            mapTypeControl: false,
            streetViewControl: false,
        };
        var LatLng = { lat: parseFloat(lat), lng: parseFloat(lng) };
        var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
        map.setCenter(LatLng);
        var marker = new google.maps.Marker({
            position: LatLng,
            map: map
        });
         //dopo aver posizionato la mappa, tolgo le coordinate prese dal db: così se viene cambiato l'indirizzo si entra nell'else e si posiziona sulle nuove coordinate
        $("#googleMap").data("latlng", null);
        $("#googleMap").show();
    }
    else {//quando viene inserito/modificato l'indirizzo, entro nell'else e chiedo a google lat e lng sapendo l'indirizzo
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

function RemoveFile(button, listname, childrensName, inputname) {
    $(button).parent().remove();
    refreshNames(listname, childrensName, inputname);
}

