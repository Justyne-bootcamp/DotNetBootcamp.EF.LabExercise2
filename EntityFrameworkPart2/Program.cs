using System;
using EntityFrameworkPart2.Data;
using EntityFrameworkPart2.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkPart2.Repositories;
using System.Collections.Generic;

namespace EntityFrameworkPart2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConfigurationHelper configuration = ConfigurationHelper.Instance();
            var dbConnection = configuration.GetProperty<string>("DbConnectionString");
            Console.WriteLine(dbConnection);

            Console.WriteLine("Please Enter Employee Code");
            string employeeCode = Console.ReadLine();
            //000001
            using (RecruitmentContext context = new RecruitmentContext(dbConnection))
            {
                EmployeeDataRepository employeeDataRepository = EmployeeDataRepository.Instance(context);
                try
                {
                    Employee verifiedEmployee = employeeDataRepository.GetEmployeeByCode(employeeCode);
                    EmployeeData employeeData = employeeDataRepository.GetEmployeeData(verifiedEmployee.CEmployeeCode);

                    Console.WriteLine($"Employee Code: {employeeData.EmployeeCode}");
                    Console.WriteLine($"Employee Name: {employeeData.FirstName} {employeeData.LastName}");
                    Console.WriteLine($"Birthday: {employeeData.Birthday}");
                    Console.WriteLine($"Position: {employeeData.Position}");

                    List<MonthlySalary> monthlySalaries = employeeDataRepository.GetMonthlySalary(verifiedEmployee.CEmployeeCode);

                    Console.WriteLine("\nMonthly Salary\n");
                    foreach (var salary in monthlySalaries)
                    {
                        Console.WriteLine(salary.MMonthlySalary);
                    }

                    List<AnnualSalary> annualSalaries = employeeDataRepository.GetAnnualSalary(verifiedEmployee.CEmployeeCode);
                    Console.WriteLine("\nAnnual Salary\n");
                    foreach (var salary in annualSalaries)
                    {
                        Console.WriteLine(salary.MAnnualSalary);
                    }

                    List<string> skills = employeeDataRepository.GetEmployeeSkills(verifiedEmployee.CEmployeeCode);

                    Console.WriteLine("\nSkills\n");
                    foreach(var skill in skills)
                    {
                        Console.WriteLine(skill);
                    }

                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
