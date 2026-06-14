using System;
using System.Collections.Generic;

namespace HospitalSummaryReport
{
    class PatientRecord
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal BillAmount { get; set; }
        public string Status { get; set; }

        public PatientRecord(string name, string department, decimal billAmount, string status)
        {
            Name = name;
            Department = department;
            BillAmount = billAmount;
            Status = status;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<PatientRecord> patients = new List<PatientRecord>()
            {
                new PatientRecord("John Doe", "General", 500m, "Discharged"),
                new PatientRecord("Jane Smith", "Dental", 1200m, "Admitted"),
                new PatientRecord("Bob Brown", "General", 400m, "Discharged"),
                new PatientRecord("Alice W.", "Ortho", 2500m, "Admitted"),
                new PatientRecord("Sam K.", "Dental", 800m, "Discharged"),
                new PatientRecord("David Lee", "Cardiology", 1800m, "Admitted")
            };

            int totalPatients = patients.Count;
            decimal totalRevenue = 0;

            Dictionary<string, int> departmentCount = new Dictionary<string, int>();

            foreach (PatientRecord patient in patients)
            {
                totalRevenue += patient.BillAmount;

                if (departmentCount.ContainsKey(patient.Department))
                {
                    departmentCount[patient.Department]++;
                }
                else
                {
                    departmentCount[patient.Department] = 1;
                }
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("       DAILY HOSPITAL ACTIVITY REPORT");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Date: {DateTime.Now.ToShortDateString()}");
            Console.WriteLine();

            Console.WriteLine("Patient List:");

            int index = 1;
            foreach (PatientRecord patient in patients)
            {
                Console.WriteLine(
                    $"{index}. {patient.Name,-12} - {patient.Department,-10} - ₹{patient.BillAmount}");
                index++;
            }

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("SUMMARY STATISTICS");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine($"Total Patients Visited:  {totalPatients}");
            Console.WriteLine($"Total Revenue:           ₹{totalRevenue:N2}");
            Console.WriteLine();

            Console.WriteLine("Traffic by Department:");

            foreach (var department in departmentCount)
            {
                Console.WriteLine($"- {department.Key}: {department.Value}");
            }

            Console.WriteLine();
            Console.WriteLine("End of Report.");
            Console.WriteLine("--------------------------------------------------");
        }
    }
}