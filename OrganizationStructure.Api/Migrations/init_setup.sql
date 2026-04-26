-- =============================================
-- Organization Structure Database Setup
-- Version: 1.0
-- Date: 2026-04-26
-- =============================================

USE master;
GO

-- Drop database if exists (for clean setup)
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'OrganizationStructureDb')
BEGIN
    ALTER DATABASE OrganizationStructureDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE OrganizationStructureDb;
END
GO

-- Create database
CREATE DATABASE OrganizationStructureDb;
GO

USE OrganizationStructureDb;
GO

-- =============================================
-- Create Tables
-- =============================================

-- Employees Table
CREATE TABLE [dbo].[Employees] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Title] NVARCHAR(50) NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [Phone] NVARCHAR(20) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL
);
GO

-- Companies Table
CREATE TABLE [dbo].[Companies] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [Code] NVARCHAR(20) NOT NULL,
    [DirectorId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT [FK_Companies_Employees_DirectorId] FOREIGN KEY ([DirectorId]) 
        REFERENCES [Employees]([Id]) ON DELETE NO ACTION
);
GO

-- Divisions Table
CREATE TABLE [dbo].[Divisions] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [Code] NVARCHAR(20) NOT NULL,
    [CompanyId] UNIQUEIDENTIFIER NOT NULL,
    [ManagerId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT [FK_Divisions_Companies_CompanyId] FOREIGN KEY ([CompanyId]) 
        REFERENCES [Companies]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Divisions_Employees_ManagerId] FOREIGN KEY ([ManagerId]) 
        REFERENCES [Employees]([Id]) ON DELETE NO ACTION
);
GO

-- Projects Table
CREATE TABLE [dbo].[Projects] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [Code] NVARCHAR(20) NOT NULL,
    [DivisionId] UNIQUEIDENTIFIER NOT NULL,
    [ManagerId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT [FK_Projects_Divisions_DivisionId] FOREIGN KEY ([DivisionId]) 
        REFERENCES [Divisions]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Projects_Employees_ManagerId] FOREIGN KEY ([ManagerId]) 
        REFERENCES [Employees]([Id]) ON DELETE NO ACTION
);
GO

-- Departments Table
CREATE TABLE [dbo].[Departments] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [Code] NVARCHAR(20) NOT NULL,
    [ProjectId] UNIQUEIDENTIFIER NOT NULL,
    [ManagerId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    CONSTRAINT [FK_Departments_Projects_ProjectId] FOREIGN KEY ([ProjectId]) 
        REFERENCES [Projects]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Departments_Employees_ManagerId] FOREIGN KEY ([ManagerId]) 
        REFERENCES [Employees]([Id]) ON DELETE NO ACTION
);
GO

-- =============================================
-- Create Indexes
-- =============================================

-- Employees Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees_Email] 
    ON [Employees]([Email]);
GO

CREATE NONCLUSTERED INDEX [IX_Employees_LastName_FirstName] 
    ON [Employees]([LastName], [FirstName]);
GO

-- Companies Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_Companies_Code] 
    ON [Companies]([Code]);
GO

CREATE NONCLUSTERED INDEX [IX_Companies_DirectorId] 
    ON [Companies]([DirectorId]);
GO

-- Divisions Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_Divisions_CompanyId_Code] 
    ON [Divisions]([CompanyId], [Code]);
GO

CREATE NONCLUSTERED INDEX [IX_Divisions_ManagerId] 
    ON [Divisions]([ManagerId]);
GO

-- Projects Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_Projects_DivisionId_Code] 
    ON [Projects]([DivisionId], [Code]);
GO

CREATE NONCLUSTERED INDEX [IX_Projects_ManagerId] 
    ON [Projects]([ManagerId]);
GO

-- Departments Indexes
CREATE UNIQUE NONCLUSTERED INDEX [IX_Departments_ProjectId_Code] 
    ON [Departments]([ProjectId], [Code]);
GO

CREATE NONCLUSTERED INDEX [IX_Departments_ManagerId] 
    ON [Departments]([ManagerId]);
GO

