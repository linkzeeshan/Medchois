CREATE TABLE [dbo].[Setting] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (150) NULL,
    [Value]     NVARCHAR (MAX) NULL,
    [CompanyId] INT            NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([Id] ASC)
);

