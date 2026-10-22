using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using InventoryManagement.Data;
using InventoryManagement.Models;

namespace InventoryManagement
{
    internal class Program
    {
        private static readonly ProductRepository Repo = new ProductRepository();

        private static void Main()
        {
            Console.Title = "Inventory Management Module - ADO.NET CRUD";

            try
            {
                DbHelper.InitializeDatabase();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not connect to SQL Server: " + ex.Message);
                Console.WriteLine("Set INVENTORY_SERVER_CONN to your server and run again.");
                Console.ReadLine();
                return;
            }

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = (Console.ReadLine() ?? "").Trim();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddProduct(); break;
                        case "2": ViewAll(); break;
                        case "3": ViewById(); break;
                        case "4": UpdateProduct(); break;
                        case "5": DeleteProduct(); break;
                        case "6": SearchProducts(); break;
                        case "7": LowStockReport(); break;
                        case "0": running = false; break;
                        default: Console.WriteLine("Invalid choice. Please select 0-7."); break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Database error: " + ex.Message);
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.Write("Press Enter to continue...");
                    Console.ReadLine();
                    ClearScreen();
                }
            }

            Console.WriteLine("Thank you for using the Inventory Management Module.");
        }

        // ---------- menu ----------

        private static void ShowMenu()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("        INVENTORY MANAGEMENT MODULE       ");
            Console.WriteLine("==========================================");
            Console.WriteLine(" 1. Add New Product      (Create)");
            Console.WriteLine(" 2. View All Products    (Read)");
            Console.WriteLine(" 3. View Product By Id   (Read)");
            Console.WriteLine(" 4. Update Product       (Update)");
            Console.WriteLine(" 5. Delete Product       (Delete)");
            Console.WriteLine(" 6. Search Products");
            Console.WriteLine(" 7. Low Stock Report");
            Console.WriteLine(" 0. Exit");
            Console.WriteLine("------------------------------------------");
            Console.Write("Enter your choice: ");
        }

        // ---------- operations ----------

        private static void AddProduct()
        {
            Console.WriteLine("--- Add New Product ---");
            Product p = new Product
            {
                ProductName = ReadText("Product name : "),
                Category = ReadText("Category     : "),
                Quantity = ReadInt("Quantity     : ", 0),
                UnitPrice = ReadDecimal("Unit price   : ", 0)
            };

            int newId = Repo.Add(p);
            Console.WriteLine();
            Console.WriteLine("Product added successfully. New ProductId = " + newId);
        }

        private static void ViewAll()
        {
            Console.WriteLine("--- All Products ---");
            PrintTable(Repo.GetAll());
        }

        private static void ViewById()
        {
            int id = ReadInt("Enter ProductId: ", 1);
            Product p = Repo.GetById(id);
            if (p == null)
            {
                Console.WriteLine("No product found with Id " + id);
                return;
            }
            PrintTable(new List<Product> { p });
        }

        private static void UpdateProduct()
        {
            int id = ReadInt("Enter ProductId to update: ", 1);
            Product p = Repo.GetById(id);
            if (p == null)
            {
                Console.WriteLine("No product found with Id " + id);
                return;
            }

            Console.WriteLine("Current details:");
            PrintTable(new List<Product> { p });
            Console.WriteLine("Press Enter to keep the current value.");

            string name = ReadOptional("Product name [" + p.ProductName + "]: ");
            if (name != "") p.ProductName = name;

            string category = ReadOptional("Category [" + p.Category + "]: ");
            if (category != "") p.Category = category;

            p.Quantity = ReadOptionalInt("Quantity [" + p.Quantity + "]: ", p.Quantity);
            p.UnitPrice = ReadOptionalDecimal("Unit price [" + p.UnitPrice.ToString("0.00") + "]: ", p.UnitPrice);

            Console.WriteLine();
            Console.WriteLine(Repo.Update(p) ? "Product updated successfully." : "Update failed.");
        }

        private static void DeleteProduct()
        {
            int id = ReadInt("Enter ProductId to delete: ", 1);
            Product p = Repo.GetById(id);
            if (p == null)
            {
                Console.WriteLine("No product found with Id " + id);
                return;
            }

            PrintTable(new List<Product> { p });
            Console.Write("Are you sure you want to delete this product? (Y/N): ");
            string answer = (Console.ReadLine() ?? "").Trim().ToUpper();
            if (answer != "Y")
            {
                Console.WriteLine("Delete cancelled.");
                return;
            }

            Console.WriteLine(Repo.Delete(id) ? "Product deleted successfully." : "Delete failed.");
        }

        private static void SearchProducts()
        {
            string keyword = ReadText("Enter name or category to search: ");
            Console.WriteLine();
            PrintTable(Repo.Search(keyword));
        }

        private static void LowStockReport()
        {
            int limit = ReadInt("Show products with quantity at or below: ", 0);
            Console.WriteLine();
            PrintTable(Repo.GetLowStock(limit));
        }

        // ---------- display ----------

        private static void PrintTable(List<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            string line = new string('-', 82);
            Console.WriteLine(line);
            Console.WriteLine("{0,-5}{1,-26}{2,-15}{3,8}{4,14}  {5,-12}",
                "Id", "Product Name", "Category", "Qty", "Unit Price", "Added On");
            Console.WriteLine(line);

            decimal totalValue = 0;
            foreach (Product p in products)
            {
                Console.WriteLine("{0,-5}{1,-26}{2,-15}{3,8}{4,14:N2}  {5,-12:dd-MM-yyyy}",
                    p.ProductId, Trim(p.ProductName, 25), Trim(p.Category, 14),
                    p.Quantity, p.UnitPrice, p.AddedOn);
                totalValue += p.Quantity * p.UnitPrice;
            }

            Console.WriteLine(line);
            Console.WriteLine("Records: {0}    Total stock value: Rs. {1:N2}", products.Count, totalValue);
        }

        private static string Trim(string text, int max)
        {
            return text.Length <= max ? text : text.Substring(0, max - 1) + "~";
        }

        private static void ClearScreen()
        {
            try { Console.Clear(); }
            catch (IOException) { /* output is redirected, nothing to clear */ }
        }

        // ---------- input helpers ----------

        private static string ReadText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = (Console.ReadLine() ?? "").Trim();
                if (value.Length > 0) return value;
                Console.WriteLine("Value cannot be empty.");
            }
        }

        private static string ReadOptional(string prompt)
        {
            Console.Write(prompt);
            return (Console.ReadLine() ?? "").Trim();
        }

        private static int ReadInt(string prompt, int min)
        {
            while (true)
            {
                Console.Write(prompt);
                int value;
                if (int.TryParse(Console.ReadLine(), out value) && value >= min) return value;
                Console.WriteLine("Enter a whole number >= " + min + ".");
            }
        }

        private static decimal ReadDecimal(string prompt, decimal min)
        {
            while (true)
            {
                Console.Write(prompt);
                decimal value;
                if (decimal.TryParse(Console.ReadLine(), out value) && value >= min) return value;
                Console.WriteLine("Enter a valid amount >= " + min + ".");
            }
        }

        private static int ReadOptionalInt(string prompt, int current)
        {
            while (true)
            {
                string text = ReadOptional(prompt);
                if (text == "") return current;
                int value;
                if (int.TryParse(text, out value) && value >= 0) return value;
                Console.WriteLine("Enter a whole number >= 0.");
            }
        }

        private static decimal ReadOptionalDecimal(string prompt, decimal current)
        {
            while (true)
            {
                string text = ReadOptional(prompt);
                if (text == "") return current;
                decimal value;
                if (decimal.TryParse(text, out value) && value >= 0) return value;
                Console.WriteLine("Enter a valid amount >= 0.");
            }
        }
    }
}
