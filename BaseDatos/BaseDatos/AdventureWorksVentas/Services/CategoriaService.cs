using AdventureWorksVentas.DTOs;
using AdventureWorksVentas.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdventureWorksVentas.Services;

public class CategoriaService
{
    private readonly AdventureWorksContext _context;

    public CategoriaService(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDTO>> ConsultarCategorias()
    {
        List<CategoriaDTO> lista = new();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Categorias_Consultar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new CategoriaDTO
            {
                ProductCategoryID = Convert.ToInt32(dr["ProductCategoryID"]),
                NombreCategoria = dr["NombreCategoria"].ToString()!
            });
        }

        return lista;
    }

    public async Task<CategoriaDTO?> BuscarCategoria(int id)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Categorias_Buscar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductCategoryID", id);

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        if (await dr.ReadAsync())
        {
            return new CategoriaDTO
            {
                ProductCategoryID = Convert.ToInt32(dr["ProductCategoryID"]),
                NombreCategoria = dr["NombreCategoria"].ToString()!
            };
        }

        return null;
    }

    public async Task GuardarCategoria(CategoriaDTO categoria)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Categorias_Insertar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task ActualizarCategoria(CategoriaDTO categoria)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Categorias_Actualizar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductCategoryID", categoria.ProductCategoryID);
        cmd.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> EliminarCategoria(int id)
    {
        try
        {
            using SqlConnection cn = new(_context.Database.GetConnectionString());

            using SqlCommand cmd = new("sp_Categorias_Eliminar", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductCategoryID", id);

            await cn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<CategoriaListaDTO>> ConsultarListaCategorias()
    {
        List<CategoriaListaDTO> lista = new();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Categorias_Lista", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new CategoriaListaDTO
            {
                ProductCategoryID = Convert.ToInt32(dr["ProductCategoryID"]),
                NombreCategoria = dr["NombreCategoria"].ToString()!
            });
        }

        return lista;
    }
}