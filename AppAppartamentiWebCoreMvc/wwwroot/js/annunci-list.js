var listPage = 1;
var isLoading = false;

$(document).ready(function () {
    ReloadList();

    //$('#loadFromMainFrame').click(function (e) {
    //    e.preventdefault;

    //    $("#listLoader").show();
    //    if (CurrentListPage == undefined || CurrentListPage == null || CurrentListPage == "") {
    //        CurrentListPage = 1;
    //    }
    //    UrlRefresh = UrlRefresh.replace(CurrentListPage.toString(), (new Number(CurrentListPage) + 1).toString());
    //    //listPage = listPage + 1;
    //    ReloadList();
    //});

    $('#select-order').change(function () {
        ReloadList();
    });

    var previousScrollPoint = 0;

    $(window).scroll(function () {
        //raggiungimentoFondo = numero che identifica altezza a 200 dalla fine della pagina: voglio che se sto scendendo
        var raggiungimentoFondo = $(document).height() - 200; 
        var posizioneAttuale = $(window).scrollTop() + $(window).height();
        //discesaAttraversamento = true se sto passando per il punto "raggiungimentoFondo" scorrendo dall'alto al basso; 
        //discesaAttraversamento = false se sto passando per il punto "raggiungimentoFondo" scorrendo dal basso all'alto
        var discesaAttraversamento = previousScrollPoint < raggiungimentoFondo;

        //se sono oltre il punto di "raggiungimentoFondo" per la prima volta e stavo andando dall'alto al basso, allora chiamo API
        if (posizioneAttuale > raggiungimentoFondo && discesaAttraversamento) { 
            if (!isLoading) {  //se non sto già caricando, chiedo una nuova pagina alle API
                //alert("near bottom! ricarico");
                isLoading = true;

                UrlRefresh = UrlRefresh.replace(CurrentListPage.toString(), (new Number(CurrentListPage) + 1).toString());
                //listPage = listPage + 1;
                ReloadList();
            }
        }
        previousScrollPoint = posizioneAttuale;
    });
});

function ReloadList() {
    let url = UrlRefresh;

    if ($("#select-order").lenght > 0) {
        let order = $("#select-order").val();
        if (order != "") {
            url = url + "&Order=" + $("#select-order").val();
        }
    }

    $('#annunci').load(url, function () {
    });
}



