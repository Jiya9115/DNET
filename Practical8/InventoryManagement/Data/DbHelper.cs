using System;
using System.Data.SqlClient;

namespace InventoryManagement.Data
{
    /// <summary>
    /// Creates SqlConnection objects and prepares the InventoryDB database on first run.
    /// Default server is SQL Server LocalDB (installed with Visual Studio).
    /// To use another server, set the environment variable INVENTORY_SERVER_CONN, e.g.
    /// Server=.\SQLEXPRESS;Integrated Security=true;
    /// </summary>
    public static class DbHelper
    {
        private const string DatabaseName = "InventoryDB";

        private static readonly string ServerConnection =
            Environment.GetEnvironmentVariable("INVENTORY_SERVER_CONN")
            ?? @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;";

        private static string BuildConnection(string database)
        {
            return ServerConnection.TrimEnd(';') + ";Database=" + database + ";";
        }

        public static string ConnectionString
        {
            get { return BuildConnection(DatabaseName); }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Creates the database, the Products table and a few sample rows if they do not exist.
        /// </summary>
        public static void InitializeDatabase()
        {
            using (SqlConnection master = new SqlConnection(BuildConnection("master")))
            {
                master.Open();
                const string createDb =
                    "IF DB_ID('InventoryDB') IS NULL CREATE DATABASE InventoryDB;";
                using (SqlCommand cmd = new SqlCommand(createDb, master))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            const string createTable = @"
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

IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
    INSERT INTO dbo.Products (ProductName, Category, Quantity, UnitPrice) VALUES
    ('Wireless Mouse',      'Electronics', 45,  599.00),
    ('Mechanical Keyboard', 'Electronics', 8,  2499.00),
    ('A4 Paper Ream',       'Stationery',  120, 285.50),
    ('Office Chair',        'Furniture',   4,  5499.00),
    ('USB-C Cable',         'Accessories', 60,  199.00);
END;";

            using (SqlConnection con = GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(createTable, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
