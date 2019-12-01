$(document).ready(function () {
    $("#nav").addClass("navbar-transparent");
    $("#navSearchBar").hide();

    let searchbarInput = document.getElementById("txtRicerca");
    searchbarInput.addEventListener("keyup", EnableSearch);

    window.onscroll = function () { changeScroll() };

    $('#btnSignIn').click(ShowModalSignIn);
    $('#btnSignUp').click(ShowModalSignUp);
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

function EnableSearch() {
    if (document.getElementById("txtRicerca").value == "") {
        $("#btnSearchHome").addClass("disabled");
    } else {
        $("#btnSearchHome").removeClass("disabled");
        var nomeComune = $("#txtRicerca").val();

        $.ajax({
            type: "POST",
            url: "/Home/ListaComuni",
            data: { NomeComune: nomeComune },
            dataType: "json",
            cache: false,
            success: function (result, status, xhr) {
                var nam = [];
                for (var i = 0; i < result.length; i++) {
                    nam.push(result[i].NomeComune);
                }

                $("#txtRicerca").autocomplete({
                    source: nam
                });
            },
            error: function (xhr, status, error) {
                //console.log("Error during EnableSearch");
            }
        });
    }
}

