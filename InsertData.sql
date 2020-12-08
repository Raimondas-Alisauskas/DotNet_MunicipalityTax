IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Municipality] (
    [Id] uniqueidentifier NOT NULL,
    [MunicipalityName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Municipality] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TaxSchedule] (
    [Id] uniqueidentifier NOT NULL,
    [ScheduleType] int NOT NULL,
    [TaxStartDate] datetime2 NOT NULL,
    [Tax] decimal(18,2) NOT NULL,
    [MunicipalityId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TaxSchedule] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TaxSchedule_Municipality_MunicipalityId] FOREIGN KEY ([MunicipalityId]) REFERENCES [Municipality] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MunicipalityName') AND [object_id] = OBJECT_ID(N'[Municipality]'))
    SET IDENTITY_INSERT [Municipality] ON;
INSERT INTO [Municipality] ([Id], [MunicipalityName])
VALUES ('7ebced2b-e2f9-45e0-bf75-111111111100', N'TestMunicipality');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MunicipalityName') AND [object_id] = OBJECT_ID(N'[Municipality]'))
    SET IDENTITY_INSERT [Municipality] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MunicipalityId', N'ScheduleType', N'Tax', N'TaxStartDate') AND [object_id] = OBJECT_ID(N'[TaxSchedule]'))
    SET IDENTITY_INSERT [TaxSchedule] ON;
INSERT INTO [TaxSchedule] ([Id], [MunicipalityId], [ScheduleType], [Tax], [TaxStartDate])
VALUES ('7ebced2b-e2f9-45e0-bf75-111111111111', '7ebced2b-e2f9-45e0-bf75-111111111100', 0, 0.1, '2016-01-01T00:00:00.0000000'),
('7ebced2b-e2f9-45e0-bf75-111111111112', '7ebced2b-e2f9-45e0-bf75-111111111100', 1, 0.2, '2015-12-28T00:00:00.0000000'),
('7ebced2b-e2f9-45e0-bf75-111111111113', '7ebced2b-e2f9-45e0-bf75-111111111100', 2, 0.3, '2016-01-01T00:00:00.0000000'),
('7ebced2b-e2f9-45e0-bf75-111111111114', '7ebced2b-e2f9-45e0-bf75-111111111100', 3, 0.4, '2016-01-01T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'MunicipalityId', N'ScheduleType', N'Tax', N'TaxStartDate') AND [object_id] = OBJECT_ID(N'[TaxSchedule]'))
    SET IDENTITY_INSERT [TaxSchedule] OFF;
GO

CREATE INDEX [IX_TaxSchedule_MunicipalityId] ON [TaxSchedule] ([MunicipalityId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201205092755_InitialWithData', N'5.0.0');
GO

COMMIT;
GO

