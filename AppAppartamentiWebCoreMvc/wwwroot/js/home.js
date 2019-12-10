$(document).ready(function () {
    $("#nav").addClass("navbar-transparent");
    $("#navSearchBar").hide();
    window.onscroll = function () { changeScroll() };

    $('#btnSignIn').click(function () {
        ShowModalSignIn();
    });

    $('#btnSignUp').click(function () {
        ShowModalSignUp();
    });

    $('#btn-restore-modal').click(function () {
        ShowRestoreModal();
    });

    $("#btn-back-login").click(function () {
        $("#modal-restore").modal("hide");
        $("#modalLogin").modal("show");
    })

    if (loggedUser != null && loggedUser.length > 0) {
        $("#txtSearchHome").keyup(function () {
            EnableSearch(this);
        })
    }

    $("#btn-restore").click(function (e) {
        if ($("#form-restore").valid()) {
            RestorePassword($(this).data("url"));
        } else {
            $("#form-restore").addClass('was-validated');
        }
    });

    $("#condition-check").change(function () {
        if ($(this).is(":checked")) {
            $("#btn-signin").removeAttr("disabled");
        } else {
            $("#btn-signin").attr("disabled","disabled");

        }
    });

    $("#ConfirmPassword").blur(function () {
        if ($(this).val() != $("#Password").val()) {
            $("#confirm-password-error").removeClass("d-none");
            $(this).addClass("input-validation-error");
        } else {
            $("#confirm-password-error").addClass("d-none");
            $(this).removeClass("input-validation-error");
        }
    });
});

function RestorePassword(url) {
    $("#spinner-restore").removeClass("d-none");
    $("#btn-restore").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: url,
        data: { Email: $("#email-restore").val() },
        dataType: "json",
        cache: false,
        success: function (result, status, xhr) {
            location.reload();
        },
        error: function (xhr, status, error) {
            $("#spinner-restore").addClass("d-none");
            $("#btn-restore").removeAttr("disabled");
            Console.log("Error in RestorePassword function: " + error)
        }
    });
}


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
function ShowRestoreModal() {
    $("#modalLogin").modal("hide");
    $("#modal-restore").modal("show");
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



