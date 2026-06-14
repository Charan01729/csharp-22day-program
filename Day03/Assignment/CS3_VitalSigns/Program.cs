using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("           VITAL SIGNS MONITOR");
        Console.WriteLine("--------------------------------------------------");

        Console.Write("Enter Patient Name: ");
        string patientName = Console.ReadLine();

        double temperature = ReadTemperature();
        int oxygen = ReadOxygenLevel();
        int pulse = ReadPulseRate();

        Console.WriteLine("\n[Analyzing Data...]\n");

        string status = CheckStatus(temperature, oxygen, pulse);
        string reason = GetReason(temperature, oxygen, pulse);

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("       MEDICAL ASSESSMENT REPORT");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"Patient: {patientName}\n");

        Console.WriteLine("Vitals Recorded:");
        Console.WriteLine($"- Temp:   {temperature} C");
        Console.WriteLine($"- Oxygen: {oxygen} %");
        Console.WriteLine($"- Pulse:  {pulse} BPM\n");

        Console.WriteLine($"Status Assessment: {status}");
        Console.WriteLine($"(Reason: {reason})\n");

        if (status == "CRITICAL / EMERGENCY")
        {
            Console.WriteLine("Action: Immediate medical attention required.");
        }
        else if (status == "OBSERVATION NEEDED")
        {
            Console.WriteLine("Action: Nurse to monitor every hour.");
        }
        else
        {
            Console.WriteLine("Action: Continue routine monitoring.");
        }

        Console.WriteLine("--------------------------------------------------");
    }

    static double ReadTemperature()
    {
        while (true)
        {
            try
            {
                Console.Write("Enter Temperature (C): ");
                double temp = Convert.ToDouble(Console.ReadLine());

                if (temp < 25 || temp > 45)
                {
                    Console.WriteLine("Temperature must be between 25°C and 45°C.");
                    continue;
                }

                return temp;
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a numeric temperature.");
            }
        }
    }

    static int ReadOxygenLevel()
    {
        while (true)
        {
            try
            {
                Console.Write("Enter Oxygen Level (%): ");
                int oxygen = Convert.ToInt32(Console.ReadLine());

                if (oxygen < 0 || oxygen > 100)
                {
                    Console.WriteLine("Oxygen level must be between 0 and 100.");
                    continue;
                }

                return oxygen;
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a valid oxygen level.");
            }
        }
    }

    static int ReadPulseRate()
    {
        while (true)
        {
            try
            {
                Console.Write("Enter Pulse Rate (BPM): ");
                int pulse = Convert.ToInt32(Console.ReadLine());

                if (pulse < 20 || pulse > 250)
                {
                    Console.WriteLine("Pulse rate must be between 20 and 250 BPM.");
                    continue;
                }

                return pulse;
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a valid pulse rate.");
            }
        }
    }

    static string CheckStatus(double temp, int oxygen, int pulse)
    {
        if (temp > 39.0 || oxygen < 90 || pulse < 50 || pulse > 120)
        {
            return "CRITICAL / EMERGENCY";
        }
        else if (temp > 37.5 || oxygen < 95 || pulse > 100)
        {
            return "OBSERVATION NEEDED";
        }
        else
        {
            return "NORMAL";
        }
    }

    static string GetReason(double temp, int oxygen, int pulse)
    {
        if (temp > 39.0)
            return "Very High Temperature";

        if (oxygen < 90)
            return "Dangerously Low Oxygen Level";

        if (pulse < 50 || pulse > 120)
            return "Critical Pulse Rate";

        if (temp > 37.5)
            return "Elevated Temperature";

        if (oxygen < 95)
            return "Slightly Low Oxygen Level";

        if (pulse > 100)
            return "High Pulse Rate";

        return "All Vitals Within Normal Range";
    }
}