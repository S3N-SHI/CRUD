using AdventureWorksVentas.DTOs;
using AdventureWorksVentas.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdventureWorksVentas.Services;

public class ClienteService
{
    private readonly AdventureWorksContext _context;

    public ClienteService(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<ClienteDTO>> ConsultarClientes()
    {
        List<ClienteDTO> lista = new();

        using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

        using SqlCommand cmd = new SqlCommand("sp_Clientes_Consultar", cn);

        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        await cn.OpenAsync();

        SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new ClienteDTO
            {
                CustomerID = Convert.ToInt32(dr["CustomerID"]),
                NombreCliente = dr["NombreCliente"].ToString()!,
                Email = dr["Email"].ToString()!,
                Telefono = dr["Telefono"].ToString()!
            });
        }

        return lista;
    }

    public async Task<ClienteDTO?> BuscarCliente(int customerID)
{
    using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

    using SqlCommand cmd = new SqlCommand("sp_Clientes_Buscar", cn);

    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.AddWithValue("@CustomerID", customerID);

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    if (await dr.ReadAsync())
    {
        return new ClienteDTO
        {
            CustomerID = Convert.ToInt32(dr["CustomerID"]),
            NombreCliente = dr["NombreCliente"].ToString()!,
            Email = dr["Email"].ToString()!,
            Telefono = dr["Telefono"].ToString()!
        };
    }

    return null;
}
public async Task<bool> EliminarCliente(int customerID)
{
    try
    {
        using SqlConnection cn = new SqlConnection(_context.Database.GetConnectionString());

        using SqlCommand cmd = new SqlCommand("sp_Clientes_Eliminar", cn);

        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustomerID", customerID);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();

        return true;
    }
    catch
    {
        return false;
    }
}
   public async Task<List<ClienteListaDTO>> ConsultarListaClientes()
{
    List<ClienteListaDTO> lista = new();

    using SqlConnection cn = new(_context.Database.GetConnectionString());

    using SqlCommand cmd = new("sp_Clientes_Lista", cn);

    cmd.CommandType = CommandType.StoredProcedure;

    await cn.OpenAsync();

    SqlDataReader dr = await cmd.ExecuteReaderAsync();

    while (await dr.ReadAsync())
    {
        lista.Add(new ClienteListaDTO
        {
            CustomerID = Convert.ToInt32(dr["CustomerID"]),
            NombreCliente = dr["NombreCliente"].ToString()!
        });
    }

    return lista;
}
}