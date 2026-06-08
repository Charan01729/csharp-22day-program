using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareBridge.EFCoreDemo.Models;

public class Provider
{
    public int ProviderId { get; set; }           // ProviderId column (primary key)
    public string FullName { get; set; } = "";    // FullName column (e.g. 'Dr. Maya Patel')
    public string Specialty { get; set; } = "";   // Specialty column
    public int DepartmentId { get; set; }         // DepartmentId column (FK)
}

