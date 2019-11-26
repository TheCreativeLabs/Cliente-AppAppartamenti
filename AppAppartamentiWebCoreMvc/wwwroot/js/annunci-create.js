$(document).ready(function () {
    $("#smallTitle").hide();
    $("#navbar-create").addClass("mt-5");
    window.onscroll = function () { changeScroll() };
});

function changeScroll() {
    if (document.body.scrollTop > 70 || document.documentElement.scrollTop > 70) {
        $("#smallTitle").show();
        $("#navbar-create").removeClass("mt-5");

    } else {
        $("#smallTitle").hide();
        $("#navbar-create").addClass("mt-5");
    }
}

