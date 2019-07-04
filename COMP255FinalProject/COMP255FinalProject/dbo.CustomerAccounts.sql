CREATE TABLE [dbo].[CustomerAccounts] (
    [AccountNumber] INT             NOT NULL,
    [FirstName]     NVARCHAR (50)   NULL,
    [LastName]      NVARCHAR (50)   NULL,
    [Email]         NVARCHAR (50)   NULL,
    [Phone]         NVARCHAR (24)   NULL,
    [Balance]       DECIMAL (19, 4) NULL,
    PRIMARY KEY CLUSTERED ([AccountNumber] ASC)
);

