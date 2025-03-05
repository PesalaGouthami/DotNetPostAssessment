using ResumeTrackingSystem.Model;
using ResumeTrackingSystemAPI.Model;
using System.Collections.Generic;
using System.Linq;

namespace ResumeTrackingSystem.Data
{
    public class EmployeeDataAccess : IEmployeeDataAccess
    {
        private readonly EmployeeDbContext _employeeDb;

        public EmployeeDataAccess(EmployeeDbContext employeeDb)
        {
            _employeeDb = employeeDb;
        }

        public void AddEmployee(Employee emp)
        {
            _employeeDb.Employees.Add(emp);
            _employeeDb.SaveChanges();
        }

        public void DeleteEmployee(int empId)
        {
            var result = _employeeDb.Employees.Find(empId);
            if (result != null)
            {
                _employeeDb.Employees.Remove(result);
                _employeeDb.SaveChanges();
            }
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeDb.Employees.ToList();
        }

        public Employee GetEmployee(int empId)
        {
            var result = _employeeDb.Employees.Find(empId);
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public List<Employee> SearchEmployees(string? name, string? email, string? phone, string? skills, int? experience)
        {
            var query = _employeeDb.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(e => e.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(e => e.PhoneNumber.Contains(phone));
            }
            if (!string.IsNullOrEmpty(skills))
            {
                query = query.Where(e => e.Skills.Contains(skills));
            }
            if (experience.HasValue)
            {
                query = query.Where(e => e.YearsOfExperience == experience.Value);
            }

            return query.ToList();
        }

        public void UpdateEmployee(Employee emp)
        {
            _employeeDb.Employees.Update(emp);
            _employeeDb.SaveChanges();
        }
    }
}
