$(document).ready(function () {
    $("#imageid").change(async function () {
        const file = document.querySelector('#imageid').files[0];
        let base64 = await toBase64(file);
        
        let imageBytes = base64.slice(base64.indexOf(",")).substring(1, base64.lenght);
        $("#ImmagineProfilo").val(imageBytes);
        $("#imgFotoProfilo").attr("src", base64);
    })
});

