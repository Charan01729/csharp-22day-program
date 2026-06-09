using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareBridge.PerformanceLab
{
    public class PatientExportStats
    {
        public int EncounterCount { get; set; }
        public int DiagnosisCount { get; set; }
        public int ClaimCount { get; set; }
        public long ElapsedMs { get; set; }
        public int TrackedEntities { get; set; }
    }
}
