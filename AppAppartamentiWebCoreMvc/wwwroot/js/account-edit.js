$(document).ready(function () {
    try{
        $("#inputAddImage").change(await function () {
            GetProfileImage();
        })
    }catch(ex){
        TrapError("Error during GetProfileImage");
    }
});

async function GetProfileImage() {
    const file = document.querySelector('#inputAddImage').files[0];

    let toBase64 = new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });

    // wait until the promise returns us a value
    let base64 = await toBase64;

    let imageBytes = base64.slice(base64.indexOf(",")).substring(1, base64.lenght);

    $("#ImmagineProfilo").val(imageBytes);
    $("#profileImage").attr("src", base64);
}
