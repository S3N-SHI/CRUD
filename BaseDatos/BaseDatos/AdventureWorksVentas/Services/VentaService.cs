using AdventureWorksVentas.DTOs;
using AdventureWorksVentas.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdventureWorksVentas.Services;

public class VentaService
{
    private readonly AdventureWorksContext _context;

    public VentaService(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<VentaDTO>> ConsultarVentas()
    {
        List<VentaDTO> lista = new();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Ventas_Consultar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new VentaDTO
            {
                SalesOrderID = Convert.ToInt32(dr["SalesOrderID"]),
                OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                CustomerID = Convert.ToInt32(dr["CustomerID"]),
                Cliente = dr["Cliente"].ToString()!,
                BusinessEntityID = Convert.ToInt32(dr["BusinessEntityID"]),
                Vendedor = dr["Vendedor"].ToString()!,
                Territorio = dr["Territorio"].ToString()!,
                SubTotal = Convert.ToDecimal(dr["SubTotal"]),
                TaxAmt = Convert.ToDecimal(dr["TaxAmt"]),
                Freight = Convert.ToDecimal(dr["Freight"]),
                TotalDue = Convert.ToDecimal(dr["TotalDue"]),
                Anulado = Convert.ToBoolean(dr["Anulado"]),
                FechaAnulacion = dr["FechaAnulacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaAnulacion"])
            });
        }

        return lista;
    }
    public async Task<VentaDTO?> BuscarVenta(int id)
{
    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_Buscar", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@SalesOrderID", id);

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    if (await dr.ReadAsync())
    {
        return new VentaDTO
        {
            SalesOrderID = Convert.ToInt32(dr["SalesOrderID"]),
            OrderDate = Convert.ToDateTime(dr["OrderDate"]),
            CustomerID = Convert.ToInt32(dr["CustomerID"]),
            Cliente = dr["Cliente"].ToString()!,
            BusinessEntityID = Convert.ToInt32(dr["BusinessEntityID"]),
            Vendedor = dr["Vendedor"].ToString()!,
            Territorio = dr["Territorio"].ToString()!,
            SubTotal = Convert.ToDecimal(dr["SubTotal"]),
            TaxAmt = Convert.ToDecimal(dr["TaxAmt"]),
            Freight = Convert.ToDecimal(dr["Freight"]),
            TotalDue = Convert.ToDecimal(dr["TotalDue"]),
            Anulado = Convert.ToBoolean(dr["Anulado"]),
            FechaAnulacion = dr["FechaAnulacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaAnulacion"])
        };
    }

    return null;
    }
    public async Task<List<VentaListaDTO>> ConsultarListaVentas()
{
    List<VentaListaDTO> lista = new();

    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_Lista", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
    {
        lista.Add(new VentaListaDTO
        {
            SalesOrderID = Convert.ToInt32(dr["SalesOrderID"]),
            Cliente = dr["Cliente"].ToString()!
        });
    }

    return lista;
    }

    public async Task<List<VentaDetalleDTO>> ConsultarDetalleVenta(int salesOrderID)
{
    List<VentaDetalleDTO> lista = new();

    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_Detalle", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@SalesOrderID", salesOrderID);

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
    {
        lista.Add(new VentaDetalleDTO
        {
            SalesOrderDetailID = Convert.ToInt32(dr["SalesOrderDetailID"]),
            ProductID = Convert.ToInt32(dr["ProductID"]),
            Producto = dr["Producto"].ToString()!,
            OrderQty = Convert.ToInt16(dr["OrderQty"]),
            UnitPrice = Convert.ToDecimal(dr["UnitPrice"]),
            UnitPriceDiscount = Convert.ToDecimal(dr["UnitPriceDiscount"]),
            Total = Convert.ToDecimal(dr["Total"])
        });
    }

    return lista;
    }

    public async Task<int> CrearCabecera(int customerID, int? salesPersonID)
{
    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_CrearCabecera", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@CustomerID", customerID);

    if (salesPersonID == null)
        cmd.Parameters.AddWithValue("@SalesPersonID", DBNull.Value);
    else
        cmd.Parameters.AddWithValue("@SalesPersonID", salesPersonID);

    await cn.OpenAsync();

    return Convert.ToInt32(await cmd.ExecuteScalarAsync());
}

public async Task AgregarDetalle(int salesOrderID, int productID, short cantidad)
{
    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_AgregarDetalle", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@SalesOrderID", salesOrderID);
    cmd.Parameters.AddWithValue("@ProductID", productID);
    cmd.Parameters.AddWithValue("@Cantidad", cantidad);

    await cn.OpenAsync();

    await cmd.ExecuteNonQueryAsync();
}

public async Task ModificarDetalle(int salesOrderDetailID, short cantidad)
{
    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_ModificarDetalle", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@SalesOrderDetailID", salesOrderDetailID);
    cmd.Parameters.AddWithValue("@Cantidad", cantidad);

    await cn.OpenAsync();

    await cmd.ExecuteNonQueryAsync();
}

public async Task EliminarDetalle(int salesOrderDetailID)
{
    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_EliminarDetalle", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@SalesOrderDetailID", salesOrderDetailID);

    await cn.OpenAsync();

    await cmd.ExecuteNonQueryAsync();
}

public async Task AnularVenta(int salesOrderID)
{
    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Ventas_Anular", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@SalesOrderID", salesOrderID);

    await cn.OpenAsync();

    await cmd.ExecuteNonQueryAsync();
}
}