-- =============================================
-- Insert Sample Data (Optional)
-- =============================================

-- Insert Sample Employees
DECLARE @Employee1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Employee2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Employee3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Employee4Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO [Employees] ([Id], [Title], [FirstName], [LastName], [Phone], [Email], [CreatedAt])
VALUES 
    (@Employee1Id, 'Ing.', 'Ján', 'Novák', '+421901234567', 'jan.novak@example.com', GETUTCDATE()),
    (@Employee2Id, 'Mgr.', 'Peter', 'Horváth', '+421902345678', 'peter.horvath@example.com', GETUTCDATE()),
    (@Employee3Id, 'Bc.', 'Mária', 'Kováčová', '+421903456789', 'maria.kovacova@example.com', GETUTCDATE()),
    (@Employee4Id, 'Ing.', 'Lucia', 'Tóthová', '+421904567890', 'lucia.tothova@example.com', GETUTCDATE());
GO

-- Insert Sample Company
DECLARE @CompanyId UNIQUEIDENTIFIER = NEWID();
DECLARE @DirectorId UNIQUEIDENTIFIER = (SELECT TOP 1 [Id] FROM [Employees]);

INSERT INTO [Companies] ([Id], [Name], [Code], [DirectorId], [CreatedAt])
VALUES (@CompanyId, 'Tech Solutions s.r.o.', 'TECH-001', @DirectorId, GETUTCDATE());
GO

-- Insert Sample Division
DECLARE @DivisionId UNIQUEIDENTIFIER = NEWID();
DECLARE @CompanyIdVar UNIQUEIDENTIFIER = (SELECT TOP 1 [Id] FROM [Companies]);
DECLARE @ManagerId1 UNIQUEIDENTIFIER = (SELECT [Id] FROM [Employees] WHERE [Email] = 'peter.horvath@example.com');

INSERT INTO [Divisions] ([Id], [Name], [Code], [CompanyId], [ManagerId], [CreatedAt])
VALUES (@DivisionId, 'IT Divízia', 'IT-DIV-001', @CompanyIdVar, @ManagerId1, GETUTCDATE());
GO

-- Insert Sample Project
DECLARE @ProjectId UNIQUEIDENTIFIER = NEWID();
DECLARE @DivisionIdVar UNIQUEIDENTIFIER = (SELECT TOP 1 [Id] FROM [Divisions]);
DECLARE @ManagerId2 UNIQUEIDENTIFIER = (SELECT [Id] FROM [Employees] WHERE [Email] = 'maria.kovacova@example.com');

INSERT INTO [Projects] ([Id], [Name], [Code], [DivisionId], [ManagerId], [CreatedAt])
VALUES (@ProjectId, 'Webová Aplikácia', 'WEB-APP-001', @DivisionIdVar, @ManagerId2, GETUTCDATE());
GO

-- Insert Sample Department
DECLARE @ProjectIdVar UNIQUEIDENTIFIER = (SELECT TOP 1 [Id] FROM [Projects]);
DECLARE @ManagerId3 UNIQUEIDENTIFIER = (SELECT [Id] FROM [Employees] WHERE [Email] = 'lucia.tothova@example.com');

INSERT INTO [Departments] ([Id], [Name], [Code], [ProjectId], [ManagerId], [CreatedAt])
VALUES (NEWID(), 'Frontend Tím', 'FE-TEAM-001', @ProjectIdVar, @ManagerId3, GETUTCDATE());
GO

-- =============================================
-- Verify Database Structure
-- =============================================

PRINT 'Database OrganizationStructureDb created successfully!';
PRINT '';
PRINT 'Tables:';
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;
GO

PRINT '';
PRINT 'Sample Data Count:';
SELECT 'Employees' AS TableName, COUNT(*) AS RecordCount FROM [Employees]
UNION ALL
SELECT 'Companies', COUNT(*) FROM [Companies]
UNION ALL
SELECT 'Divisions', COUNT(*) FROM [Divisions]
UNION ALL
SELECT 'Projects', COUNT(*) FROM [Projects]
UNION ALL
SELECT 'Departments', COUNT(*) FROM [Departments];
GO