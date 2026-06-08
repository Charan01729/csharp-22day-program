using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CareBridgeConsole
{
    class Program
    {
        static string connectionString =
            @"Server=localhost;
              Database=CareBridgeDB;
              Trusted_Connection=True;
              TrustServerCertificate=True";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=====================================");
                Console.WriteLine(" CAREBRIDGE CLINICAL OPERATIONS");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. 30-Day Readmissions");
                Console.WriteLine("2. High-Risk Patients");
                Console.WriteLine("3. Provider Workload");
                Console.WriteLine("4. Revenue Analysis");
                Console.WriteLine("5. Exit");
                Console.WriteLine();

                Console.Write("Select Option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid Input.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ExecuteProcedure("usp_30DayReadmissions");
                        break;

                    case 2:
                        ExecuteProcedure("usp_HighRiskPatients");
                        break;

                    case 3:
                        ExecuteProcedure("usp_ProviderWorkload");
                        break;

                    case 4:
                        ExecuteProcedure("usp_RevenueAnalysis");
                        break;

                    case 5:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid Choice.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void ExecuteProcedure(string procedureName)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();

                using SqlCommand cmd = new SqlCommand(procedureName, conn);

                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine($"Results from {procedureName}");
                Console.WriteLine(new string('-', 80));

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader.GetName(i),-25}");
                }

                Console.WriteLine();
                Console.WriteLine(new string('-', 80));

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader[i],-25}");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}