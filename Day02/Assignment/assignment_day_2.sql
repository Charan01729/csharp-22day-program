use CareBridgeDB;
--1st assignment
sp_help Department;
sp_help Encounter;
sp_help Provider;

SELECT
    p.FullName AS ProviderName,
    d.Name AS DepartmentName,

    COUNT(e.EncounterId) AS TotalEncounters,

    RANK() OVER (
        ORDER BY COUNT(e.EncounterId) DESC
    ) AS ProviderRank

FROM Provider p

JOIN Department d
    ON d.DepartmentId = p.DepartmentId

LEFT JOIN Encounter e
    ON e.ProviderId = p.ProviderId

GROUP BY
    p.ProviderId,
    p.FullName,
    d.Name

ORDER BY
    TotalEncounters DESC;

--2nd assignment
sp_help Patient;
sp_help Insurance;

select * from Insurance;
--add two more columns in insurance
ALTER TABLE Insurance
ADD
    ValidFrom DATETIME2
        GENERATED ALWAYS AS ROW START HIDDEN
        CONSTRAINT DF_Insurance_ValidFrom
        DEFAULT SYSUTCDATETIME(),

    ValidTo DATETIME2
        GENERATED ALWAYS AS ROW END HIDDEN
        CONSTRAINT DF_Insurance_ValidTo
        DEFAULT '9999-12-31 23:59:59.9999999',

    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo);

--enable temporal versioning
ALTER TABLE Insurance
SET (
    SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = dbo.Insurance_History
    )
);
--update payer for a patient
UPDATE Insurance
SET Payer = 'Aetna India'
WHERE PatientId = 1;

--checking audit
SELECT
    InsuranceId,
    Payer,
    PolicyNumber,
    ValidFrom,
    ValidTo
FROM Insurance
FOR SYSTEM_TIME ALL
WHERE PatientId = 1
ORDER BY ValidFrom;

--3rd assignment
CREATE PROCEDURE usp_RevenueLeakageAnalysis
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Status AS ClaimStatus,
        COUNT(*) AS TotalClaims,
        SUM(BilledAmount) AS TotalBilledAmount,
        SUM(ISNULL(ReimbursedAmt, 0)) AS TotalReimbursedAmount,
        SUM(BilledAmount - ISNULL(ReimbursedAmt, 0)) AS OutstandingAmount,
        RANK() OVER (
            ORDER BY SUM(BilledAmount - ISNULL(ReimbursedAmt, 0)) DESC
        ) AS LossRank
    FROM Claim
    GROUP BY Status
    ORDER BY LossRank;
END;
GO

EXEC usp_RevenueLeakageAnalysis;

--4th assignment
CREATE PROCEDURE usp_ExecutiveDashboard
AS
BEGIN
    SET NOCOUNT ON;

    -- Total Active Patients 
    SELECT
        'Total Active Patients' AS Metric,
        COUNT(*) AS Value
    FROM Patient
    WHERE IsActive = 1;

   -- Top 5 Departments by Encounters
    SELECT TOP 5
        d.Name AS Department,
        COUNT(e.EncounterId) AS TotalEncounters
    FROM Department d
    JOIN Provider p
        ON d.DepartmentId = p.DepartmentId
    JOIN Encounter e
        ON p.ProviderId = e.ProviderId
    GROUP BY d.Name
    ORDER BY COUNT(e.EncounterId) DESC;

    -- Denied Claims 
    SELECT
        COUNT(*) AS DeniedClaims,
        SUM(BilledAmount) AS TotalDeniedAmount
    FROM Claim
    WHERE Status = 'Denied';
END;
GO

EXEC usp_ExecutiveDashboard;

--5th assignment

--30-Day Readmissions
CREATE PROCEDURE usp_30DayReadmissions
AS
BEGIN
    SELECT PatientId,
           COUNT(*) AS ReadmissionCount
    FROM Encounter
    GROUP BY PatientId
    HAVING COUNT(*) > 1;
END;
GO

--High-Risk Patients
CREATE PROCEDURE usp_HighRiskPatients
AS
BEGIN
    SELECT TOP 10
           PatientId,
           FullName,
           DateOfBirth
    FROM Patient
    ORDER BY DateOfBirth;
END;
GO

--Provider Workload
CREATE PROCEDURE usp_ProviderWorkload
AS
BEGIN
    SELECT
        p.FullName,
        COUNT(e.EncounterId) AS TotalEncounters
    FROM Provider p
    JOIN Encounter e
        ON p.ProviderId = e.ProviderId
    GROUP BY p.FullName
    ORDER BY TotalEncounters DESC;
END;
GO

--Revenue Analysis
CREATE PROCEDURE usp_RevenueAnalysis
AS
BEGIN
    SELECT
        Status,
        COUNT(*) AS Claims,
        SUM(BilledAmount) AS TotalBilled,
        SUM(BilledAmount - ISNULL(ReimbursedAmt,0)) AS Outstanding
    FROM Claim
    GROUP BY Status;
END;
GO

--6th assignment

--Clinical Team View
CREATE VIEW vw_Clinical
AS
SELECT
    p.MRN,
    p.FullName,
    e.EncounterId,
    e.EncounterType,
    e.AdmitDate,
    e.DischargeDate,
    d.IcdCode,
    d.Description AS Diagnosis
FROM Patient p
INNER JOIN Encounter e
    ON p.PatientId = e.PatientId
INNER JOIN Diagnosis d
    ON e.EncounterId = d.EncounterId;
GO

--Billing Team View
CREATE VIEW vw_Billing
AS
SELECT
    c.ClaimId,
    p.MRN,
    p.FullName,
    i.Payer,
    c.BilledAmount,
    c.ReimbursedAmt,
    c.Status
FROM Claim c
INNER JOIN Insurance i
    ON c.InsuranceId = i.InsuranceId
INNER JOIN Patient p
    ON i.PatientId = p.PatientId;
GO

--Analytics Team View
CREATE VIEW vw_Analytics_DeId
AS
SELECT
    CASE
        WHEN DATEDIFF(YEAR,p.DateOfBirth,GETDATE()) < 18 THEN '0-17'
        WHEN DATEDIFF(YEAR,p.DateOfBirth,GETDATE()) BETWEEN 18 AND 35 THEN '18-35'
        WHEN DATEDIFF(YEAR,p.DateOfBirth,GETDATE()) BETWEEN 36 AND 60 THEN '36-60'
        ELSE '60+'
    END AS AgeBand,
    p.Gender,
    e.EncounterType,
    d.Name AS Department
FROM Patient p
INNER JOIN Encounter e
    ON p.PatientId = e.PatientId
INNER JOIN Department d
    ON e.DepartmentId = d.DepartmentId;
GO