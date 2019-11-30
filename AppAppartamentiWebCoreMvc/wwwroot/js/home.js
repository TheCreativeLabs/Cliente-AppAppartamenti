$(document).ready(function () {
    $("#nav").addClass("navbar-transparent");
    $("#navSearchBar").hide();

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

