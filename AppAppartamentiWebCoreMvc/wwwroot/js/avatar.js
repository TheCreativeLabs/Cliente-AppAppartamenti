$(document).ready(function () {
    $(".avatar-picture").click(function () {
        let id = $(this).data("id");

        $(".avatar-picture").parent().removeClass("border border-dark");
        $(this).parent().addClass("border border-dark");

        $.ajax({
            type: "POST",
            url: '/VirtualAssistent/ChangeAvatarImage',
            data: { Id: id },
            dataType: "json",
            cache: false,
            success: function (result, status, xhr) {
                alert('appuntamento done');
            },
            error: function (xhr, status, error) {
                $('#appointmentModal').modal("hide");
            }
        });
    });
});





