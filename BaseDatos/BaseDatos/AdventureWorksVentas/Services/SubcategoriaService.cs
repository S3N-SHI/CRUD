using AdventureWorksVentas.DTOs;
using AdventureWorksVentas.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdventureWorksVentas.Services;

public class SubcategoriaService
{
    private readonly AdventureWorksContext _context;

    public SubcategoriaService(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<SubcategoriaDTO>> ConsultarSubcategorias()
    {
        List<SubcategoriaDTO> lista = new();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Subcategorias_Consultar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new SubcategoriaDTO
            {
                ProductSubcategoryID = Convert.ToInt32(dr["ProductSubcategoryID"]),
                NombreSubcategoria = dr["NombreSubcategoria"].ToString()!
            });
        }

        return lista;
    }

    public async Task<SubcategoriaDTO?> BuscarSubcategoria(int id)
    {
        SubcategoriaDTO? subcategoria = null;

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Subcategorias_Buscar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductSubcategoryID", id);

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        if (await dr.ReadAsync())
        {
            subcategoria = new SubcategoriaDTO
            {
                ProductSubcategoryID = Convert.ToInt32(dr["ProductSubcategoryID"]),
                ProductCategoryID = Convert.ToInt32(dr["ProductCategoryID"]),
                NombreSubcategoria = dr["NombreSubcategoria"].ToString()!
            };
        }

        return subcategoria;
    }

    public async Task GuardarSubcategoria(SubcategoriaDTO subcategoria)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Subcategorias_Insertar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductCategoryID", subcategoria.ProductCategoryID);
        cmd.Parameters.AddWithValue("@NombreSubcategoria", subcategoria.NombreSubcategoria);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task ActualizarSubcategoria(SubcategoriaDTO subcategoria)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Subcategorias_Actualizar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductSubcategoryID", subcategoria.ProductSubcategoryID);
        cmd.Parameters.AddWithValue("@ProductCategoryID", subcategoria.ProductCategoryID);
        cmd.Parameters.AddWithValue("@NombreSubcategoria", subcategoria.NombreSubcategoria);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarSubcategoria(int id)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Subcategorias_Eliminar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductSubcategoryID", id);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }
    public async Task<List<SubcategoriaListaDTO>> ConsultarListaSubcategorias()
{
    List<SubcategoriaListaDTO> lista = new();

    using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

    using SqlCommand cmd = new SqlCommand("sp_Subcategorias_Lista", cn);

    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
    {
        lista.Add(new SubcategoriaListaDTO
        {
            ProductSubcategoryID = Convert.ToInt32(dr["ProductSubcategoryID"]),
            NombreSubcategoria = dr["NombreSubcategoria"].ToString()!
        });
    }

    return lista;
}
}