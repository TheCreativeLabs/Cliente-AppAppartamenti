$(document).ready(function () {
    $("#smallTitle").hide();
    $("#navbar-create").addClass("mt-5");
    window.onscroll = function () { changeScroll() };

    $("input[type=file]").change(function () {
        GetProfileImage(this,$(this).data("list-name"));
    });
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

async function GetProfileImage(input,listname) {
    const file = input.files[0];

    let toBase64 = new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });

    // wait until the promise returns us a value
    let base64 = await toBase64; 

    let imageBytes = base64.slice(base64.indexOf(",")).substring(1, base64.lenght);

    document.getElementById(listname).innerHTML += '<div class="col-xs-12 col-sm-4 p-0 ad-create-image"><div class="border p-2 m-2"><img id="ad-image" src="' + base64 + '" alt="image" class="d-block w-100 " /></div></div>';
}



