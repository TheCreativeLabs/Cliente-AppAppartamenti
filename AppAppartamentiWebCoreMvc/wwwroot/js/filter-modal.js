$(document).ready(function () {
    if (loggedUser != null && loggedUser.length > 0) {
        //Carico i filtri prendendoli dalla sessione
        LoadFilter();

        $("#btnSaveFilter").click(function (e) {
            SaveFilter()
        });

        //This button will increment the value
        $('[data-quantity="plus"]').click(function (e) {
            ButtonPlusIncrement(e,this);
        });

        //This button will decrement the value
        $('[data-quantity="minus"]').click(function (e) {
            ButtonPlusDecrement(e,this);
        });
    }
});

//Ricarica i filtri presi dalla sessione
function LoadFilter() {
    try {
        //codiceComune viene salvato in una variabile nella razor page della filter modal
        if (codiceComune != null && codiceComune != 0) {
            $("#codiceComune").val(codiceComune);
            $(".search-textbox").val(nomeComune);
            $(".search-button").removeClass("disabled");
        }

        let sliderPrezzo = $('#sliderPrezzo');

        let prezzoMin = $(sliderPrezzo).data("minValue");
        prezzoMin = ((prezzoMin == undefined || prezzoMin == null || prezzoMin == 0) ? 50000 : prezzoMin);

        let prezzoMax = $(sliderPrezzo).data("maxValue");
        prezzoMax = ((prezzoMax == undefined || prezzoMax == null || prezzoMax == 0) ? 250000 : prezzoMax);

        noUiSlider.create(document.getElementById("sliderPrezzo"), {
            start: [prezzoMin, prezzoMax],
            tooltips: [wNumb({
                mark: '.',
                thousand: ',',
                prefix: '€ ',
                suffix: '',
                decimals: 0
            }), wNumb({
                mark: '.',
                thousand: ',',
                prefix: '€ ',
                suffix: '',
                decimals: 0
            })],
            connect: true,
            step: 10000,
            range: {
                'min': 30000,
                'max': 500000
            },
            format: {
                // 'to' the formatted value. Receives a number.
                to: function (value) {
                    return value;
                },
                // 'from' the formatted value.
                // Receives a string, should return a number.
                from: function (value) {
                    return Number(value);
                }
            }
        });

        let sliderDimensione = $('#sliderDimensione');

        let dimensioneMin = $(sliderPrezzo).data("minValue");
        dimensioneMin = ((dimensioneMin == undefined || dimensioneMin == null || dimensioneMin == 0) ? 60 : dimensioneMin);


        let dimensioneMax = $(sliderPrezzo).data("maxValue");
        dimensioneMax = ((dimensioneMax == undefined || dimensioneMax == null || dimensioneMax == 0) ? 250 : dimensioneMax);

        noUiSlider.create(document.getElementById("sliderDimensione"), {
            start: [dimensioneMin, dimensioneMax],
            tooltips: [wNumb({ decimals: 0, suffix: ' m²' }), wNumb({ decimals: 0, suffix: ' m²', })
            ],
            connect: true,
            step: 10,
            range: {
                'min': 30,
                'max': 500
            },
            format: {
                to: function (value) {
                    return value;
                },
                from: function (value) {
                    return Number(value);
                }
            }
        });
    }
        catch (ex) {
        TrapError("Error during LoadFilter:" + ex);
    }
}

//Salva i filtri in sessione
function SaveFilter() {
    try {
        let codiceComune = (($("#codiceComune").val() != null && $("#codiceComune").val() != "") ? parseInt($("#codiceComune").val()) : 0);

        //Ottengo i valori delle slider
        let rangePrice = document.getElementById("sliderPrezzo").noUiSlider.get();
        let rangeDimensione = document.getElementById("sliderDimensione").noUiSlider.get();

        let ascensore = (($("#customSwitchAscensore").is(":checked")) ? true : null);
        let cantina = (($("#customSwitchCantina").is(":checked")) ? true : null);
        let giardino = (($("#customSwitchGiaridno").is(":checked")) ? true : null);
        let piscina = (($("#customSwitchPiscina").is(":checked")) ? true : null);

        let numeroBagni = (($("#NumeroBagni").val() != null && $("#NumeroBagni").val() != "") ? parseInt($("#NumeroBagni").val()) : 0);
        let numeroAltreStanze = (($("#NumeroAltreStanze").val() != null && $("#NumeroAltreStanze").val() != "") ? parseInt($("#NumeroAltreStanze").val()) : 0);
        let numeroCamereLetto = (($("#NumeroCamereLetto").val() != null && $("#NumeroCamereLetto").val() != "") ? parseInt($("#NumeroCamereLetto").val()) : 0);
        let numeroCucine = (($("#NumeroCucine").val() != null && $("#NumeroCucine").val() != "") ? parseInt($("#NumeroCucine").val()) : 0);
        let numeroGarage = (($("#NumeroGarage").val() != null && $("#NumeroGarage").val() != "") ? parseInt($("#NumeroGarage").val()) : 0);
        let numeroPostiAuto = (($("#NumeroPostiAuto").val() != null && $("#NumeroPostiAuto").val() != "") ? parseInt($("#NumeroPostiAuto").val()) : 0);

        let idTipologiaAnnuncio = $("#selectTipologiaAnnuncio").val();
        if (idTipologiaAnnuncio == null || idTipologiaAnnuncio == "") {
            idTipologiaAnnuncio = null;
        }

        let idTipologiaProprieta = $("#selectTipologiaProprieta").val();
        if (idTipologiaProprieta == null || idTipologiaProprieta == "") {
            idTipologiaProprieta = null;
        }

        //creo l'oggetto
        let modelObj = {
            CodiceComune: codiceComune,
            NomeComune: $("#searchbarInput").val(),
            IdTipologiaProprieta: idTipologiaProprieta,
            IdTipologiaAnnuncio: idTipologiaAnnuncio,
            PrezzoMin: rangePrice[0],
            PrezzoMax: rangePrice[1],
            DimensioneMin: rangeDimensione[0],
            DimensioneMax: rangeDimensione[1],
            NumeroBagni: numeroBagni,
            NumeroAltreStanze: numeroAltreStanze,
            NumeroCamereLetto: numeroCamereLetto,
            NumeroCucine: numeroCucine,
            NumeroGarage: numeroGarage,
            NumeroPostiAuto: numeroPostiAuto,
            Ascensore: $("#customSwitchAscensore").is(":checked"),
            Cantina: $("#customSwitchCantina").is(":checked"),
            Giardino: $("#customSwitchGiardino").is(":checked"),
            Piscina: $("#customSwitchPiscina").is(":checked")
        };


        $.ajax({
            type: "POST",
            url: "/Home/SaveSearchFilter",
            data: { Model: modelObj },
            dataType: "json",
            cache: false,
            success: function (result, status, xhr) {
                $("#filterModal").modal("hide");
            },
            error: function (xhr, status, error) {
                TrapError("Error during ajax call SaveFilter");
            }
        });
    }
    catch (ex) {
        TrapError("Error during SaveFilter" + ex);
    }
}


