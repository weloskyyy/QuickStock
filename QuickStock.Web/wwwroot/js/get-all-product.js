
 
  @returns {Promise<Array>} 

async function getAllProducts() {
    try {
        
        const apiUrl = "https://localhost:7122/api/products"

      
        const response = await fetch(apiUrl)

     
        if (!response.ok) {
            throw new Error(`Error ${response.status}: ${response.statusText}`)
        }

        
        return await response.json()
    } catch (error) {
        console.error("Error al obtener los productos:", error)
        throw error
    }
}
