using AdventureWorksVentas.Components;
using AdventureWorksVentas.Models;
using Microsoft.EntityFrameworkCore;
using AdventureWorksVentas.Services;
using QuestPDF.Infrastructure;

// QuestPDF requiere declarar el tipo de licencia antes de generar
// cualquier documento. Community es gratuita para este alcance
// (proyecto academico / empresa chica).
QuestPDF.Settings.License = LicenseType.Community;

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
        builder.Services.AddScoped<ReporteService>();
        builder.Services.AddScoped<UsuarioService>();
        builder.Services.AddScoped<VentaPdfService>();
        
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

// Endpoint que genera y descarga el PDF de una venta puntual.
// Se resuelve como un GET normal (no un metodo de Blazor) porque
// en Blazor Server es la forma mas simple de forzar una descarga
// de archivo binario desde el navegador.
app.MapGet("/ventas/{id:int}/pdf", async (int id, VentaService ventaService, VentaPdfService pdfService) =>
{
    var venta = await ventaService.BuscarVenta(id);

    if (venta == null)
    {
        return Results.NotFound($"No existe la venta {id}.");
    }

    var detalle = await ventaService.ConsultarDetalleVenta(id);

    var pdfBytes = pdfService.Generar(venta, detalle);

    return Results.File(pdfBytes, "application/pdf", $"Venta_{id}.pdf");
});

app.Run();