$(document).ready(function () {
    var url_string = window.location.href;
    var url = new URL(url_string);

    var access_token = window.location.href.replace(window.location.href.slice(window.location.href.indexOf("&token_type=bearer&expires_in="), window.location.href.length), "")
    access_token = access_token.replace("https://localhost:5001/Home/GoogleLogin#access_token=", "");

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
            alert("error");
        }
    });
});


