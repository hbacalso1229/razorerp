USE [master]
GO

/****** Object: Table [dbo].[User] Script Date: 4/09/2024 6:38:41 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Email]          VARCHAR (100)    NOT NULL,
    [FirstName]      VARCHAR (50)     NOT NULL,
    [LastName]       VARCHAR (50)     NOT NULL,
    [Username]       VARCHAR (50)     NOT NULL,
    [Password]       VARCHAR (250)    NOT NULL,
    [CompanyId]      UNIQUEIDENTIFIER NOT NULL,
    [Role]           VARCHAR (50)     NOT NULL,
    [Created]        DATETIME2 (7)    NOT NULL,
    [CreatedBy]      VARCHAR (50)     NULL,
    [LastModified]   DATETIME2 (7)    NULL,
    [LastModifiedBy] VARCHAR (50)     NULL
);
