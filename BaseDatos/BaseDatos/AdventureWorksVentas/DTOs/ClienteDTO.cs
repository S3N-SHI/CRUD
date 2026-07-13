namespace AdventureWorksVentas.DTOs;

public class ClienteDTO
{
    public int CustomerID { get; set; }

    public string NombreCliente { get; set; } = "";

    public string Email { get; set; } = "";

    public string Telefono { get; set; } = "";
}