using System;
using System.Collections.Generic;

namespace AdventureWorksVentas.Models;

public partial class vProductModelInstruction
{
    public int ProductModelID { get; set; }

    public string Name { get; set; } = null!;

    public string? Instructions { get; set; }

    public int? LocationID { get; set; }

    public decimal? SetupHours { get; set; }

    public decimal? MachineHours { get; set; }

    public decimal? LaborHours { get; set; }

    public int? LotSize { get; set; }

    public string? Step { get; set; }

    public Guid rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }
}
