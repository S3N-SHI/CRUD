namespace AdventureWorksVentas.DTOs;

public class UsuarioDTO
{
    public int UsuarioID { get; set; }

    public string NombreUsuario { get; set; } = "";

    public string NombreCompleto { get; set; } = "";

    public string Rol { get; set; } = "Vendedor";

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; }
}

public class UsuarioListaDTO
{
    public int UsuarioID { get; set; }

    public string NombreCompleto { get; set; } = "";
}