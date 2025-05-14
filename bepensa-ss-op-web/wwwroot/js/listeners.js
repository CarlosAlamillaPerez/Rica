function ActualizarTotalPremios() {
    $.get('/carrito/total-de-premios', function (data) {
        $('#icnCarrito').text(data);
    }).fail(function () {
        alert("Error al actualizar carrito.");
    });
}