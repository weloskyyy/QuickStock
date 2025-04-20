using QuickStock.Application.Services;
using QuickStock.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using QuickStock.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<SaleService>();
builder.Services.AddScoped<SaleRepository>();
builder.Services.AddScoped<ProductRepository>();


builder.Services.AddHttpClient("QuickStockApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7122");
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
