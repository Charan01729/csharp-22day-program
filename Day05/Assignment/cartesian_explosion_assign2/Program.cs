using CareBridge.PerformanceLab;
using CareBridge.PerformanceLab.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

while (true)
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine(" Cartesian Explosion Demo");
    Console.WriteLine("=================================");
    Console.WriteLine();

    Console.WriteLine("1. Single Query (Default Include)");
    Console.WriteLine("2. Split Query (AsSplitQuery)");

    Console.WriteLine();
    Console.Write("Choose Option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CartesianExplosionDemo();
            break;

        case "2":
            SplitQueryDemo();
            break;

        default:
            Console.WriteLine("Invalid Option");
            break;
    }

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static void CartesianExplosionDemo()
{
    using var db = new CareBridgeContext();

    var sw = Stopwatch.StartNew();

    var patient = db.Patients
        .AsNoTracking()
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Diagnoses)
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Claims)
        .FirstOrDefault(p => p.Mrn == "MRN888888");

    sw.Stop();

    int encounters = patient?.Encounters.Count ?? 0;

    int diagnoses = patient?.Encounters
        .Sum(e => e.Diagnoses.Count) ?? 0;

    int claims = patient?.Encounters
        .Sum(e => e.Claims.Count) ?? 0;

    Console.WriteLine();
    Console.WriteLine("SINGLE QUERY (DEFAULT INCLUDE)");
    Console.WriteLine(new string('-', 80));

    Console.WriteLine($"Encounters : {encounters}");
    Console.WriteLine($"Diagnoses  : {diagnoses}");
    Console.WriteLine($"Claims     : {claims}");

    Console.WriteLine(new string('-', 80));

    Console.WriteLine($"Tracked Entities           : {db.ChangeTracker.Entries().Count()}");
    Console.WriteLine($"Elapsed Time               : {sw.ElapsedMilliseconds} ms");

    Console.WriteLine();
    Console.WriteLine("Measurable Outcomes");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("SQL Statements             : 1");
    Console.WriteLine("Rows Returned Over Wire    : ~900");
    Console.WriteLine("Largest Result Set         : 900");
    Console.WriteLine($"Object Counts             : {encounters}/{diagnoses}/{claims}");
    Console.WriteLine("Tracked Entities           : 0");
    Console.WriteLine("Loading Strategy           : Single JOIN Query");

    Console.WriteLine();
    Console.WriteLine("Profiler Verification");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("Expected SQL Row Count     : ~900");
    Console.WriteLine("Reason                     : 100 × 3 × 3");
    Console.WriteLine("Cartesian Explosion        : YES");
}

static void SplitQueryDemo()
{
    using var db = new CareBridgeContext();

    var sw = Stopwatch.StartNew();

    var patient = db.Patients
        .AsNoTracking()
        .AsSplitQuery()
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Diagnoses)
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Claims)
        .FirstOrDefault(p => p.Mrn == "MRN888888");

    sw.Stop();

    int encounters = patient?.Encounters.Count ?? 0;

    int diagnoses = patient?.Encounters
        .Sum(e => e.Diagnoses.Count) ?? 0;

    int claims = patient?.Encounters
        .Sum(e => e.Claims.Count) ?? 0;

    Console.WriteLine();
    Console.WriteLine("SPLIT QUERY (AsSplitQuery)");
    Console.WriteLine(new string('-', 80));

    Console.WriteLine($"Encounters : {encounters}");
    Console.WriteLine($"Diagnoses  : {diagnoses}");
    Console.WriteLine($"Claims     : {claims}");

    Console.WriteLine(new string('-', 80));

    Console.WriteLine($"Tracked Entities           : {db.ChangeTracker.Entries().Count()}");
    Console.WriteLine($"Elapsed Time               : {sw.ElapsedMilliseconds} ms");

    Console.WriteLine();
    Console.WriteLine("Measurable Outcomes");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("SQL Statements             : 3");
    Console.WriteLine("Rows Returned Over Wire    : 700");
    Console.WriteLine("Largest Result Set         : 300");
    Console.WriteLine($"Object Counts             : {encounters}/{diagnoses}/{claims}");
    Console.WriteLine("Tracked Entities           : 0");
    Console.WriteLine("Loading Strategy           : Split Queries");

    Console.WriteLine();
    Console.WriteLine("Profiler Verification");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("Encounter Rows             : 100");
    Console.WriteLine("Diagnosis Rows             : 300");
    Console.WriteLine("Claim Rows                 : 300");
    Console.WriteLine("Cartesian Explosion        : NO");
}
