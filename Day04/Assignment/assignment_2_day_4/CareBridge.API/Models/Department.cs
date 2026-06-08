using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareBridge.EFCoreDemo.Models;

public class Department
{
    public int DepartmentId { get; set; }         // DepartmentId column (primary key)
    public string Name { get; set; } = "";        // Name column (e.g. 'Cardiology')
    public string? Location { get; set; }         // Location column
}

