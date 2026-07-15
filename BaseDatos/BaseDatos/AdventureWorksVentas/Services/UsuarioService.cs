using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using AdventureWorksVentas.Models;
using AdventureWorksVentas.DTOs;

namespace AdventureWorksVentas.Services;

public class UsuarioService
{
    private readonly AdventureWorksContext _context;

    public UsuarioService(AdventureWorksContext context)
    {
        _context = context;
    }

    // Las contraseñas nunca se guardan en texto plano: se guarda
    // el hash SHA256. No es lo mas fuerte que existe (lo ideal en
    // produccion seria bcrypt/PBKDF2 con salt), pero para el
    // alcance de esta materia evita el error mas grave, que es
    // guardar la contraseña tal cual la escribe el usuario.
    private static string HashPassword(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public async Task<List<UsuarioDTO>> ConsultarUsuarios()
    {
        var lista = new List<UsuarioDTO>();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Usuarios_Consultar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        using SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new UsuarioDTO
            {
                UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                NombreUsuario = dr["NombreUsuario"].ToString() ?? "",
                NombreCompleto = dr["NombreCompleto"].ToString() ?? "",
                Rol = dr["Rol"].ToString() ?? "",
                Activo = Convert.ToBoolean(dr["Activo"]),
                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
            });
        }

        return lista;
    }

    public async Task<UsuarioDTO?> BuscarUsuario(int id)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Usuarios_Buscar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UsuarioID", id);

        await cn.OpenAsync();

        using SqlDataReader dr = await cmd.ExecuteReaderAsync();

        if (await dr.ReadAsync())
        {
            return new UsuarioDTO
            {
                UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                NombreUsuario = dr["NombreUsuario"].ToString() ?? "",
                NombreCompleto = dr["NombreCompleto"].ToString() ?? "",
                Rol = dr["Rol"].ToString() ?? "",
                Activo = Convert.ToBoolean(dr["Activo"]),
                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
            };
        }

        return null;
    }

    public async Task<int> GuardarUsuario(UsuarioDTO usuario, string password)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Usuarios_Insertar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
        cmd.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
        cmd.Parameters.AddWithValue("@Rol", usuario.Rol);

        await cn.OpenAsync();

        var resultado = await cmd.ExecuteScalarAsync();

        return Convert.ToInt32(resultado);
    }

    public async Task ActualizarUsuario(UsuarioDTO usuario)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Usuarios_Actualizar", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UsuarioID", usuario.UsuarioID);
        cmd.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
        cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
        cmd.Parameters.AddWithValue("@Activo", usuario.Activo);

        await cn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> EliminarUsuario(int usuarioID)
    {
        try
        {
            using SqlConnection cn = new(_context.Database.GetConnectionString());

            using SqlCommand cmd = new("sp_Usuarios_Eliminar", cn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

            await cn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<UsuarioListaDTO>> ConsultarListaUsuarios()
    {
        var lista = new List<UsuarioListaDTO>();

        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Usuarios_Lista", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        await cn.OpenAsync();

        using SqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new UsuarioListaDTO
            {
                UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                NombreCompleto = dr["NombreCompleto"].ToString() ?? ""
            });
        }

        return lista;
    }

    // Devuelve true si encontro el usuario y lo actualizo, false
    // si el nombre de usuario no existe.
    public async Task<bool> ResetPassword(string nombreUsuario, string nuevaPassword)
    {
        using SqlConnection cn = new(_context.Database.GetConnectionString());

        using SqlCommand cmd = new("sp_Usuarios_ResetPassword", cn);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
        cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(nuevaPassword));

        await cn.OpenAsync();

        var resultado = await cmd.ExecuteScalarAsync();

        return Convert.ToInt32(resultado) > 0;
    }
}