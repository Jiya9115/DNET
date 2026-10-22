-- Practical 8 : Inventory Management Module (ADO.NET & CRUD)
-- Optional script. The application creates the same database/table automatically on first run.

IF DB_ID('InventoryDB') IS NULL
    CREATE DATABASE InventoryDB;
GO

USE InventoryDB;
GO

IF OBJECT_ID('dbo.Products', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        ProductId   INT IDENTITY(1,1) PRIMARY KEY,
        ProductName NVARCHAR(100) NOT NULL,
        Category    NVARCHAR(50)  NOT NULL,
        Quantity    INT           NOT NULL CONSTRAINT CK_Products_Quantity CHECK (Quantity >= 0),
        UnitPrice   DECIMAL(10,2) NOT NULL CONSTRAINT CK_Products_UnitPrice CHECK (UnitPrice >= 0),
        AddedOn     DATETIME      NOT NULL CONSTRAINT DF_Products_AddedOn DEFAULT (GETDATE())
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
    INSERT INTO dbo.Products (ProductName, Category, Quantity, UnitPrice) VALUES
    ('Wireless Mouse',      'Electronics', 45,  599.00),
    ('Mechanical Keyboard', 'Electronics', 8,  2499.00),
    ('A4 Paper Ream',       'Stationery',  120, 285.50),
    ('Office Chair',        'Furniture',   4,  5499.00),
    ('USB-C Cable',         'Accessories', 60,  199.00);
END;
GO

SELECT * FROM dbo.Products;
