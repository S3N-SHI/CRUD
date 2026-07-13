using AdventureWorksVentas.DTOs;
using AdventureWorksVentas.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdventureWorksVentas.Services;

public class VendedorService
{
    private readonly AdventureWorksContext _context;

    public VendedorService(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<VendedorDTO>> ConsultarVendedores()
    {
        List<VendedorDTO> lista = new();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Vendedores_Consultar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new VendedorDTO
            {
                BusinessEntityID = Convert.ToInt32(dr["BusinessEntityID"]),
                NombreVendedor = dr["NombreVendedor"].ToString()!,
                TerritoryID = dr["TerritoryID"] == DBNull.Value ? null : Convert.ToInt32(dr["TerritoryID"]),
                SalesQuota = dr["SalesQuota"] == DBNull.Value ? null : Convert.ToDecimal(dr["SalesQuota"]),
                Bonus = Convert.ToDecimal(dr["Bonus"]),
                CommissionPct = Convert.ToDecimal(dr["CommissionPct"]),
                SalesYTD = Convert.ToDecimal(dr["SalesYTD"])
            });
        }

        return lista;
    }

    public async Task<VendedorDTO?> BuscarVendedor(int id)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Vendedores_Buscar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@BusinessEntityID", id);

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        if (await dr.ReadAsync())
        {
            return new VendedorDTO
            {
                BusinessEntityID = Convert.ToInt32(dr["BusinessEntityID"]),
                NombreVendedor = dr["NombreVendedor"].ToString()!,
                TerritoryID = dr["TerritoryID"] == DBNull.Value ? null : Convert.ToInt32(dr["TerritoryID"]),
                SalesQuota = dr["SalesQuota"] == DBNull.Value ? null : Convert.ToDecimal(dr["SalesQuota"]),
                Bonus = Convert.ToDecimal(dr["Bonus"]),
                CommissionPct = Convert.ToDecimal(dr["CommissionPct"]),
                SalesYTD = Convert.ToDecimal(dr["SalesYTD"])
            };
        }

        return null;
    }
    public async Task<List<VendedorListaDTO>> ConsultarListaVendedores()
{
    List<VendedorListaDTO> lista = new();

    using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

    using SqlCommand cmd = new SqlCommand("sp_Vendedores_Lista", cn);

    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
    {
        lista.Add(new VendedorListaDTO
        {
            BusinessEntityID = Convert.ToInt32(dr["BusinessEntityID"]),
            NombreVendedor = dr["NombreVendedor"].ToString()!
        });
    }

    return lista;
}
}