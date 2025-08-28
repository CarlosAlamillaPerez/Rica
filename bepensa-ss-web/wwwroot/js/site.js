function AbrirLoad() {
    $.LoadingOverlay("show");
}

function CerrarLoad() {
    $.LoadingOverlay("hide");
}

function InicializarLoad() {
    $.LoadingOverlay("show");

    setTimeout(function () {
        $('#contenido').css('display', 'block').animate({ opacity: 1 }, 1000);
    }, 10);

    setTimeout(function () {
        $.LoadingOverlay("hide");
    }, 2100);
}

function formatearMiles(num) {
    return num.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}