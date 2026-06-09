using CareBridge.PerformanceLab;
using CareBridge.PerformanceLab.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

while (true)
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine(" Revenue-at-Risk Dashboard");
    Console.WriteLine("=================================");
    Console.WriteLine();

    Console.WriteLine("1.  Per-status summary(Naive Approach)");
    Console.WriteLine("2.  Per-status summary(using AsNoTracking())");

    Console.WriteLine();
    Console.Write("Choose Option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            NaiveApproach();
            break;

        case "2":
            OptimizedApproach(); //for read-only
            break;

        default:
            Console.WriteLine("Invalid Option");
            break;
    }

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static void NaiveApproach()
{
    using var db = new CareBridgeContext();

    var sw = Stopwatch.StartNew();

    var claims = db.Claims.ToList();

    var summary = claims
        .GroupBy(c => c.Status)
        .Select(g => new RevenueSummaryDto
        {
            Status = g.Key,
            ClaimCount = g.Count(),
            TotalBilled = g.Sum(x => x.BilledAmount),
            TotalReimbursed = g.Sum(x => x.ReimbursedAmt),
            Gap = g.Sum(x => x.BilledAmount - x.ReimbursedAmt)
        })
        .OrderByDescending(x => x.TotalBilled)
        .ToList();

    var revenueAtRisk = claims
        .Where(c => c.Status != "Paid")
        .Sum(c => c.BilledAmount);

    sw.Stop();

    Console.WriteLine();
    Console.WriteLine("REVENUE-AT-RISK DASHBOARD (NAIVE)");
    Console.WriteLine(new string('-', 90));

    Console.WriteLine(
        $"{"Status",-12} {"Claims",-10} {"Billed",-18} {"Reimbursed",-18} {"Gap",-18}");

    foreach (var row in summary)
    {
        Console.WriteLine(
            $"{row.Status,-12} {row.ClaimCount,-10} {row.TotalBilled,-18:N2} {row.TotalReimbursed,-18:N2} {row.Gap,-18:N2}");
    }

    Console.WriteLine(new string('-', 90));
    Console.WriteLine($"Revenue At Risk : {revenueAtRisk:N2}");
    Console.WriteLine($"Tracked Entities: {db.ChangeTracker.Entries().Count()}");
    Console.WriteLine($"Rows Loaded     : {claims.Count}");
    Console.WriteLine($"Elapsed Time    : {sw.ElapsedMilliseconds} ms");

    Console.WriteLine();
    Console.WriteLine("Metrics");
    Console.WriteLine("--------------------------------------");
    Console.WriteLine($"Rows Transfered: {claims.Count}");
    Console.WriteLine("Aggregation    : Application Memory");
}

static void OptimizedApproach()
{
    using var db = new CareBridgeContext();

    var sw = Stopwatch.StartNew();

    var summary = db.Claims
        .AsNoTracking()
        .GroupBy(c => c.Status)
        .Select(g => new RevenueSummaryDto
        {
            Status = g.Key,
            ClaimCount = g.Count(),
            TotalBilled = g.Sum(x => x.BilledAmount),
            TotalReimbursed = g.Sum(x => x.ReimbursedAmt),
            Gap = g.Sum(x => x.BilledAmount - x.ReimbursedAmt)
        })
        .OrderByDescending(x => x.TotalBilled)
        .ToList();

    var revenueAtRisk = db.Claims
        .AsNoTracking()
        .Where(c => c.Status != "Paid")
        .Sum(c => c.BilledAmount);

    sw.Stop();

    Console.WriteLine();
    Console.WriteLine("REVENUE-AT-RISK DASHBOARD (OPTIMIZED)");
    Console.WriteLine(new string('-', 90));

    Console.WriteLine(
        $"{"Status",-12} {"Claims",-10} {"Billed",-18} {"Reimbursed",-18} {"Gap",-18}");

    foreach (var row in summary)
    {
        Console.WriteLine(
            $"{row.Status,-12} {row.ClaimCount,-10} {row.TotalBilled,-18:N2} {row.TotalReimbursed,-18:N2} {row.Gap,-18:N2}");
    }

    Console.WriteLine(new string('-', 90));
    Console.WriteLine($"Revenue At Risk : {revenueAtRisk:N2}");
    Console.WriteLine($"Tracked Entities: {db.ChangeTracker.Entries().Count()}");
    Console.WriteLine($"Rows Returned   : {summary.Count}");
    Console.WriteLine($"Elapsed Time    : {sw.ElapsedMilliseconds} ms");

    Console.WriteLine();
    Console.WriteLine("Metrics");
    Console.WriteLine("--------------------------------------");
    Console.WriteLine($"Rows Transfered: {summary.Count}");
    Console.WriteLine("Aggregation    : SQL Server");
}