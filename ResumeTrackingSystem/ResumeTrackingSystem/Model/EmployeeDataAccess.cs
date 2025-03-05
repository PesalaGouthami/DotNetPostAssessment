using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ResumeTrackingSystem.Model;
using ResumeTrackingSystemAPI.Model;
using System;
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

        public byte[] GeneratePdf(Employee emp)
        {
           
           
                using (var memoryStream = new MemoryStream())
                {
                    
                    using (var writer = new PdfWriter(memoryStream))
                    {
                       
                        using (var pdf = new PdfDocument(writer))
                        {
                            
                            Document document = new Document(pdf);

                          
                            document.Add(new Paragraph("Person Information")
                                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                            .SetFontSize(18));

                        if (emp.ProfilePicture != null && emp.ProfilePicture.Length > 0)
                        {

                            var imageData = iText.IO.Image.ImageDataFactory.Create(emp.ProfilePicture);


                            var image = new Image(imageData);


                            image.SetWidth(100f).SetHeight(100f);


                            document.Add(image);
                        }
                        else
                        {
                            document.Add(new Paragraph("No profile picture available"));


                        }

                        document.Add(new Paragraph($"FirstName: {emp.FirstName}"));
                            document.Add(new Paragraph($"LastName: {emp.LastName}"));
                            document.Add(new Paragraph($"Email: {emp.Email}"));
                            document.Add(new Paragraph($"PhoneNumber: {emp.PhoneNumber}"));
                           document.Add(new Paragraph($"Address: {emp.Address}"));
                           document.Add(new Paragraph($"City: {emp.City}"));
                           document.Add(new Paragraph($"Country: {emp.Country}"));
                        document.Add(new Paragraph($"Skills: {emp.Skills}"));
                        document.Add(new Paragraph($"Years Of Experience: {emp.YearsOfExperience}"));
                          document.Add(new Paragraph($"Date Of Birth: {emp.DateOfBirth}"));
                         
                    

                }
                    }

                    return memoryStream.ToArray();
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
