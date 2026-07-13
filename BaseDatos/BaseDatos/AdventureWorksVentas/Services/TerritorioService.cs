using AdventureWorksVentas.DTOs;
using AdventureWorksVentas.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdventureWorksVentas.Services;

public class TerritorioService
{
    private readonly AdventureWorksContext _context;

    public TerritorioService(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<TerritorioDTO>> ConsultarTerritorios()
    {
        List<TerritorioDTO> lista = new();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Territorios_Consultar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new TerritorioDTO
            {
                TerritoryID = Convert.ToInt32(dr["TerritoryID"]),
                NombreTerritorio = dr["NombreTerritorio"].ToString()!,
                CountryRegionCode = dr["CountryRegionCode"].ToString()!,
                Grupo = dr["Grupo"].ToString()!
            });
        }

        return lista;
    }

    public async Task<TerritorioDTO?> BuscarTerritorio(int id)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Territorios_Buscar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@TerritoryID", id);

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        if (await dr.ReadAsync())
        {
            return new TerritorioDTO
            {
                TerritoryID = Convert.ToInt32(dr["TerritoryID"]),
                NombreTerritorio = dr["NombreTerritorio"].ToString()!,
                CountryRegionCode = dr["CountryRegionCode"].ToString()!,
                Grupo = dr["Grupo"].ToString()!
            };
        }

        return null;
    }

    public async Task<List<TerritorioListaDTO>> ConsultarListaTerritorios()
    {
        List<TerritorioListaDTO> lista = new();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Territorios_Lista", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new TerritorioListaDTO
            {
                TerritoryID = Convert.ToInt32(dr["TerritoryID"]),
                NombreTerritorio = dr["NombreTerritorio"].ToString()!
            });
        }

        return lista;
    }
}