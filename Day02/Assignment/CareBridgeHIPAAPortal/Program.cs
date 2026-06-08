using System;
using Microsoft.Data.SqlClient;

namespace CareBridgeHIPAAPortal
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

                Console.WriteLine("====================================");
                Console.WriteLine(" CAREBRIDGE HIPAA ACCESS PORTAL");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Clinical Team");
                Console.WriteLine("2. Billing Team");
                Console.WriteLine("3. Analytics Team");
                Console.WriteLine("4. Exit");
                Console.WriteLine();

                Console.Write("Select Role: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        DisplayView("vw_Clinical");
                        break;

                    case 2:
                        DisplayView("vw_Billing");
                        break;

                    case 3:
                        DisplayView("vw_Analytics_DeId");
                        break;

                    case 4:
                        return;

                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void DisplayView(string viewName)
        {
            try
            {
                using SqlConnection conn =
                    new SqlConnection(connectionString);

                conn.Open();

                string query = $"SELECT TOP 20 * FROM {viewName}";

                using SqlCommand cmd =
                    new SqlCommand(query, conn);

                using SqlDataReader reader =
                    cmd.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine($"Accessing {viewName}");
                Console.WriteLine(new string('-', 120));

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader.GetName(i),-25}");
                }

                Console.WriteLine();
                Console.WriteLine(new string('-', 120));

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