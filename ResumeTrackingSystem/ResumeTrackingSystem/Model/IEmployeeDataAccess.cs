using System.Collections.Generic;
using ResumeTrackingSystem.Model;

namespace ResumeTrackingSystem.Data
{
    public interface IEmployeeDataAccess
    {
        void AddEmployee(Employee emp);
        void DeleteEmployee(int empId);
        List<Employee> GetAllEmployees();
        Employee GetEmployee(int empId);
        List<Employee> SearchEmployees(string? name, string? email, string? phone, string? skills, int? experience);
        void UpdateEmployee(Employee emp);
    }
}
