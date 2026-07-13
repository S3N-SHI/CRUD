using System;
using System.Collections.Generic;

namespace AdventureWorksVentas.Models;

public partial class vStateProvinceCountryRegion
{
    public int StateProvinceID { get; set; }

    public string StateProvinceCode { get; set; } = null!;

    public string StateProvinceName { get; set; } = null!;

    public int TerritoryID { get; set; }

    public string CountryRegionCode { get; set; } = null!;

    public string CountryRegionName { get; set; } = null!;
}
