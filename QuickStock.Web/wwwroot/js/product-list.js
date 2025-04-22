// Script para manejar la visualización de productos


async function getAllProducts() {
    
    return new Promise((resolve) => {
        setTimeout(() => {
            const products = [
                { id: 1, name: "Producto 1", size: "M", color: "Rojo", price: 25.99, stock: 10, categoryId: 1 },
                { id: 2, name: "Producto 2", size: "L", color: "Azul", price: 39.5, stock: 5, categoryId: 2 },
                { id: 3, name: "Producto 3", size: "S", color: "Verde", price: 15.0, stock: 12, categoryId: 1 },
            ]
            resolve(products)
        }, 500)
    })
}

function showMessage(type, message) {
    
    const alertDiv = document.createElement("div")
    alertDiv.className = `alert alert-${type}`
    alertDiv.textContent = message
    const productsContainer = document.getElementById("products-container")
    if (productsContainer) {
        productsContainer.prepend(alertDiv)
        setTimeout(() => {
            alertDiv.remove()
        }, 3000) 
}

async function deleteProduct(productId) {
    // Simulación de la eliminación de un producto
    return new Promise((resolve, reject) => {
        setTimeout(() => {
         
            console.log(`Producto con ID ${productId} eliminado`)
            resolve()
        }, 500)
    })
}

document.addEventListener("DOMContentLoaded", () => {
    
    const viewProductsBtn = document.getElementById("view-products-btn")

    if (viewProductsBtn) {
       
        viewProductsBtn.addEventListener("click", handleViewProducts)
    }
})


async function handleViewProducts() {
   
    const productsContainer = document.getElementById("products-container")
    if (productsContainer) {
        productsContainer.innerHTML =
            '<div class="text-center my-5"><div class="spinner-border" role="status"><span class="visually-hidden">Cargando...</span></div><p class="mt-2">Cargando productos...</p></div>'
    }

    try {
       
        const products = await getAllProducts()

        
        displayProducts(products)
    } catch (error) {
        console.error("Error al cargar los productos:", error)
        showMessage("danger", `Error al cargar los productos: ${error.message}`)

        if (productsContainer) {
            productsContainer.innerHTML =
                '<div class="alert alert-danger">No se pudieron cargar los productos. Por favor, intente nuevamente.</div>'
        }
    }
}


  @param {Array} products 
 
function displayProducts(products) {
    const productsContainer = document.getElementById("products-container")

    if (!productsContainer) return

    productsContainer.innerHTML = ""

    if (products.length === 0) {
        productsContainer.innerHTML = '<div class="alert alert-info">No hay productos disponibles.</div>'
        return
    }

    const table = document.createElement("table")
    table.className = "table table-striped table-hover"

    
    const thead = document.createElement("thead")
    thead.innerHTML = `
        <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Tamaño</th>
            <th>Color</th>
            <th>Precio</th>
            <th>Stock</th>
            <th>Categoría</th>
            <th>Acciones</th>
        </tr>
    `
    table.appendChild(thead)

   
    const tbody = document.createElement("tbody")


    products.forEach((product) => {
        const tr = document.createElement("tr")
        tr.innerHTML = `
            <td>${product.id}</td>
            <td>${product.name}</td>
            <td>${product.size}</td>
            <td>${product.color}</td>
            <td>$${product.price.toFixed(2)}</td>
            <td>${product.stock}</td>
            <td>${product.categoryId}</td>
            <td>
                <div class="btn-group btn-group-sm" role="group">
                    <a href="/Product/Edit/${product.id}" class="btn btn-primary" data-bs-toggle="tooltip" title="Editar">
                        <i class="bi bi-pencil"></i>
                    </a>
                    <button type="button" class="btn btn-danger delete-product" data-id="${product.id}" data-bs-toggle="tooltip" title="Eliminar">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </td>
        `
        tbody.appendChild(tr)
    })

    table.appendChild(tbody)
    productsContainer.appendChild(table)

    
    if (typeof bootstrap !== "undefined" && bootstrap.Tooltip) {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
        tooltipTriggerList.map((tooltipTriggerEl) => new bootstrap.Tooltip(tooltipTriggerEl))
    }

    
    const deleteButtons = document.querySelectorAll(".delete-product")
    deleteButtons.forEach((button) => {
        button.addEventListener("click", async function () {
            const productId = this.getAttribute("data-id")
            if (confirm("¿Está seguro de que desea eliminar este producto?")) {
                try {
                    await deleteProduct(productId)
                    showMessage("success", "Producto eliminado exitosamente")
                    
                    handleViewProducts()
                } catch (error) {
                    showMessage("danger", `Error al eliminar el producto: ${error.message}`)
                }
            }
        })
    })
}
