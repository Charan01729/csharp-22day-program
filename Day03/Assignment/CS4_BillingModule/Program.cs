using System;

class Bill
{
    public const decimal ConsultationFee = 500m;
    public const decimal BloodTestFee = 200m;
    public const decimal XRayFee = 1000m;
    public const decimal AdmissionFee = 2000m;

    public decimal BaseAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }

    public void CalculateBill(int age, bool consultationAdded)
    {
        decimal discountRate = 0;

        if (age > 60)
        {
            discountRate = 0.20m;
            DiscountAmount = BaseAmount * discountRate;
        }
        else if (age < 10 && consultationAdded)
        {
            DiscountAmount = ConsultationFee * 0.50m;
        }
        else
        {
            DiscountAmount = 0;
        }

        decimal amountAfterDiscount = BaseAmount - DiscountAmount;
        TaxAmount = amountAfterDiscount * 0.05m;
        NetAmount = amountAfterDiscount + TaxAmount;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("       HOSPITAL BILLING CALCULATOR");
        Console.WriteLine("--------------------------------------------------");

        Console.Write("Patient Name: ");
        string patientName = Console.ReadLine();

        Console.Write("Patient Age: ");
        int age = int.Parse(Console.ReadLine());

        Bill bill = new Bill();
        bool consultationAdded = false;

        Console.WriteLine("\nAdd Services:");

        while (true)
        {
            Console.WriteLine("\n1. Consultation (500)");
            Console.WriteLine("2. Blood Test (200)");
            Console.WriteLine("3. X-Ray (1000)");
            Console.WriteLine("4. Admission (2000)");
            Console.WriteLine("5. Done");

            Console.Write("Choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    bill.BaseAmount += Bill.ConsultationFee;
                    consultationAdded = true;
                    Console.WriteLine("[Added Consultation]");
                    break;

                case 2:
                    bill.BaseAmount += Bill.BloodTestFee;
                    Console.WriteLine("[Added Blood Test]");
                    break;

                case 3:
                    bill.BaseAmount += Bill.XRayFee;
                    Console.WriteLine("[Added X-Ray]");
                    break;

                case 4:
                    bill.BaseAmount += Bill.AdmissionFee;
                    Console.WriteLine("[Added Admission]");
                    break;

                case 5:
                    goto Calculate;

                default:
                    Console.WriteLine("Invalid Choice!");
                    break;
            }
        }

    Calculate:

        Console.WriteLine("\n[Calculating Bill...]\n");

        bill.CalculateBill(age, consultationAdded);

        string category;

        if (age > 60)
            category = "Senior Citizen";
        else if (age < 10)
            category = "Child";
        else
            category = "Regular Patient";

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("            FINAL BILL INVOICE");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"Patient: {patientName} ({category})\n");

        Console.WriteLine($"Base Amount:      {bill.BaseAmount:F2}");
        Console.WriteLine($"Discount:        -{bill.DiscountAmount:F2}");
        Console.WriteLine($"Tax (5%):        +{bill.TaxAmount:F2}");

        Console.WriteLine("\n--------------------------------------------------");
        Console.WriteLine($"TOTAL PAYABLE:    {bill.NetAmount:F2}");
        Console.WriteLine("--------------------------------------------------");
    }
}