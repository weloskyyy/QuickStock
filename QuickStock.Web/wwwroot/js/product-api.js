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


        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(productData)
        };


        const response = await fetch(apiUrl, requestOptions);


        if (!response.ok) {

            const errorData = await response.json().catch(() => null);
            throw new Error(errorData ? JSON.stringify(errorData) : `Error ${response.status}: ${response.statusText}`);
        }


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

        const apiUrl = `https://localhost:7122/api/products/${id}`;


        const requestOptions = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(productData)
        };


        const response = await fetch(apiUrl, requestOptions);


        if (!response.ok) {

            const errorData = await response.json().catch(() => null);
            throw new Error(errorData ? JSON.stringify(errorData) : `Error ${response.status}: ${response.statusText}`);
        }


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

        const apiUrl = `https://localhost:7122/api/products/${id}`;


        const response = await fetch(apiUrl);


        if (!response.ok) {
            throw new Error(`Error ${response.status}: ${response.statusText}`);
        }

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

        const apiUrl = `https://localhost:7122/api/products/${id}`;


        const requestOptions = {
            method: 'DELETE'
        };


        const response = await fetch(apiUrl, requestOptions);


        if (!response.ok) {
            throw new Error(`Error ${response.status}: ${response.statusText}`);
        }


        return true;
    } catch (error) {
        console.error('Error al eliminar el producto:', error);
        throw error;
    }
}