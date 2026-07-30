using System;

class Program
{
    static void Main(string[] args)
    {
        // Array to store expense amounts
        double[] amount = new double[10];

        // Array to store expense categories
        string[] category = new string[10];

        // Variable to keep track of the number of expenses
        int count = 0;

        // Infinite loop to display the menu until the user exits
        while (true)
        {
            // Display menu options
            Console.WriteLine("\n===== Expense Tracker =====");
            Console.WriteLine("1. Add Expense");
            Console.WriteLine("2. View Expenses");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            // Read user's menu choice
            string choice = Console.ReadLine();

            try
            {
                // Execute the selected menu option
                switch (choice)
                {
                    case "1":

                        // Check if the arrays are full
                        if (count >= amount.Length)
                        {
                            throw new Exception("Expense storage is full.");
                        }

                        // Read expense amount from the user
                        Console.Write("Enter Expense Amount: ");
                        double expenseAmount = Convert.ToDouble(Console.ReadLine());

                        // Check if the entered amount is valid
                        if (expenseAmount <= 0)
                        {
                            throw new Exception("Amount must be greater than zero.");
                        }

                        // Read expense category
                        Console.Write("Enter Expense Category: ");
                        string expenseCategory = Console.ReadLine();

                        // Store expense details in arrays
                        amount[count] = expenseAmount;
                        category[count] = expenseCategory;

                        // Increase expense count
                        count++;

                        // Display success message
                        Console.WriteLine("Expense added successfully.");
                        break;

                    case "2":

                        // Check if no expenses have been added
                        if (count == 0)
                        {
                            Console.WriteLine("No expenses recorded.");
                        }
                        else
                        {
                            // Variable to calculate total expenses
                            double total = 0;

                            Console.WriteLine("\nExpense List:");

                            // Display all stored expenses
                            for (int i = 0; i < count; i++)
                            {
                                Console.WriteLine("Category: " + category[i] +
                                                  " | Amount: " + amount[i]);

                                // Add each expense to the total
                                total += amount[i];
                            }

                            // Display total expense amount
                            Console.WriteLine("Total Expenses: " + total);
                        }
                        break;

                    case "3":

                        // Exit the program
                        Console.WriteLine("Exiting...");
                        return;

                    default:

                        // Throw an exception for an invalid menu option
                        throw new Exception("Invalid menu choice.");
                }
            }

            // Handle invalid numeric input
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter a numeric value.");
            }

            // Handle all other exceptions
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
