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
        // Data Members
        public int EmployeeID;
        public string Name;

        // Constructor
        public Employee(int id, string name)
        {
            EmployeeID = id;
            Name = name;
        }

        // Virtual Method (Polymorphism)
        public virtual double CalculateSalary()
        {
            return 0;
        }

        // Display Employee Details
        public virtual void Display()
        {
            Console.WriteLine("Employee ID : " + EmployeeID);
            Console.WriteLine("Employee Name : " + Name);
        }
    }

    // Derived Class - Full Time Employee
    class FullTimeEmployee : Employee
    {
        public double MonthlySalary;

        public FullTimeEmployee(int id, string name, double salary)
            : base(id, name)
        {
            MonthlySalary = salary;
        }

        // Method Overriding
        public override double CalculateSalary()
        {
            return MonthlySalary;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Employee Type : Full Time");
            Console.WriteLine("Salary : " + CalculateSalary());
        }
    }

    // Derived Class - Part Time Employee
    class PartTimeEmployee : Employee
    {
        public int HoursWorked;
        public double HourlyRate;

        public PartTimeEmployee(int id, string name, int hours, double rate)
            : base(id, name)
        {
            HoursWorked = hours;
            HourlyRate = rate;
        }

        // Method Overriding
        public override double CalculateSalary()
        {
            return HoursWorked * HourlyRate;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Employee Type : Part Time");
            Console.WriteLine("Hours Worked : " + HoursWorked);
            Console.WriteLine("Hourly Rate : " + HourlyRate);
            Console.WriteLine("Salary : " + CalculateSalary());
        }
    }

    // Main Class
    class Program
    {
        static void Main(string[] args)
        {
            // Polymorphism
            Employee emp1 = new FullTimeEmployee(101, "Jiya", 50000);
            Employee emp2 = new PartTimeEmployee(102, "Rahul", 120, 250);

            Console.WriteLine("===== Employee Payroll System =====\n");

            emp1.Display();

            Console.WriteLine();

            emp2.Display();

            Console.ReadKey();
        }
    }
}
