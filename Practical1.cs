using System;

class Student
{
    string Name, Email, Branch, GRNo, EnrollmentNo;
    int Age;
    double Percentage, TuitionFee;

    bool TransportTaken;

    const double FormFee = 500, IdCardFee = 300, UniformFee = 2000,
                  ExamFee = 2500, TransportFee = 15000;

    public Student(string name, string email, int age, string branch, double percentage, bool transport)
    {
        Name = name;
        Email = email;
        Age = age;
        Branch = branch.ToUpper();
        Percentage = percentage;
        TransportTaken = transport;
    }

    public bool CheckEligibility()
    {
        bool eligible = false;

        switch (Branch)
        {
            case "ICT": TuitionFee = 140000; eligible = Percentage >= 70; break;
            case "CE": TuitionFee = 130000; eligible = Percentage >= 65; break;
            case "CSE": TuitionFee = 135000; eligible = Percentage >= 68; break;
            case "IT": TuitionFee = 120000; eligible = Percentage >= 70; break;
            case "EC": TuitionFee = 110000; eligible = Percentage >= 75; break;
            case "ME": TuitionFee = 90000; eligible = Percentage >= 70; break;
            case "ELECTRICAL": TuitionFee = 95000; eligible = Percentage >= 75; break;
            case "CIVIL": TuitionFee = 85000; eligible = Percentage >= 65; break;
            default:
                Console.WriteLine("Invalid Branch!");
                return false;
        }

        Console.WriteLine(eligible
            ? "\nCongratulations! You are Eligible."
            : "\nSorry! You are NOT Eligible.");

        return eligible;
    }

    public void CalculateFees()
    {
        double scholarship = Percentage >= 90 ? 50 :
                             Percentage >= 80 ? 30 :
                             Percentage >= 70 ? 20 : 0;

        double finalFee = TuitionFee - TuitionFee * scholarship / 100;

        double total = finalFee + FormFee + IdCardFee + UniformFee + ExamFee +
                       (TransportTaken ? TransportFee : 0);

        Console.WriteLine("\n----- Fee Structure -----");
        Console.WriteLine($"Form Fee              : {FormFee}");
        Console.WriteLine($"Id Card Fee           : {IdCardFee}");
        Console.WriteLine($"Uniform Fee           : {UniformFee}");
        Console.WriteLine($"Exam Fee              : {ExamFee}");
        Console.WriteLine($"Tuition Fee           : {TuitionFee}");
        Console.WriteLine($"Scholarship           : {scholarship}%");
        Console.WriteLine($"Final Tuition Fee     : {finalFee}");
        Console.WriteLine($"Transport Fee         : {(TransportTaken ? TransportFee : 0)}");
        Console.WriteLine($"Total Fees            : {total}");
    }

    public void ConfirmAdmission()
    {
        Console.Write("\nConfirm Admission (Yes/No): ");
        if (Console.ReadLine().ToLower() != "yes")
        {
            Console.WriteLine("Admission Cancelled.");
            return;
        }

        Random r = new Random();
        GRNo = "GR2026" + r.Next(1000, 9999);
        EnrollmentNo = "926" + Branch + r.Next(1000, 9999);

        Console.WriteLine("\nAdmission Successful!");
        Console.WriteLine($"Name        : {Name}");
        Console.WriteLine($"Email       : {Email}");
        Console.WriteLine($"Age         : {Age}");
        Console.WriteLine($"Branch      : {Branch}");
        Console.WriteLine($"Percentage  : {Percentage}%");
        Console.WriteLine($"GR No       : {GRNo}");
        Console.WriteLine($"Enrollment  : {EnrollmentNo}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("STUDENT ADMISSION MANAGEMENT SYSTEM\n");

        Console.Write("Name: ");
        string name = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        Console.Write("Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("12th Percentage: ");
        double per = Convert.ToDouble(Console.ReadLine());

        Console.Write("Branch (ICT/CE/CSE/IT/EC/ME/Electrical/Civil): ");
        string branch = Console.ReadLine();

        Console.Write("Transport Required (Yes/No): ");
        bool transport = Console.ReadLine().ToLower() == "yes";

        Student s = new Student(name, email, age, branch, per, transport);

        if (s.CheckEligibility())
        {
            s.CalculateFees();
            s.ConfirmAdmission();
        }

        Console.WriteLine("\nThank You...");
        Console.ReadLine();
    }
}
