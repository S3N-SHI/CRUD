namespace AdventureWorksVentas.DTOs;

public class VentaDTO
{
    public int SalesOrderID { get; set; }

    public DateTime OrderDate { get; set; }

    public int CustomerID { get; set; }

    public string Cliente { get; set; } = "";

    public int BusinessEntityID { get; set; }

    public string Vendedor { get; set; } = "";

    public string Territorio { get; set; } = "";

    public decimal SubTotal { get; set; }

    public decimal TaxAmt { get; set; }

    public decimal Freight { get; set; }

    public decimal TotalDue { get; set; }

    public bool Anulado { get; set; }

    public DateTime? FechaAnulacion { get; set; }
}