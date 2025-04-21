// Script para manejar el formulario de productos

document.addEventListener('DOMContentLoaded', function () {
    // Obtener el formulario de creación de productos
    const productForm = document.getElementById('product-form');

    if (productForm) {
        // Agregar evento de envío al formulario
        productForm.addEventListener('submit', handleProductFormSubmit);
    }

    // Inicializar tooltips de Bootstrap si existen
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }
});

/**
 * Maneja el envío del formulario de productos
 * @param {Event} event - Evento de envío del formulario
 */
async function handleProductFormSubmit(event) {
    // Prevenir el envío tradicional del formulario
    event.preventDefault();

    // Obtener el formulario
    const form = event.target;

    // Verificar si el formulario es válido según las validaciones HTML5
    if (!form.checkValidity()) {
        event.stopPropagation();
        form.classList.add('was-validated');
        return;
    }

    // Mostrar indicador de carga
    showLoading(true);

    try {
        // Obtener los datos del formulario
        const formData = new FormData(form);
        const productData = {
            name: formData.get('Name'),
            size: formData.get('Size'),
            color: formData.get('Color'),
            price: parseFloat(formData.get('Price')),
            stock: parseInt(formData.get('Stock')),
            categoryId: parseInt(formData.get('CategoryId'))
        };

        // Verificar si es una creación o actualización
        const productId = formData.get('Id');
        let result;

        if (productId && productId !== '0') {
            // Es una actualización
            productData.id = parseInt(productId);
            result = await updateProduct(productId, productData);
            showMessage('success', 'Producto actualizado exitosamente');
        } else {
            // Es una creación
            result = await createProduct(productData);
            showMessage('success', 'Producto creado exitosamente');
        }

        // Redirigir a la lista de productos después de un breve retraso
        setTimeout(() => {
            window.location.href = '/Product';
        }, 1500);
    } catch (error) {
        console.error('Error al procesar el formulario:', error);
        showMessage('danger', `Error: ${error.message}`);
    } finally {
        // Ocultar indicador de carga
        showLoading(false);
    }
}

/**
 * Muestra un mensaje al usuario
 * @param {string} type - Tipo de mensaje (success, danger, warning, info)
 * @param {string} message - Texto del mensaje
 */
function showMessage(type, message) {
    // Crear el elemento de alerta
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
    alertDiv.role = 'alert';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;

    // Obtener el contenedor para mensajes
    let messageContainer = document.getElementById('message-container');

    // Si no existe, crearlo
    if (!messageContainer) {
        messageContainer = document.createElement('div');
        messageContainer.id = 'message-container';
        messageContainer.className = 'mb-3';

        // Insertar antes del formulario
        const form = document.getElementById('product-form');
        form.parentNode.insertBefore(messageContainer, form);
    }

    // Agregar la alerta al contenedor
    messageContainer.appendChild(alertDiv);

    // Configurar eliminación automática después de 5 segundos
    setTimeout(() => {
        alertDiv.classList.remove('show');
        setTimeout(() => alertDiv.remove(), 150);
    }, 5000);
}

/**
 * Muestra u oculta el indicador de carga
 * @param {boolean} show - Indica si se debe mostrar u ocultar el indicador
 */
function showLoading(show) {
    // Obtener el botón de envío
    const submitButton = document.querySelector('button[type="submit"]');

    if (submitButton) {
        if (show) {
            // Guardar el texto original y deshabilitar el botón
            submitButton.dataset.originalText = submitButton.innerHTML;
            submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Procesando...';
            submitButton.disabled = true;
        } else {
            // Restaurar el texto original y habilitar el botón
            submitButton.innerHTML = submitButton.dataset.originalText || 'Guardar';
            submitButton.disabled = false;
        }
    }
}