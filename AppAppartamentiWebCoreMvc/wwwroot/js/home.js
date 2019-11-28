$(document).ready(function () {
    $("#nav").addClass("navbar-transparent");
    $("#navSearchBar").hide();

    window.onscroll = function () { changeScroll() };

    HideFiltriAggiuntivi();

    //sul focus mostro i filtri aggiuntivi
    //let txtRicerca = document.getElementById("txtRicerca");
    //txtRicerca.addEventListener("focus", ShowFiltriAggiuntivi);
    //txtRicerca.addEventListener("blur", HideFiltriAggiuntivi);

    $("#btnFiltri").click(function () {
        ShowFiltriAggiuntivi();
    });

    $('#btnSignIn').click(ShowModalSignIn);
    $('#btnSignUp').click(ShowModalSignUp);

    var sliderPrezzo = document.getElementById('sliderPrezzo');
    noUiSlider.create(sliderPrezzo, {
        start: [50000, 150000],
        tooltips: [wNumb({
            mark: '.',
            thousand: ',',
            prefix: '€ ',
            suffix: '',
            decimals:0
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
        }
    });

    var sliderDimensione= document.getElementById('sliderDimensione');
    noUiSlider.create(sliderDimensione, {
        start: [80, 150],
        tooltips: [wNumb({ decimals: 0, suffix: ' m²' }), wNumb({ decimals: 0, suffix: ' m²', })
        ],
        connect: true,
        step: 50,
        range: {
            'min': 50,
            'max': 500
        }
    });
});

function changeScroll() {
    if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 80) {
        $("#nav").removeClass("navbar-transparent");
        $("#navSearchBar").show();
    } else {
        $("#nav").addClass("navbar-transparent");
        $("#navSearchBar").hide();
    }
}

function ShowFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").show();
}

function HideFiltriAggiuntivi() {
    $("#divRicercaAggiuntiva").hide();
}

//mostra la modale di login
function ShowModalSignIn() {
    $("#modalLogin").modal("show");
    $("#modalSignUp").modal("hide");
}

//mostra la modale di regisrazione
function ShowModalSignUp() {
    $("#modalLogin").modal("hide");
    $("#modalSignUp").modal("show");
}

