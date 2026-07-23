using System;

namespace EmployeePayrollSystem
{
    // Interface
    interface IPayable
    {
        double CalculateSalary();
    }

    // Base Class
    class Employee : IPayable
    {
        // Static variable to generate Employee ID automatically
        private static int NextID = 1001;

        // Employee Details
        public int EmployeeID { get; }
        public string Name { get; set; }
        public int Age { get; set; }

        // Constructor
        public Employee(string name, int age)
        {
            EmployeeID = NextID++;
            Name = name;
            Age = age;
        }

        // Virtual Method
        public virtual double CalculateSalary()
        {
            return 0;
        }

        // Display Employee Details
        public virtual void Display()
        {
            Console.WriteLine("\n-----------------------------");
            Console.WriteLine("Employee ID   : " + EmployeeID);
            Console.WriteLine("Name          : " + Name);
            Console.WriteLine("Age           : " + Age);
        }
    }

    // Full Time Employee
    class FullTimeEmployee : Employee
    {
        public double BasicSalary;
        public double Increment;

        public FullTimeEmployee(string name, int age, double salary, double increment)
            : base(name, age)
        {
            BasicSalary = salary;
            Increment = increment;
        }

        // Override Method
        public override double CalculateSalary()
        {
            return BasicSalary + (BasicSalary * Increment / 100);
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Employee Type : Full Time");
            Console.WriteLine("Basic Salary  : " + BasicSalary);
            Console.WriteLine("Increment (%) : " + Increment);
            Console.WriteLine("Net Salary    : " + CalculateSalary());
        }
    }

    // Part Time Employee
    class PartTimeEmployee : Employee
    {
        public int HoursWorked;
        public double HourlyRate;

        public PartTimeEmployee(string name, int age, int hours, double rate)
            : base(name, age)
        {
            HoursWorked = hours;
            HourlyRate = rate;
        }

        public override double CalculateSalary()
        {
            return HoursWorked * HourlyRate;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Employee Type : Part Time");
            Console.WriteLine("Hours Worked  : " + HoursWorked);
            Console.WriteLine("Hourly Rate   : " + HourlyRate);
            Console.WriteLine("Net Salary    : " + CalculateSalary());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Employee emp;

            Console.WriteLine("====== Employee Payroll System ======");

            Console.Write("Enter Employee Name : ");
            string name = Console.ReadLine();

            Console.Write("Enter Age : ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nSelect Employee Type");
            Console.WriteLine("1. Full Time");
            Console.WriteLine("2. Part Time");
            Console.Write("Enter Choice : ");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                Console.Write("Enter Basic Salary : ");
                double salary = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter Performance Rating (1-5) : ");
                int rating = Convert.ToInt32(Console.ReadLine());

                double increment = 0;

                // Increment based on Performance
                switch (rating)
                {
                    case 5:
                        increment = 20;
                        break;
                    case 4:
                        increment = 15;
                        break;
                    case 3:
                        increment = 10;
                        break;
                    case 2:
                        increment = 5;
                        break;
                    default:
                        increment = 0;
                        break;
                }

                emp = new FullTimeEmployee(name, age, salary, increment);
            }
            else
            {
                Console.Write("Enter Hours Worked : ");
                int hours = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter Hourly Rate : ");
                double rate = Convert.ToDouble(Console.ReadLine());

                emp = new PartTimeEmployee(name, age, hours, rate);
            }

            Console.WriteLine("\n===== Payroll Details =====");
            emp.Display();

            Console.ReadKey();
        }
    }
}
