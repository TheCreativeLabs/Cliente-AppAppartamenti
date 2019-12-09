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

    $('#select-order').change(function () {
        ReloadList();
    });
});

function ReloadList() {

    let url = UrlRefresh;
    let order = $("#select-order").val();
    if (order != "") {
        url = url + "&Order=" + $("#select-order").val();
    }
    $('#annunci').load(url);
   
    setInterval('HideLoader()', 10000);
}

function HideLoader() {
    $("#listLoader").hide();
}


