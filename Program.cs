using CampusOrdering.Controllers;
using CampusOrdering.Models;
using Microsoft.EntityFrameworkCore;
using CampusOrdering.Interfaces;
using CampusOrdering.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<OrderingContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=FoodOrderingDB;Trusted_Connection=True;MultipleActiveResultSets=true"));



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

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseStaticFiles();
}

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
