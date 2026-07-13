using AdventureWorksVentas.Components;
using AdventureWorksVentas.Models;
using Microsoft.EntityFrameworkCore;
using AdventureWorksVentas.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    
builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AdventureWorks")));
        
        builder.Services.AddScoped<ProductoService>();
        builder.Services.AddScoped<SubcategoriaService>();
        builder.Services.AddScoped<ClienteService>();
        builder.Services.AddScoped<CategoriaService>();
        builder.Services.AddScoped<VendedorService>();
        builder.Services.AddScoped<TerritorioService>();
        builder.Services.AddScoped<VentaService>();
        
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
