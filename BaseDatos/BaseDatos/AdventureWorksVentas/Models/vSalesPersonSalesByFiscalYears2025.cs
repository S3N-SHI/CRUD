using System;
using System.Collections.Generic;

namespace AdventureWorksVentas.Models;

public partial class vSalesPersonSalesByFiscalYears2025
{
    public int? SalesPersonID { get; set; }

    public string? FullName { get; set; }

    public string JobTitle { get; set; } = null!;

    public string SalesTerritory { get; set; } = null!;

    public decimal? _2022 { get; set; }

    public decimal? _2023 { get; set; }

    public decimal? _2024 { get; set; }
}
