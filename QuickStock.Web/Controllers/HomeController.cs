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

            var products = await _httpClient.GetFromJsonAsync<List<Product>>(productUrl) ?? new();
            var categories = await _httpClient.GetFromJsonAsync<List<Category>>(categoryUrl) ?? new();
            var sales = await _httpClient.GetFromJsonAsync<List<Sale>>(saleUrl) ?? new();

            ViewBag.TotalProductos = products.Count;
            ViewBag.TotalCategorias = categories.Count;
            ViewBag.TotalVentas = sales.Count;
            ViewBag.TotalIngresos = sales.Sum(s => s.Total);

            return View();
        }
    }
}
