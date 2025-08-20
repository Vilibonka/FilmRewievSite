using FilmIncelemeProjesi.Models;
using Microsoft.EntityFrameworkCore;
using FilmIncelemeProjesi.Services;

var builder = WebApplication.CreateBuilder(args);

// EF Core için SQL Server bağlantısı
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FilmIncelemeDb;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddHttpClient<TmdbService>();

// MVC ve Session servisi ekleniyor
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
