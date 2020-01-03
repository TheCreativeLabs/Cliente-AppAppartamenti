$(document).ready(function () {
    
});

function OpenModal(IdChat, PersonToChat) {
    let url = DetailChatUrl + "?IdChat=" + IdChat;

    $("#chat-modal-title").text(PersonToChat);
    $("#chatModal").attr("data-idchat", IdChat);
    $("#chatModal").modal("show");

    $("#MessagesList").load(url, function () {
    });
}





