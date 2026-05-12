CREATE TABLE dbo.IncomingOrders
(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_IncomingOrders PRIMARY KEY,
    CustomerId NVARCHAR(100) NOT NULL,
    OrderNumber NVARCHAR(100) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Currency NVARCHAR(3) NOT NULL,
    OriginalMessage NVARCHAR(MAX) NULL,
    RequestedAtUtc DATETIME2 NOT NULL,
    SavedAtUtc DATETIME2 NOT NULL
);
GO

CREATE PROCEDURE dbo.SaveIncomingOrder
    @CustomerId NVARCHAR(100),
    @OrderNumber NVARCHAR(100),
    @Amount DECIMAL(18, 2),
    @Currency NVARCHAR(3),
    @OriginalMessage NVARCHAR(MAX) = NULL,
    @RequestedAtUtc DATETIME2,
    @SavedAtUtc DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.IncomingOrders
    (
        CustomerId,
        OrderNumber,
        Amount,
        Currency,
        OriginalMessage,
        RequestedAtUtc,
        SavedAtUtc
    )
    VALUES
    (
        @CustomerId,
        @OrderNumber,
        @Amount,
        UPPER(@Currency),
        @OriginalMessage,
        @RequestedAtUtc,
        @SavedAtUtc
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
