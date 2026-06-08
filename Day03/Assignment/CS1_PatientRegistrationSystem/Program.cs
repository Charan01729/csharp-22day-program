using System;
using System.Text.RegularExpressions;

namespace PatientRegistrationSystem
{
    class Patient
    {
        public string PatientID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }

        public void DisplayRegistrationSlip()
        {
            Console.WriteLine("\n====================================");
            Console.WriteLine("      PATIENT REGISTRATION SLIP");
            Console.WriteLine("====================================");
            Console.WriteLine($"Patient ID   : {PatientID}");
            Console.WriteLine($"Name         : {Name}");
            Console.WriteLine($"Age          : {Age}");
            Console.WriteLine($"Gender       : {Gender}");
            Console.WriteLine($"Phone Number : {PhoneNumber}");
            Console.WriteLine($"City         : {City}");
            Console.WriteLine("====================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Patient patient = new Patient();

            Console.WriteLine("====================================");
            Console.WriteLine("     PATIENT REGISTRATION SYSTEM");
            Console.WriteLine("====================================");

            Console.Write("Enter Patient ID: ");
            patient.PatientID = Console.ReadLine();

            // Name Validation
            while (true)
            {
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();

                if (Regex.IsMatch(name, @"^[A-Za-z ]+$"))
                {
                    patient.Name = name;
                    break;
                }

                Console.WriteLine("Invalid Name. Use only letters and spaces.");
            }

            // Age Validation
            while (true)
            {
                try
                {
                    Console.Write("Enter Age: ");
                    patient.Age = int.Parse(Console.ReadLine());

                    if (patient.Age <= 0 || patient.Age >= 120)
                    {
                        Console.WriteLine("Age must be between 1 and 119.");
                        continue;
                    }

                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Age. Enter numbers only.");
                }
            }

            // Gender Validation
            while (true)
            {
                Console.Write("Enter Gender (M/F/O): ");
                string gender = Console.ReadLine().ToUpper();

                if (Regex.IsMatch(gender, @"^[MFO]$"))
                {
                    patient.Gender = gender;
                    break;
                }

                Console.WriteLine("Invalid Gender. Enter M, F or O.");
            }

            // Phone Validation
            while (true)
            {
                Console.Write("Enter Phone Number: ");
                string phone = Console.ReadLine();

                if (Regex.IsMatch(phone, @"^[0-9]{10}$"))
                {
                    patient.PhoneNumber = phone;
                    break;
                }

                Console.WriteLine("Phone Number must contain exactly 10 digits.");
            }

            // City Validation
            while (true)
            {
                Console.Write("Enter City: ");
                string city = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(city))
                {
                    patient.City = city;
                    break;
                }

                Console.WriteLine("City cannot be empty.");
            }

            patient.DisplayRegistrationSlip();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}