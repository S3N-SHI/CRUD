using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using AdventureWorksVentas.Models;

namespace AdventureWorksVentas.Services;

// A diferencia de los demas Services, este devuelve DataTable en
// vez de una lista de DTO. Se eligio asi porque cada uno de los 5
// reportes tiene columnas distintas (y ademas cambian segun el
// modo DETALLE/RESUMEN), asi que armar un DTO por cada combinacion
// hubiera significado 10 clases nuevas solo para mostrar una
// tabla. DataTable permite que la pantalla dibuje las columnas
// que vengan, sin importar cuales sean.
public class ReporteService
{
    private readonly AdventureWorksContext _context;

    public ReporteService(AdventureWorksContext context)
    {
        _context = context;
    }

    private async Task<DataTable> EjecutarAsync(string procedimiento, params (string nombre, object? valor)[] parametros)
    {
        var tabla = new DataTable();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new(procedimiento, cn);

        cmd.CommandType = CommandType.StoredProcedure;

        foreach (var (nombre, valor) in parametros)
        {
            cmd.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        }

        await cn.OpenAsync();

        using SqlDataReader dr = await cmd.ExecuteReaderAsync();

        tabla.Load(dr);

        return tabla;
    }

    public Task<DataTable> DetalleVentas(DateTime desde, DateTime hasta, int? clienteId, int? vendedorId) =>
        EjecutarAsync("sp_Reporte_DetalleVentas",
            ("@FechaInicio", desde), ("@FechaFin", hasta),
            ("@CustomerID", clienteId), ("@SalesPersonID", vendedorId));

    public Task<DataTable> ResumenVentas(DateTime desde, DateTime hasta, int? clienteId, int? vendedorId) =>
        EjecutarAsync("sp_Reporte_ResumenVentas",
            ("@FechaInicio", desde), ("@FechaFin", hasta),
            ("@CustomerID", clienteId), ("@SalesPersonID", vendedorId));

    public Task<DataTable> VentasPorProducto(DateTime desde, DateTime hasta, int? productoId, string modo) =>
        EjecutarAsync("sp_Reporte_VentasPorProducto",
            ("@FechaInicio", desde), ("@FechaFin", hasta),
            ("@ProductID", productoId), ("@Modo", modo));

    public Task<DataTable> VentasPorCategoria(DateTime desde, DateTime hasta, int? categoriaId, string modo) =>
        EjecutarAsync("sp_Reporte_VentasPorCategoria",
            ("@FechaInicio", desde), ("@FechaFin", hasta),
            ("@ProductCategoryID", categoriaId), ("@Modo", modo));

    public Task<DataTable> VentasPorTerritorio(DateTime desde, DateTime hasta, int? territorioId, string modo) =>
        EjecutarAsync("sp_Reporte_VentasPorTerritorio",
            ("@FechaInicio", desde), ("@FechaFin", hasta),
            ("@TerritoryID", territorioId), ("@Modo", modo));
}