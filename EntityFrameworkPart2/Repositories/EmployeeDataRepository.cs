using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkPart2.Models;
using EntityFrameworkPart2.Data;

namespace EntityFrameworkPart2.Repositories
{
    public class EmployeeDataRepository
    {
        private static RecruitmentContext Context { get; set; }
        private static EmployeeDataRepository INSTANCE { get; set; }
        private EmployeeDataRepository(RecruitmentContext context)
        {
            Context = context;
        }

        public static EmployeeDataRepository Instance(RecruitmentContext context)
        {
            if( INSTANCE == null)
            {
                INSTANCE = new EmployeeDataRepository(context);
            }
            return INSTANCE;
        }

        public Employee GetEmployeeByCode(string employeeCode)
        {
            var employee = Context.Employees
                .Where(e => e.CEmployeeCode.Equals(employeeCode))
                .FirstOrDefault();
            if(employee == null)
            {
                throw new Exception("Employee Not found");
            }
            return employee;
        }
        public EmployeeData GetEmployeeData(string employeeCode)
        {
            EmployeeData employeeData = new EmployeeData();
            var employee = Context.Employees
            .Join(Context.Positions,
            e => e.CCurrentPosition,
            p => p.CPositionCode,
            (e, p) => new
            {
                EmployeeCode = e.CEmployeeCode,
                EmployeeFirstName = e.VFirstName,
                EmployeeLastName = e.VLastName,
                EmployeeBirthday = e.DBirthDate,
                EmployeePosition = p.VDescription
            })
            .Where(e => e.EmployeeCode.Equals(employeeCode))
            .FirstOrDefault();
            employeeData.EmployeeCode = employee.EmployeeCode;
            employeeData.FirstName = employee.EmployeeFirstName;
            employeeData.LastName = employee.EmployeeLastName;
            employeeData.Birthday = (DateTime)employee.EmployeeBirthday;
            employeeData.Position = employee.EmployeePosition;

            return employeeData;
        }
        public List<MonthlySalary> GetMonthlySalary(string employeeCode)
        {
            var monthlySalaries = Context.MonthlySalaries
                .Select(e => e)
                .Where(e => e.CEmployeeCode.Equals(employeeCode))
                .ToList();

            return monthlySalaries;
        }
        public List<AnnualSalary> GetAnnualSalary(string employeeCode)
        {
            var annualSalaries = Context.AnnualSalaries
                .Select(e => e)
                .Where(e => e.CEmployeeCode.Equals(employeeCode))
                .ToList();

            return annualSalaries;
        }
        public List<string> GetEmployeeSkills(string employeeCode)
        {
            var skills = Context.EmployeeSkills
                .Join(Context.Skills,
                es => es.CSkillCode,
                s => s.CSkillCode,
                (es, s) => new
                {
                    EmployeeCode = es.CEmployeeCode,
                    SkillCode = es.CSkillCode,
                    Skill = s.VSkill
                })
                .Where(e => e.EmployeeCode.Equals(employeeCode))
                .ToList();
            
            List<string> skillList = new List<string>();
            foreach (var skill in skills)
            {
                skillList.Add(skill.Skill);
            }

            return skillList;
        }
    }
}
