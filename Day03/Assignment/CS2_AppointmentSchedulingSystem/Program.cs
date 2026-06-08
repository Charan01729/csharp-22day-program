using System;
using System.Collections.Generic;

namespace AppointmentSchedulingSystem
{
    class Appointment
    {
        public string PatientName { get; set; }
        public string Department { get; set; }
        public string Doctor { get; set; }
        public string TimeSlot { get; set; }

        public void DisplayTicket()
        {
            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("            APPOINTMENT TICKET");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Patient:    {PatientName}");
            Console.WriteLine($"Department: {Department}");
            Console.WriteLine($"Doctor:     {Doctor}");
            Console.WriteLine($"Time:       {TimeSlot}");
            Console.WriteLine($"Status:     Confirmed");
            Console.WriteLine();
            Console.WriteLine("Please arrive 15 mins before your slot.");
            Console.WriteLine("--------------------------------------------------");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> departments = new List<string>
            {
                "General Medicine",
                "Dental",
                "Orthopedics"
            };

            Dictionary<string, List<string>> doctors =
                new Dictionary<string, List<string>>
            {
                {
                    "General Medicine",
                    new List<string>
                    {
                        "Dr. A. Kumar",
                        "Dr. B. Singh"
                    }
                },
                {
                    "Dental",
                    new List<string>
                    {
                        "Dr. C. Roy",
                        "Dr. D. Gupta"
                    }
                },
                {
                    "Orthopedics",
                    new List<string>
                    {
                        "Dr. E. Sharma",
                        "Dr. F. Verma"
                    }
                }
            };

            Dictionary<string, List<string>> doctorSlots =
                new Dictionary<string, List<string>>
            {
                {
                    "Dr. A. Kumar",
                    new List<string>
                    {
                        "10:00 AM",
                        "11:00 AM",
                        "12:00 PM"
                    }
                },
                {
                    "Dr. B. Singh",
                    new List<string>
                    {
                        "09:00 AM",
                        "10:30 AM",
                        "02:00 PM"
                    }
                },
                {
                    "Dr. C. Roy",
                    new List<string>
                    {
                        "11:00 AM",
                        "12:00 PM",
                        "03:00 PM"
                    }
                },
                {
                    "Dr. D. Gupta",
                    new List<string>
                    {
                        "09:30 AM",
                        "01:00 PM",
                        "04:00 PM"
                    }
                },
                {
                    "Dr. E. Sharma",
                    new List<string>
                    {
                        "10:00 AM",
                        "01:00 PM",
                        "05:00 PM"
                    }
                },
                {
                    "Dr. F. Verma",
                    new List<string>
                    {
                        "09:00 AM",
                        "12:30 PM",
                        "03:30 PM"
                    }
                }
            };

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("       APPOINTMENT BOOKING SYSTEM");
            Console.WriteLine("--------------------------------------------------");

            string patientName;

            while (true)
            {
                Console.Write("Enter Patient Name: ");
                patientName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(patientName))
                    break;

                Console.WriteLine("Patient name cannot be empty.");
            }

            int departmentChoice;

            while (true)
            {
                Console.WriteLine("\nSelect Department:");

                for (int i = 0; i < departments.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {departments[i]}");
                }

                Console.Write("Enter Choice: ");

                if (int.TryParse(Console.ReadLine(), out departmentChoice)
                    && departmentChoice >= 1
                    && departmentChoice <= departments.Count)
                {
                    break;
                }

                Console.WriteLine("Invalid department selection.");
            }

            string selectedDepartment =
                departments[departmentChoice - 1];

            List<string> selectedDoctors =
                doctors[selectedDepartment];

            int doctorChoice;

            while (true)
            {
                Console.WriteLine("\nSelect Doctor:");

                for (int i = 0; i < selectedDoctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedDoctors[i]}");
                }

                Console.Write("Enter Choice: ");

                if (int.TryParse(Console.ReadLine(), out doctorChoice)
                    && doctorChoice >= 1
                    && doctorChoice <= selectedDoctors.Count)
                {
                    break;
                }

                Console.WriteLine("Invalid doctor selection.");
            }

            string selectedDoctor =
                selectedDoctors[doctorChoice - 1];

            List<string> availableSlots =
                doctorSlots[selectedDoctor];

            int slotChoice;

            while (true)
            {
                Console.WriteLine("\nSelect Time Slot:");

                for (int i = 0; i < availableSlots.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableSlots[i]}");
                }

                Console.Write("Enter Choice: ");

                if (int.TryParse(Console.ReadLine(), out slotChoice)
                    && slotChoice >= 1
                    && slotChoice <= availableSlots.Count)
                {
                    break;
                }

                Console.WriteLine("Invalid slot selection.");
            }

            string selectedSlot =
                availableSlots[slotChoice - 1];

            Appointment appointment = new Appointment
            {
                PatientName = patientName,
                Department = selectedDepartment,
                Doctor = selectedDoctor,
                TimeSlot = selectedSlot
            };

            Console.WriteLine("\n[Booking Confirmed]");

            appointment.DisplayTicket();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
