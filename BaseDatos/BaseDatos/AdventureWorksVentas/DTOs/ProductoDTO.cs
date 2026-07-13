namespace AdventureWorksVentas.DTOs;

public class ProductoDTO
{
    public int ProductID { get; set; }

    public string NombreProducto { get; set; } = "";

    public string ProductNumber { get; set; } = "";

    public string? Color { get; set; }

    public decimal StandardCost { get; set; }

    public decimal ListPrice { get; set; }

    public string Categoria { get; set; } = "";

    public string Subcategoria { get; set; } = "";

    public string? SizeUnitMeasureCode { get; set; }

    public int? ProductSubcategoryID { get; set; }
}