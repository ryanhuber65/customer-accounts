CREATE TABLE [dbo].[ AccountTransactions]
(
    [TransactionNumber] INT NOT NULL , 
    [AccountNumber] INT NOT NULL, 
    [TransactionDate] DATETIME NULL, 
    [TransactionAmount] DECIMAL(19, 4) NULL, 
    CONSTRAINT [PK_ AccountTransactions] PRIMARY KEY ([TransactionNumber]), 
    CONSTRAINT [FK_ AccountTransactions_ToTable] FOREIGN KEY ([AccountNumber]) REFERENCES [CustomerAccounts]([AccountNumber]) 
)

