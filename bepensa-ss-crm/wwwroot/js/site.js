// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var loading = `
                <div class="loader-wrapper">
                    <div class="loader-index"><span></span></div>
                    <svg>
                        <defs></defs>
                        <filter id="goo">
                            <feGaussianBlur in="SourceGraphic" stdDeviation="11" result="blur"></feGaussianBlur>
                            <feColorMatrix in="blur" values="1 0 0 0 0  0 1 0 0 0  0 0 1 0 0  0 0 0 19 -9" result="goo"></feColorMatrix>
                        </filter>
                    </svg>
                </div>
            `;

function AbrirLoad() {
    $('body').after(loading);
}

function CerrarLoad() {
    $('.loader-wrapper').fadeOut('slow', function () {
        $(this).remove();
    });
}
function mostrarMensaje(tipo, mensaje, titulo = '¡Aviso!') {

    if (tipo) {
        swal(titulo, mensaje, tipo.toLowerCase());
    }
}

function formatearMiles(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}