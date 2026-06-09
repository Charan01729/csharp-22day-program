using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareBridge.PerformanceLab
{
    public class RevenueSummaryDto
    {
        public string Status { get; set; } = "";
        public int ClaimCount { get; set; }
        public decimal? TotalBilled { get; set; }
        public decimal? TotalReimbursed { get; set; }
        public decimal? Gap { get; set; }
    }
}
