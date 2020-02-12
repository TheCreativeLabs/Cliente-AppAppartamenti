$(document).ready(function () {
    var url_string = window.location.href;
    var url = new URL(url_string);

    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)')
            .exec(window.location.href.replace("#", "?"));
        if (results == null) {
            return 0;
        }
        return results[1] || 0;
    }

    var access_token = $.urlParam('access_token');

    $.ajax({
        type: "POST",
        url: "/Login/GoogleLoginResult",
        data: {token:access_token},
        dataType: "json",  
        cache: false,
        success: function (result, status, xhr) {
            if (result != null && result.length > 0) {
                window.location.href = result;
            } else {
                window.location.href = url.origin + "/Home/Index";
            }
        },
        error: function (xhr, status, error) {
            window.location.href = url.origin + "/Home/Index?provider_error=1";
        }
    });
});


