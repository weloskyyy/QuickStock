    using Microsoft.AspNetCore.Mvc;
    using QuickStock.Domain.Entities;
    using System.Net.Http;
    using System.Net.Http.Json;

    namespace QuickStock.Web.Controllers
    {
        public class HomeController : Controller
        {
            private readonly HttpClient _httpClient;

            public HomeController(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

        public async Task<IActionResult> Index()
        {
            var productUrl = "https://localhost:7122/api/products";
            var categoryUrl = "https://localhost:7122/api/categories";
            var saleUrl = "https://localhost:7122/api/sales";

            List<Product> products = new();
            List<Category> categories = new();
            List<Sale> sales = new();

            // Productos
            var productResponse = await _httpClient.GetAsync(productUrl);
            if (productResponse.IsSuccessStatusCode)
            {
                products = await productResponse.Content.ReadFromJsonAsync<List<Product>>() ?? new();
            }

            // Categorías
            var categoryResponse = await _httpClient.GetAsync(categoryUrl);
            if (categoryResponse.IsSuccessStatusCode)
            {
                categories = await categoryResponse.Content.ReadFromJsonAsync<List<Category>>() ?? new();
            }

            // Ventas
            var saleResponse = await _httpClient.GetAsync(saleUrl);
            if (saleResponse.IsSuccessStatusCode)
            {
                sales = await saleResponse.Content.ReadFromJsonAsync<List<Sale>>() ?? new();
            }

            // ViewBags
            ViewBag.TotalProductos = products.Count;
            ViewBag.TotalCategorias = categories.Count;
            ViewBag.TotalVentas = sales.Count;
            ViewBag.TotalIngresos = sales.Sum(s => s.TotalAmount);
            // Agrupar ingresos por fecha
            var ingresosPorDia = sales
                .GroupBy(s => s.Date.Date)
                .Select(g => new
                {
                    Fecha = g.Key.ToString("yyyy-MM-dd"),
                    Total = g.Sum(s => s.TotalAmount)
                })
                .ToList();

            // Pasar a la vista
            ViewBag.Fechas = ingresosPorDia.Select(i => i.Fecha).ToList();
            ViewBag.Montos = ingresosPorDia.Select(i => i.Total).ToList();


            return View();

        }


    }
}
