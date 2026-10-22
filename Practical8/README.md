# Practical 8 - Inventory Management Module (ADO.NET & CRUD Operations)

A menu-driven C# console application that manages product inventory in SQL Server using
**ADO.NET** (`SqlConnection`, `SqlCommand`, `SqlDataReader`, parameterized queries).

Course: 01CE1523 - .NET Technologies

## Features
- **Create** - add a new product
- **Read** - view all products, view by Id, search by name/category, low-stock report
- **Update** - edit a product (press Enter to keep a value)
- **Delete** - remove a product with confirmation

## Project structure
```
Practical8/
|-- InventoryManagement.sln
|-- Database/InventoryDB.sql          (optional SQL script)
|-- Screenshots/                      (output screenshots)
`-- InventoryManagement/
    |-- InventoryManagement.csproj    (.NET Framework 4.8)
    |-- Program.cs                    (console menu / UI)
    |-- Models/Product.cs             (entity)
    `-- Data/
        |-- DbHelper.cs               (connection + database setup)
        `-- ProductRepository.cs      (CRUD using ADO.NET)
```

## Requirements
- Visual Studio 2019/2022 with **.NET desktop development** workload (includes SQL Server LocalDB)
- .NET Framework 4.8

## How to run
1. Open `InventoryManagement.sln` in Visual Studio.
2. Press **F5** (or Ctrl+F5). The `InventoryDB` database, `Products` table and sample rows are
   created automatically on first run using LocalDB `(localdb)\MSSQLLocalDB`.
3. Use the menu (1-7) to perform the operations.

### Using SQL Server Express / another server
Set the environment variable `INVENTORY_SERVER_CONN` before running, for example:
```
Server=.\SQLEXPRESS;Integrated Security=true;
```

## Database schema
| Column      | Type          | Notes                      |
|-------------|---------------|----------------------------|
| ProductId   | INT IDENTITY  | Primary key                |
| ProductName | NVARCHAR(100) | Required                   |
| Category    | NVARCHAR(50)  | Required                   |
| Quantity    | INT           | Must be >= 0               |
| UnitPrice   | DECIMAL(10,2) | Must be >= 0               |
| AddedOn     | DATETIME      | Defaults to current date   |
