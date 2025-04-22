

document.addEventListener('DOMContentLoaded', function () {
    
    const productForm = document.getElementById('product-form');

    if (productForm) {
        
        productForm.addEventListener('submit', handleProductFormSubmit);
    }

  
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
   
    event.preventDefault();

   
    const form = event.target;

   
    if (!form.checkValidity()) {
        event.stopPropagation();
        form.classList.add('was-validated');
        return;
    }

   
    showLoading(true);

    try {
      
        const formData = new FormData(form);
        const productData = {
            name: formData.get('Name'),
            size: formData.get('Size'),
            color: formData.get('Color'),
            price: parseFloat(formData.get('Price')),
            stock: parseInt(formData.get('Stock')),
            categoryId: parseInt(formData.get('CategoryId'))
        };

       
        const productId = formData.get('Id');
        let result;

        if (productId && productId !== '0') {
            
            productData.id = parseInt(productId);
            result = await updateProduct(productId, productData);
            showMessage('success', 'Producto actualizado exitosamente');
        } else {
           
            result = await createProduct(productData);
            showMessage('success', 'Producto creado exitosamente');
        }

        
        setTimeout(() => {
            window.location.href = '/Product';
        }, 1500);
    } catch (error) {
        console.error('Error al procesar el formulario:', error);
        showMessage('danger', `Error: ${error.message}`);
    } finally {
       
        showLoading(false);
    }
}

/**
 * Muestra un mensaje al usuario
 * @param {string} type - Tipo de mensaje (success, danger, warning, info)
 * @param {string} message - Texto del mensaje
 */
function showMessage(type, message) {
  
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
    alertDiv.role = 'alert';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;

    
    let messageContainer = document.getElementById('message-container');

   
    if (!messageContainer) {
        messageContainer = document.createElement('div');
        messageContainer.id = 'message-container';
        messageContainer.className = 'mb-3';

       
        const form = document.getElementById('product-form');
        form.parentNode.insertBefore(messageContainer, form);
    }

  
    messageContainer.appendChild(alertDiv);

    
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
  
    const submitButton = document.querySelector('button[type="submit"]');

    if (submitButton) {
        if (show) {
          
            submitButton.dataset.originalText = submitButton.innerHTML;
            submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Procesando...';
            submitButton.disabled = true;
        } else {
           
            submitButton.innerHTML = submitButton.dataset.originalText || 'Guardar';
            submitButton.disabled = false;
        }
    }
}