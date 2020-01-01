$(document).ready(function () {
    
});

function OpenModal(IdChat) {
    let url = DetailChatUrl + "?IdChat=" + IdChat;

    $("#chatModal").modal("show");

    $("#MessagesList").load(url, function () {
    });
}





