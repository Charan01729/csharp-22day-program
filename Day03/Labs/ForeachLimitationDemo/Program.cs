using System;
using System.Collections.Generic;

namespace ForeachLimitationDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FOREACH LIMITATION DEMO ===");

            // Create a list of patients
            List<string> patients = new List<string>
            {
                "John",
                "Jane",
                "Alice",
                "Jane"
            };

            Console.WriteLine("\nInitial List:");
            PrintList(patients);
           
        }
        static void PrintList(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

    }
}