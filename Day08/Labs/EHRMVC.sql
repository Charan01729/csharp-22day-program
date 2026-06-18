--5. DATABASE SETUP (RUN FIRST)
--5.1 Ensure Database & Schema

USE EHRMVC;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Healthcare')
    EXEC('CREATE SCHEMA Healthcare');
GO

--5.2 PatientTransactions Table (Operational Data)

CREATE TABLE Healthcare.PatientTransactions
(
    TransactionId INT IDENTITY PRIMARY KEY,
    PatientName NVARCHAR(100) NOT NULL,
    TransactionType NVARCHAR(20) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    TransactionDate DATETIME2 NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
GO

--5.3 TransactionLedger Table (Immutable History)

CREATE TABLE Healthcare.TransactionLedger
(
    LedgerId INT IDENTITY PRIMARY KEY,
    TransactionId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,     -- Snapshot of original amount
    PreviousHash NVARCHAR(256) NOT NULL,
    CurrentHash NVARCHAR(256) NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Ledger_Transaction
        FOREIGN KEY (TransactionId)
        REFERENCES Healthcare.PatientTransactions(TransactionId)
);
GO


SELECT * FROM Healthcare.PatientTransactions;
SELECT * FROM Healthcare.TransactionLedger;
