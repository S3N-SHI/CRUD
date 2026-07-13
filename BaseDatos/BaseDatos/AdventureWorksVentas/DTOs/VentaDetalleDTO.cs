namespace AdventureWorksVentas.DTOs;

public class VentaDetalleDTO
{
    public int SalesOrderDetailID { get; set; }

    public int ProductID { get; set; }

    public string Producto { get; set; } = "";

    public short OrderQty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal UnitPriceDiscount { get; set; }

    public decimal Total { get; set; }
}