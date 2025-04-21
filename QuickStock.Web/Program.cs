using QuickStock.Application.Services;
using QuickStock.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using QuickStock.Infrastructure.Data;
using QuickStock.Web.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<SaleService>();
builder.Services.AddScoped<SaleRepository>();
builder.Services.AddScoped<ProductRepository>();


builder.Services.AddHttpClient("QuickStockApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7122/");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddTransient<ProductController>(provider => {
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var client = httpClientFactory.CreateClient("QuickStockApi");
    return new ProductController(client);
});

builder.Services.AddDbContext<QuickStockDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
