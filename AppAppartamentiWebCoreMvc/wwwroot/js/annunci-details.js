$(document).ready(function () {
    $("#secondnav").hide();

    window.onscroll = function () { changeScroll() };
});

function changeScroll() {
    if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
        $("#secondnav").show();
        $("#nav").removeClass("shadow-lg");
       
    } else {
        $("#secondnav").hide();
        $("#nav").addClass("shadow-lg");
    }
}

