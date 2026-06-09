using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class VwBilling
{
    public int ClaimId { get; set; }

    public string Mrn { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Payer { get; set; } = null!;

    public decimal BilledAmount { get; set; }

    public decimal? ReimbursedAmt { get; set; }

    public string Status { get; set; } = null!;
}
