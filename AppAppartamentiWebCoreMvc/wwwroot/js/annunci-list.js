var listPage = 1;

$(document).ready(function () {
    ReloadList();


    $('#loadFromMainFrame').click(function (e) {
        e.preventdefault;

        $("#listLoader").show();

        UrlRefresh = UrlRefresh.replace(listPage.toString(), (listPage + 1).toString());
        listPage = listPage + 1;
        ReloadList();
    });
});

function ReloadList() {
    $('#annunci').load(UrlRefresh);
   
    setInterval('HideLoader()', 10000);
}

function HideLoader() {
    $("#listLoader").hide();
}


function AddPreferred(btn,url) {
    event.stopPropagation();

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        success: function (result, status, xhr) {
            $(btn).addClass("text-primary");
        },
        error: function (xhr, status, error) {

        }
    });
}
