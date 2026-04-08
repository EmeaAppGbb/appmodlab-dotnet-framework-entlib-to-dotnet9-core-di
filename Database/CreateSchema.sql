-- Health Claims Processor Database Schema
-- SQL Server / LocalDB

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HealthClaimsDB')
BEGIN
    CREATE DATABASE HealthClaimsDB;
END
GO

USE HealthClaimsDB;
GO

-- Patients Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patients')
BEGIN
    CREATE TABLE Patients (
        PatientId INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        DateOfBirth DATETIME NOT NULL,
        SSN NVARCHAR(11) NOT NULL,
        Address NVARCHAR(100) NOT NULL,
        City NVARCHAR(50) NOT NULL,
        State NVARCHAR(2) NOT NULL,
        ZipCode NVARCHAR(10) NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        Email NVARCHAR(100),
        InsurancePolicyNumber NVARCHAR(50) NOT NULL,
        InsuranceGroupNumber NVARCHAR(50) NOT NULL,
        InsuranceEffectiveDate DATETIME NOT NULL,
        InsuranceTerminationDate DATETIME NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
END
GO

-- Providers Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Providers')
BEGIN
    CREATE TABLE Providers (
        ProviderId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        NPI NVARCHAR(10) NOT NULL,
        TaxId NVARCHAR(11) NOT NULL,
        Specialty NVARCHAR(50) NOT NULL,
        Address NVARCHAR(100) NOT NULL,
        City NVARCHAR(50) NOT NULL,
        State NVARCHAR(2) NOT NULL,
        ZipCode NVARCHAR(10) NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        Fax NVARCHAR(20),
        Email NVARCHAR(100),
        IsNetworkProvider BIT NOT NULL DEFAULT 1,
        CredentialedDate DATETIME NOT NULL,
        CredentialExpirationDate DATETIME NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
END
GO

-- Claims Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Claims')
BEGIN
    CREATE TABLE Claims (
        ClaimId INT IDENTITY(1,1) PRIMARY KEY,
        ClaimNumber NVARCHAR(50) NOT NULL UNIQUE,
        PatientId INT NOT NULL FOREIGN KEY REFERENCES Patients(PatientId),
        ProviderId INT NOT NULL FOREIGN KEY REFERENCES Providers(ProviderId),
        ServiceDateFrom DATETIME NOT NULL,
        ServiceDateTo DATETIME NOT NULL,
        SubmittedDate DATETIME NOT NULL,
        Status INT NOT NULL DEFAULT 0,
        PlaceOfService NVARCHAR(20) NOT NULL,
        DiagnosisPointer NVARCHAR(50),
        TotalChargeAmount DECIMAL(18, 2) NOT NULL,
        TotalApprovedAmount DECIMAL(18, 2) NOT NULL DEFAULT 0,
        TotalPatientResponsibility DECIMAL(18, 2) NOT NULL DEFAULT 0,
        PriorAuthorizationNumber NVARCHAR(50),
        ReferringProviderNPI NVARCHAR(10),
        IsAccidentRelated BIT NOT NULL DEFAULT 0,
        IsEmploymentRelated BIT NOT NULL DEFAULT 0,
        ProcessedBy NVARCHAR(100),
        ProcessedDate DATETIME NULL,
        AdjudicationNotes NVARCHAR(MAX),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME NULL,
        CreatedBy NVARCHAR(100),
        ModifiedBy NVARCHAR(100)
    );
END
GO

-- ClaimLineItems Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ClaimLineItems')
BEGIN
    CREATE TABLE ClaimLineItems (
        LineItemId INT IDENTITY(1,1) PRIMARY KEY,
        ClaimId INT NOT NULL FOREIGN KEY REFERENCES Claims(ClaimId),
        LineNumber INT NOT NULL,
        ServiceDate DATETIME NOT NULL,
        CPTCode NVARCHAR(10) NOT NULL,
        CPTDescription NVARCHAR(200),
        ICD10Code NVARCHAR(10) NOT NULL,
        ICD10Description NVARCHAR(200),
        Quantity INT NOT NULL,
        UnitCharge DECIMAL(18, 2) NOT NULL,
        TotalCharge DECIMAL(18, 2) NOT NULL,
        ApprovedAmount DECIMAL(18, 2) NOT NULL DEFAULT 0,
        PatientResponsibility DECIMAL(18, 2) NOT NULL DEFAULT 0,
        DenialReason NVARCHAR(500),
        Notes NVARCHAR(MAX)
    );
END
GO

-- Payments Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        PaymentId INT IDENTITY(1,1) PRIMARY KEY,
        ClaimId INT NOT NULL FOREIGN KEY REFERENCES Claims(ClaimId),
        PaymentNumber NVARCHAR(50) NOT NULL UNIQUE,
        PaymentAmount DECIMAL(18, 2) NOT NULL,
        PaymentDate DATETIME NOT NULL,
        PaymentMethod NVARCHAR(20) NOT NULL,
        CheckNumber NVARCHAR(50),
        EFTTraceNumber NVARCHAR(50),
        PayeeName NVARCHAR(100),
        PayeeAddress NVARCHAR(200),
        PaymentStatus NVARCHAR(20) NOT NULL,
        ClearedDate DATETIME NULL,
        RemittanceAdviceNumber NVARCHAR(50),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME NULL,
        CreatedBy NVARCHAR(100),
        ModifiedBy NVARCHAR(100)
    );
END
GO

-- AuditLog Table for Enterprise Library Logging
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLog')
BEGIN
    CREATE TABLE AuditLog (
        LogId INT IDENTITY(1,1) PRIMARY KEY,
        EventId INT,
        Priority INT,
        Severity NVARCHAR(50),
        Title NVARCHAR(500),
        Timestamp DATETIME,
        MachineName NVARCHAR(100),
        AppDomainName NVARCHAR(500),
        ProcessId NVARCHAR(50),
        ProcessName NVARCHAR(500),
        ThreadName NVARCHAR(500),
        Win32ThreadId NVARCHAR(50),
        Message NVARCHAR(MAX),
        FormattedMessage NVARCHAR(MAX),
        Category NVARCHAR(100)
    );
END
GO

-- Stored Procedures for Patients
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetAllPatients') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetAllPatients;
GO

CREATE PROCEDURE sp_GetAllPatients
AS
BEGIN
    SELECT * FROM Patients WHERE IsActive = 1 ORDER BY LastName, FirstName;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetPatientById') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetPatientById;
GO

CREATE PROCEDURE sp_GetPatientById
    @PatientId INT
AS
BEGIN
    SELECT * FROM Patients WHERE PatientId = @PatientId;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_InsertPatient') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_InsertPatient;
GO

CREATE PROCEDURE sp_InsertPatient
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATETIME,
    @SSN NVARCHAR(11),
    @Address NVARCHAR(100),
    @City NVARCHAR(50),
    @State NVARCHAR(2),
    @ZipCode NVARCHAR(10),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @InsurancePolicyNumber NVARCHAR(50),
    @InsuranceGroupNumber NVARCHAR(50),
    @InsuranceEffectiveDate DATETIME,
    @InsuranceTerminationDate DATETIME,
    @IsActive BIT,
    @NewPatientId INT OUTPUT
AS
BEGIN
    INSERT INTO Patients (FirstName, LastName, DateOfBirth, SSN, Address, City, State, ZipCode, Phone, Email,
        InsurancePolicyNumber, InsuranceGroupNumber, InsuranceEffectiveDate, InsuranceTerminationDate, IsActive)
    VALUES (@FirstName, @LastName, @DateOfBirth, @SSN, @Address, @City, @State, @ZipCode, @Phone, @Email,
        @InsurancePolicyNumber, @InsuranceGroupNumber, @InsuranceEffectiveDate, @InsuranceTerminationDate, @IsActive);
    
    SET @NewPatientId = SCOPE_IDENTITY();
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_UpdatePatient') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_UpdatePatient;
GO

CREATE PROCEDURE sp_UpdatePatient
    @PatientId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DateOfBirth DATETIME,
    @SSN NVARCHAR(11),
    @Address NVARCHAR(100),
    @City NVARCHAR(50),
    @State NVARCHAR(2),
    @ZipCode NVARCHAR(10),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @InsurancePolicyNumber NVARCHAR(50),
    @InsuranceGroupNumber NVARCHAR(50),
    @InsuranceEffectiveDate DATETIME,
    @InsuranceTerminationDate DATETIME,
    @IsActive BIT
AS
BEGIN
    UPDATE Patients 
    SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, SSN = @SSN,
        Address = @Address, City = @City, State = @State, ZipCode = @ZipCode, Phone = @Phone, Email = @Email,
        InsurancePolicyNumber = @InsurancePolicyNumber, InsuranceGroupNumber = @InsuranceGroupNumber,
        InsuranceEffectiveDate = @InsuranceEffectiveDate, InsuranceTerminationDate = @InsuranceTerminationDate,
        IsActive = @IsActive, ModifiedDate = GETDATE()
    WHERE PatientId = @PatientId;
END
GO

-- Stored Procedures for Providers
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetAllProviders') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetAllProviders;
GO

CREATE PROCEDURE sp_GetAllProviders
AS
BEGIN
    SELECT * FROM Providers WHERE IsActive = 1 ORDER BY Name;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetProviderById') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetProviderById;
GO

CREATE PROCEDURE sp_GetProviderById
    @ProviderId INT
AS
BEGIN
    SELECT * FROM Providers WHERE ProviderId = @ProviderId;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_InsertProvider') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_InsertProvider;
GO

CREATE PROCEDURE sp_InsertProvider
    @Name NVARCHAR(100),
    @NPI NVARCHAR(10),
    @TaxId NVARCHAR(11),
    @Specialty NVARCHAR(50),
    @Address NVARCHAR(100),
    @City NVARCHAR(50),
    @State NVARCHAR(2),
    @ZipCode NVARCHAR(10),
    @Phone NVARCHAR(20),
    @Fax NVARCHAR(20),
    @Email NVARCHAR(100),
    @IsNetworkProvider BIT,
    @CredentialedDate DATETIME,
    @CredentialExpirationDate DATETIME,
    @IsActive BIT,
    @NewProviderId INT OUTPUT
AS
BEGIN
    INSERT INTO Providers (Name, NPI, TaxId, Specialty, Address, City, State, ZipCode, Phone, Fax, Email,
        IsNetworkProvider, CredentialedDate, CredentialExpirationDate, IsActive)
    VALUES (@Name, @NPI, @TaxId, @Specialty, @Address, @City, @State, @ZipCode, @Phone, @Fax, @Email,
        @IsNetworkProvider, @CredentialedDate, @CredentialExpirationDate, @IsActive);
    
    SET @NewProviderId = SCOPE_IDENTITY();
END
GO

-- Stored Procedures for Claims
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetAllClaims') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetAllClaims;
GO

CREATE PROCEDURE sp_GetAllClaims
AS
BEGIN
    SELECT * FROM Claims ORDER BY SubmittedDate DESC;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetClaimById') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetClaimById;
GO

CREATE PROCEDURE sp_GetClaimById
    @ClaimId INT
AS
BEGIN
    SELECT * FROM Claims WHERE ClaimId = @ClaimId;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetClaimsByPatientId') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetClaimsByPatientId;
GO

CREATE PROCEDURE sp_GetClaimsByPatientId
    @PatientId INT
AS
BEGIN
    SELECT * FROM Claims WHERE PatientId = @PatientId ORDER BY SubmittedDate DESC;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_InsertClaim') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_InsertClaim;
GO

CREATE PROCEDURE sp_InsertClaim
    @ClaimNumber NVARCHAR(50),
    @PatientId INT,
    @ProviderId INT,
    @ServiceDateFrom DATETIME,
    @ServiceDateTo DATETIME,
    @SubmittedDate DATETIME,
    @Status INT,
    @PlaceOfService NVARCHAR(20),
    @DiagnosisPointer NVARCHAR(50),
    @TotalChargeAmount DECIMAL(18, 2),
    @TotalApprovedAmount DECIMAL(18, 2),
    @TotalPatientResponsibility DECIMAL(18, 2),
    @PriorAuthorizationNumber NVARCHAR(50),
    @ReferringProviderNPI NVARCHAR(10),
    @IsAccidentRelated BIT,
    @IsEmploymentRelated BIT,
    @CreatedBy NVARCHAR(100),
    @NewClaimId INT OUTPUT
AS
BEGIN
    INSERT INTO Claims (ClaimNumber, PatientId, ProviderId, ServiceDateFrom, ServiceDateTo, SubmittedDate, Status,
        PlaceOfService, DiagnosisPointer, TotalChargeAmount, TotalApprovedAmount, TotalPatientResponsibility,
        PriorAuthorizationNumber, ReferringProviderNPI, IsAccidentRelated, IsEmploymentRelated, CreatedBy)
    VALUES (@ClaimNumber, @PatientId, @ProviderId, @ServiceDateFrom, @ServiceDateTo, @SubmittedDate, @Status,
        @PlaceOfService, @DiagnosisPointer, @TotalChargeAmount, @TotalApprovedAmount, @TotalPatientResponsibility,
        @PriorAuthorizationNumber, @ReferringProviderNPI, @IsAccidentRelated, @IsEmploymentRelated, @CreatedBy);
    
    SET @NewClaimId = SCOPE_IDENTITY();
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_UpdateClaimStatus') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_UpdateClaimStatus;
GO

CREATE PROCEDURE sp_UpdateClaimStatus
    @ClaimId INT,
    @Status INT,
    @ProcessedBy NVARCHAR(100),
    @ProcessedDate DATETIME
AS
BEGIN
    UPDATE Claims 
    SET Status = @Status, ProcessedBy = @ProcessedBy, ProcessedDate = @ProcessedDate, ModifiedDate = GETDATE()
    WHERE ClaimId = @ClaimId;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_InsertClaimLineItem') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_InsertClaimLineItem;
GO

CREATE PROCEDURE sp_InsertClaimLineItem
    @ClaimId INT,
    @LineNumber INT,
    @ServiceDate DATETIME,
    @CPTCode NVARCHAR(10),
    @CPTDescription NVARCHAR(200),
    @ICD10Code NVARCHAR(10),
    @ICD10Description NVARCHAR(200),
    @Quantity INT,
    @UnitCharge DECIMAL(18, 2),
    @TotalCharge DECIMAL(18, 2),
    @ApprovedAmount DECIMAL(18, 2),
    @PatientResponsibility DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO ClaimLineItems (ClaimId, LineNumber, ServiceDate, CPTCode, CPTDescription, ICD10Code, ICD10Description,
        Quantity, UnitCharge, TotalCharge, ApprovedAmount, PatientResponsibility)
    VALUES (@ClaimId, @LineNumber, @ServiceDate, @CPTCode, @CPTDescription, @ICD10Code, @ICD10Description,
        @Quantity, @UnitCharge, @TotalCharge, @ApprovedAmount, @PatientResponsibility);
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetClaimLineItems') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetClaimLineItems;
GO

CREATE PROCEDURE sp_GetClaimLineItems
    @ClaimId INT
AS
BEGIN
    SELECT * FROM ClaimLineItems WHERE ClaimId = @ClaimId ORDER BY LineNumber;
END
GO

-- Stored Procedures for Payments
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetAllPayments') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetAllPayments;
GO

CREATE PROCEDURE sp_GetAllPayments
AS
BEGIN
    SELECT * FROM Payments ORDER BY PaymentDate DESC;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetPaymentById') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetPaymentById;
GO

CREATE PROCEDURE sp_GetPaymentById
    @PaymentId INT
AS
BEGIN
    SELECT * FROM Payments WHERE PaymentId = @PaymentId;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_GetPaymentsByClaimId') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_GetPaymentsByClaimId;
GO

CREATE PROCEDURE sp_GetPaymentsByClaimId
    @ClaimId INT
AS
BEGIN
    SELECT * FROM Payments WHERE ClaimId = @ClaimId ORDER BY PaymentDate DESC;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_InsertPayment') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_InsertPayment;
GO

CREATE PROCEDURE sp_InsertPayment
    @ClaimId INT,
    @PaymentNumber NVARCHAR(50),
    @PaymentAmount DECIMAL(18, 2),
    @PaymentDate DATETIME,
    @PaymentMethod NVARCHAR(20),
    @CheckNumber NVARCHAR(50),
    @EFTTraceNumber NVARCHAR(50),
    @PayeeName NVARCHAR(100),
    @PayeeAddress NVARCHAR(200),
    @PaymentStatus NVARCHAR(20),
    @RemittanceAdviceNumber NVARCHAR(50),
    @CreatedBy NVARCHAR(100),
    @NewPaymentId INT OUTPUT
AS
BEGIN
    INSERT INTO Payments (ClaimId, PaymentNumber, PaymentAmount, PaymentDate, PaymentMethod, CheckNumber,
        EFTTraceNumber, PayeeName, PayeeAddress, PaymentStatus, RemittanceAdviceNumber, CreatedBy)
    VALUES (@ClaimId, @PaymentNumber, @PaymentAmount, @PaymentDate, @PaymentMethod, @CheckNumber,
        @EFTTraceNumber, @PayeeName, @PayeeAddress, @PaymentStatus, @RemittanceAdviceNumber, @CreatedBy);
    
    SET @NewPaymentId = SCOPE_IDENTITY();
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_UpdatePaymentStatus') AND type in (N'P', N'PC'))
    DROP PROCEDURE sp_UpdatePaymentStatus;
GO

CREATE PROCEDURE sp_UpdatePaymentStatus
    @PaymentId INT,
    @PaymentStatus NVARCHAR(20),
    @ClearedDate DATETIME
AS
BEGIN
    UPDATE Payments 
    SET PaymentStatus = @PaymentStatus, ClearedDate = @ClearedDate, ModifiedDate = GETDATE()
    WHERE PaymentId = @PaymentId;
END
GO

-- Insert Sample Data
-- Sample Patients
INSERT INTO Patients (FirstName, LastName, DateOfBirth, SSN, Address, City, State, ZipCode, Phone, Email,
    InsurancePolicyNumber, InsuranceGroupNumber, InsuranceEffectiveDate, IsActive)
VALUES 
    ('John', 'Smith', '1975-03-15', '123-45-6789', '123 Main St', 'Seattle', 'WA', '98101', '(206) 555-0100', 'john.smith@email.com', 'POL-123456', 'GRP-001', '2023-01-01', 1),
    ('Mary', 'Johnson', '1980-07-22', '234-56-7890', '456 Oak Ave', 'Bellevue', 'WA', '98004', '(425) 555-0200', 'mary.j@email.com', 'POL-234567', 'GRP-001', '2023-01-01', 1),
    ('Robert', 'Williams', '1968-11-30', '345-67-8901', '789 Pine Rd', 'Redmond', 'WA', '98052', '(425) 555-0300', 'rwilliams@email.com', 'POL-345678', 'GRP-002', '2023-06-01', 1);

-- Sample Providers
INSERT INTO Providers (Name, NPI, TaxId, Specialty, Address, City, State, ZipCode, Phone, Email,
    IsNetworkProvider, CredentialedDate, IsActive)
VALUES 
    ('Seattle Medical Center', '1234567890', '12-3456789', 'Primary Care', '100 Medical Plaza', 'Seattle', 'WA', '98101', '(206) 555-1000', 'info@seattlemedical.com', 1, '2020-01-01', 1),
    ('Dr. Sarah Chen - Orthopedics', '2345678901', '23-4567890', 'Orthopedic Surgery', '200 Healthcare Dr', 'Bellevue', 'WA', '98004', '(425) 555-2000', 'drchen@ortho.com', 1, '2019-06-15', 1),
    ('Northwest Imaging Center', '3456789012', '34-5678901', 'Radiology', '300 Imaging Blvd', 'Redmond', 'WA', '98052', '(425) 555-3000', 'info@nwimaging.com', 1, '2021-03-01', 1);

-- Sample Claims
DECLARE @PatientId1 INT = (SELECT PatientId FROM Patients WHERE SSN = '123-45-6789');
DECLARE @ProviderId1 INT = (SELECT ProviderId FROM Providers WHERE NPI = '1234567890');

INSERT INTO Claims (ClaimNumber, PatientId, ProviderId, ServiceDateFrom, ServiceDateTo, SubmittedDate, Status,
    PlaceOfService, TotalChargeAmount, CreatedBy)
VALUES 
    ('CLM-2024-00001', @PatientId1, @ProviderId1, '2024-01-15', '2024-01-15', '2024-01-16', 1, 'Office', 250.00, 'System'),
    ('CLM-2024-00002', @PatientId1, @ProviderId1, '2024-02-10', '2024-02-10', '2024-02-11', 4, 'Office', 350.00, 'System');

PRINT 'Database schema created successfully with sample data.';
GO
