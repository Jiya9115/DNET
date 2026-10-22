using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InventoryManagement.Models;

namespace InventoryManagement.Data
{
    /// <summary>
    /// CRUD operations on the Products table using ADO.NET
    /// (SqlConnection, SqlCommand, SqlDataReader and parameterized queries).
    /// </summary>
    public class ProductRepository
    {
        // CREATE
        public int Add(Product product)
        {
            const string sql = @"
INSERT INTO dbo.Products (ProductName, Category, Quantity, UnitPrice)
VALUES (@Name, @Category, @Quantity, @UnitPrice);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection con = DbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                AddParameters(cmd, product);
                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        // READ (all records)
        public List<Product> GetAll()
        {
            const string sql =
                "SELECT ProductId, ProductName, Category, Quantity, UnitPrice, AddedOn " +
                "FROM dbo.Products ORDER BY ProductId;";
            return ReadList(sql, null);
        }

        // READ (single record)
        public Product GetById(int id)
        {
            const string sql =
                "SELECT ProductId, ProductName, Category, Quantity, UnitPrice, AddedOn " +
                "FROM dbo.Products WHERE ProductId = @Id;";

            using (SqlConnection con = DbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? Map(reader) : null;
                }
            }
        }

        // READ (search by name or category)
        public List<Product> Search(string keyword)
        {
            const string sql =
                "SELECT ProductId, ProductName, Category, Quantity, UnitPrice, AddedOn " +
                "FROM dbo.Products " +
                "WHERE ProductName LIKE @Key OR Category LIKE @Key ORDER BY ProductName;";
            SqlParameter key = new SqlParameter("@Key", SqlDbType.NVarChar, 100);
            key.Value = "%" + keyword + "%";
            return ReadList(sql, key);
        }

        // READ (stock report)
        public List<Product> GetLowStock(int threshold)
        {
            const string sql =
                "SELECT ProductId, ProductName, Category, Quantity, UnitPrice, AddedOn " +
                "FROM dbo.Products WHERE Quantity <= @Threshold ORDER BY Quantity;";
            SqlParameter limit = new SqlParameter("@Threshold", SqlDbType.Int);
            limit.Value = threshold;
            return ReadList(sql, limit);
        }

        // UPDATE
        public bool Update(Product product)
        {
            const string sql = @"
UPDATE dbo.Products
SET ProductName = @Name, Category = @Category, Quantity = @Quantity, UnitPrice = @UnitPrice
WHERE ProductId = @Id;";

            using (SqlConnection con = DbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                AddParameters(cmd, product);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = product.ProductId;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // DELETE
        public bool Delete(int id)
        {
            const string sql = "DELETE FROM dbo.Products WHERE ProductId = @Id;";

            using (SqlConnection con = DbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ---------- helpers ----------

        private static void AddParameters(SqlCommand cmd, Product p)
        {
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = p.ProductName;
            cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 50).Value = p.Category;
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = p.Quantity;
            SqlParameter price = cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal);
            price.Precision = 10;
            price.Scale = 2;
            price.Value = p.UnitPrice;
        }

        private static List<Product> ReadList(string sql, SqlParameter parameter)
        {
            List<Product> list = new List<Product>();

            using (SqlConnection con = DbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (parameter != null)
                {
                    cmd.Parameters.Add(parameter);
                }
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Map(reader));
                    }
                }
            }
            return list;
        }

        private static Product Map(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Category = reader.GetString(2),
                Quantity = reader.GetInt32(3),
                UnitPrice = reader.GetDecimal(4),
                AddedOn = reader.GetDateTime(5)
            };
        }
    }
}
