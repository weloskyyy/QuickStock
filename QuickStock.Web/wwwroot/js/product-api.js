// Funciones para interactuar con la API de productos

/**
 * Crea un nuevo producto mediante una petición a la API
 * @param {Object} productData - Datos del producto a crear
 * @returns {Promise} - Promesa que se resuelve con la respuesta de la API
 */
async function createProduct(productData) {
    try {
        // URL de la API
        const apiUrl = 'https://localhost:7122/api/products';

        // Configuración de la petición
        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(productData)
        };

        // Realizar la petición
        const response = await fetch(apiUrl, requestOptions);

        // Verificar si la respuesta es exitosa
        if (!response.ok) {
            // Intentar obtener el mensaje de error del servidor
            const errorData = await response.json().catch(() => null);
            throw new Error(errorData ? JSON.stringify(errorData) : `Error ${response.status}: ${response.statusText}`);
        }

        // Devolver los datos del producto creado
        return await response.json();
    } catch (error) {
        console.error('Error al crear el producto:', error);
        throw error;
    }
}

/**
 * Actualiza un producto existente mediante una petición a la API
 * @param {number} id - ID del producto a actualizar
 * @param {Object} productData - Datos actualizados del producto
 * @returns {Promise} - Promesa que se resuelve cuando la actualización es exitosa
 */
async function updateProduct(id, productData) {
    try {
        // URL de la API
        const apiUrl = `https://localhost:7122/api/products/${id}`;

        // Configuración de la petición
        const requestOptions = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(productData)
        };

        // Realizar la petición
        const response = await fetch(apiUrl, requestOptions);

        // Verificar si la respuesta es exitosa
        if (!response.ok) {
            // Intentar obtener el mensaje de error del servidor
            const errorData = await response.json().catch(() => null);
            throw new Error(errorData ? JSON.stringify(errorData) : `Error ${response.status}: ${response.statusText}`);
        }

        // La API devuelve 204 No Content en caso de éxito
        return true;
    } catch (error) {
        console.error('Error al actualizar el producto:', error);
        throw error;
    }
}

/**
 * Obtiene un producto por su ID
 * @param {number} id - ID del producto a obtener
 * @returns {Promise} - Promesa que se resuelve con los datos del producto
 */
async function getProductById(id) {
    try {
        // URL de la API
        const apiUrl = `https://localhost:7122/api/products/${id}`;

        // Realizar la petición
        const response = await fetch(apiUrl);

        // Verificar si la respuesta es exitosa
        if (!response.ok) {
            throw new Error(`Error ${response.status}: ${response.statusText}`);
        }

        // Devolver los datos del producto
        return await response.json();
    } catch (error) {
        console.error('Error al obtener el producto:', error);
        throw error;
    }
}

/**
 * Elimina un producto por su ID
 * @param {number} id - ID del producto a eliminar
 * @returns {Promise} - Promesa que se resuelve cuando la eliminación es exitosa
 */
async function deleteProduct(id) {
    try {
        // URL de la API
        const apiUrl = `https://localhost:7122/api/products/${id}`;

        // Configuración de la petición
        const requestOptions = {
            method: 'DELETE'
        };

        // Realizar la petición
        const response = await fetch(apiUrl, requestOptions);

        // Verificar si la respuesta es exitosa
        if (!response.ok) {
            throw new Error(`Error ${response.status}: ${response.statusText}`);
        }

        // La API devuelve 204 No Content en caso de éxito
        return true;
    } catch (error) {
        console.error('Error al eliminar el producto:', error);
        throw error;
    }
}