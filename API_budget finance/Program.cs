using API_budget_finance.Context;
using API_budget_finance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);



// --- 1. CONFIGURATION DE LA BASE DE DONNÉES ---
// On dit au serveur : "Utilise SQL Server avec l'adresse notée dans appsettings.json"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IAuthenticatedUserContext, AuthenticatedUserContext>();


// Configuration pour la documentation (OpenAPI)
builder.Services.AddOpenApi();

var app = builder.Build();

// --- 2. CONFIGURATION DU MODE DEV ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<User>();

app.Run();
