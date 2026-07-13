namespace AdventureWorksVentas.DTOs;

public class VendedorDTO
{
    public int BusinessEntityID { get; set; }

    public string NombreVendedor { get; set; } = "";

    public int? TerritoryID { get; set; }

    public decimal? SalesQuota { get; set; }

    public decimal Bonus { get; set; }

    public decimal CommissionPct { get; set; }

    public decimal SalesYTD { get; set; }
}