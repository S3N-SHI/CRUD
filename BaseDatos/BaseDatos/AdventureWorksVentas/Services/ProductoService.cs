using AdventureWorksVentas.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AdventureWorksVentas.Models;

namespace AdventureWorksVentas.Services;

public class ProductoService
{
    private readonly AdventureWorksContext _context;

    public ProductoService(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoDTO>> ConsultarProductos()
    {
        

        List<ProductoDTO> lista = new();

        using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

        using SqlCommand cmd = new SqlCommand("sp_Productos_Consultar", cn);

        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new ProductoDTO
            {
                ProductID = Convert.ToInt32(dr["ProductID"]),
                NombreProducto = dr["NombreProducto"].ToString()!,
                ProductNumber = dr["ProductNumber"].ToString()!,
                Color = dr["Color"]?.ToString(),
                StandardCost = Convert.ToDecimal(dr["StandardCost"]),
                SizeUnitMeasureCode = dr["SizeUnitMeasureCode"]?.ToString(),
                ListPrice = Convert.ToDecimal(dr["ListPrice"]),
                Categoria = dr["Categoria"].ToString()!,
                Subcategoria = dr["Subcategoria"].ToString()!
            });
        }

        return lista;
    }

    public async Task GuardarProducto(ProductoDTO producto)
{
    using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

    using SqlCommand cmd = new SqlCommand("sp_Productos_Insertar", cn);

    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@Name", producto.NombreProducto);
    cmd.Parameters.AddWithValue("@ProductNumber", producto.ProductNumber);
    cmd.Parameters.AddWithValue("@ListPrice", producto.ListPrice);

    if (producto.SizeUnitMeasureCode == null)
        cmd.Parameters.AddWithValue("@SizeUnitMeasureCode", DBNull.Value);
    else
        cmd.Parameters.AddWithValue("@SizeUnitMeasureCode", producto.SizeUnitMeasureCode);

    if (producto.ProductSubcategoryID == null)
        cmd.Parameters.AddWithValue("@ProductSubcategoryID", DBNull.Value);
    else
        cmd.Parameters.AddWithValue("@ProductSubcategoryID", producto.ProductSubcategoryID);

    await cn.OpenAsync();
    await cmd.ExecuteNonQueryAsync();
}

    public async Task<ProductoDTO?> BuscarProducto(int productID)
{
    using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

    using SqlCommand cmd = new SqlCommand("sp_Productos_Buscar", cn);

    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@ProductID", productID);

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    if (await dr.ReadAsync())
    {
        return new ProductoDTO
        {
            ProductID = Convert.ToInt32(dr["ProductID"]),
            NombreProducto = dr["NombreProducto"].ToString()!,
            ProductNumber = dr["ProductNumber"].ToString()!,
            SizeUnitMeasureCode = dr["SizeUnitMeasureCode"]?.ToString(),
            ListPrice = Convert.ToDecimal(dr["ListPrice"]),
            Categoria = dr["Categoria"].ToString()!,
            Subcategoria = dr["Subcategoria"].ToString()!,
            ProductSubcategoryID = dr["ProductSubcategoryID"] == DBNull.Value
                                    ? null
                                    : Convert.ToInt32(dr["ProductSubcategoryID"])
        };
    }

    return null;
}
public async Task ActualizarProducto(ProductoDTO producto)
{
    using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

    using SqlCommand cmd = new SqlCommand("sp_Productos_Actualizar", cn);

    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@ProductID", producto.ProductID);
    cmd.Parameters.AddWithValue("@Name", producto.NombreProducto);
    cmd.Parameters.AddWithValue("@ProductNumber", producto.ProductNumber);
    cmd.Parameters.AddWithValue("@ListPrice", producto.ListPrice);

    if (string.IsNullOrEmpty(producto.SizeUnitMeasureCode))
        cmd.Parameters.AddWithValue("@SizeUnitMeasureCode", DBNull.Value);
    else
        cmd.Parameters.AddWithValue("@SizeUnitMeasureCode", producto.SizeUnitMeasureCode);

    if (producto.ProductSubcategoryID == null)
        cmd.Parameters.AddWithValue("@ProductSubcategoryID", DBNull.Value);
    else
        cmd.Parameters.AddWithValue("@ProductSubcategoryID", producto.ProductSubcategoryID);

    await cn.OpenAsync();
    await cmd.ExecuteNonQueryAsync();
}
public async Task<bool> EliminarProducto(int productID)
{
    try
    {
        using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

        using SqlCommand cmd = new SqlCommand("sp_Productos_Eliminar", cn);

        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductID", productID);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();

        return true;
    }
    catch
    {
        return false;
    }
}

public async Task<List<ProductoListaDTO>> ConsultarListaProductos()
{
    List<ProductoListaDTO> lista = new();

    using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

    using SqlCommand cmd = new SqlCommand("sp_Productos_Lista", cn);

    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
    {
        lista.Add(new ProductoListaDTO
        {
            ProductID = Convert.ToInt32(dr["ProductID"]),
            NombreProducto = dr["NombreProducto"].ToString()!
        });
    }

    return lista;
}

}
