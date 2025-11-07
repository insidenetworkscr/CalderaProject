// UX helpers básicos
(function () {
    // Auto-cerrar modal al enviar formularios (si respuesta es redirect)
    document.addEventListener('submit', function (e) {
        const form = e.target;
        if (form.tagName === 'FORM') {
            const btn = form.querySelector('button[type="submit"]');
            if (btn) {
                btn.disabled = true;
                btn.innerHTML = 'Guardando...';
            }
        }
    }, true);

    // Formateo simple de inputs type=number con rueda accidental
    document.addEventListener('wheel', function (e) {
        if (document.activeElement.type === 'number') {
            document.activeElement.blur();
        }
    }, { passive: true });
})();
