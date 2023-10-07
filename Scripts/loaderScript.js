
document.getElementById('btn-iniciar-sesion').addEventListener('click', function () {
    // Mostrar el modal de carga
    var loaderModal = new bootstrap.Modal(document.getElementById('loader-modal'));
    loaderModal.show();

    // Simula una demora (reemplaza esto con tu código real de inicio de sesión)
    setTimeout(function () {
        // Ocultar el modal de carga (simulando que la acción de inicio de sesión se ha completado)
        loaderModal.hide();
    }, 3000); // Simula 3 segundos de carga
});
