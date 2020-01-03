var reloadInterval;

$(document).ready(function () {
    $('#chat-modal').on('hidden.bs.modal', function (e) {
        //stop del timer quando la modale viene chiusa
        clearInterval(reloadInterval);
    })

    $("#btn-send").click(function () {
        SendMessage();
    });
});

function OpenModal(IdChat, PersonToChat) {
    let url = DetailChatUrl + "?IdChat=" + IdChat+ "&IdAnnuncio=&IdPersonToChat=";

    //mostro il loader e rimuovo il content
    $("#chat-spinner-loading").show();
    $("#messages-list").empty();

    $("#chat-modal-title").text(PersonToChat);
    $("#chat-modal").attr("data-idchat", IdChat);

    $("#chat-modal").modal("show");

    ReloadChatContentModal(url, true);

    //refresh della lista ogni 30 sec
    reloadInterval = setInterval(function () {
        ReloadChatContentModal(url, false)
    }, 40000)
}

//Reload del contenuto modal
function ReloadChatContentModal(UrlToLoad, FirstTime){
    $("#messages-list").load(UrlToLoad, function () {
        $("#chat-spinner-loading").hide();

        if (FirstTime == true || $("#chat-modal-body").scrollTop() == $("#chat-modal-body").height()) {
            $("#chat-modal-body").scrollTop($("#chat-modal-body")[0].scrollHeight);
        }
    });
}

//Invio del messaggio
function SendMessage() {
    let idChat = $("#chat-modal").data("idchat");
    let idPerson = $("#chat-modal .row").data("idperson");
    let message = $("#chat-textarea").val();

    let content = '<div class="offset-2 mt-2 mb-2 col-10 bg-primary rounded p-2">';
    content += '<span style="color:white">' + message +'</span>';
    content += '</div>';

    $("#messages-list .row").append(content);
    $("#chat-modal-body").scrollTop($("#chat-modal-body")[0].scrollHeight);
    $("#chat-textarea").val("");

    $.ajax({
        type: "POST",
        url: "/Messaggi/Send",
        data: { IdChat: idChat, IdPersonToChat: idPerson, Message: message},
        dataType: "json",
        cache: false,
        success: function (result, status, xhr) {
            
        },
        error: function (xhr, status, error) {
            TrapError("Error during SendMessage");
        }
    });
}





