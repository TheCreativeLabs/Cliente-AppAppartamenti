﻿$(document).ready(function () {
    try{
        $("#inputAddImage").change(await function () {
            GetProfileImage();
        })
    }catch(ex){
        TrapError("Error during GetProfileImage");
    }
});

async function GetProfileImage(){
    const file = document.querySelector('#inputAddImage').files[0];
    let base64 = await toBase64(file);
    let imageBytes = base64.slice(base64.indexOf(",")).substring(1, base64.lenght);
    $("#ImmagineProfilo").val(imageBytes);
    $("#profileImage").attr("src", base64);
}
