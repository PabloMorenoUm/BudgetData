using System.Numerics;
using BudgetData;
using Microsoft.EntityFrameworkCore;
using BudgetData.Data;
using BudgetData.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BudgetDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetDataContext") ?? throw new InvalidOperationException("Connection string 'BudgetDataContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddBudgetServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BudgetDataContext>();
    db.Database.Migrate();
}

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

app.MapGet("/hello/{name:alpha}", (string name) => $"Hello {name}!");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Index}/{id?}");

app.Run();