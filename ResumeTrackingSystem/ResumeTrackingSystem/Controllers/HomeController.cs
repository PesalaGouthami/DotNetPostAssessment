using System;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeTrackingSystem.Data;
using ResumeTrackingSystem.Model;
using ResumeTrackingSystemAPI.Model;



[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IEmployeeDataAccess _employeeDataAccess;
    private readonly IConverter _converter;
    private readonly EmployeeDbContext employeeDb;

    public HomeController(IEmployeeDataAccess employeeDataAccess, IConverter converter, EmployeeDbContext employeeDb)
    {
        _employeeDataAccess = employeeDataAccess;
        _converter = converter;
        this.employeeDb = employeeDb;
    }
    [HttpPost("Create")]

    
    public IActionResult CreateEmployee([FromForm] Employee employeeDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
            }
            return BadRequest(ModelState);
        }

        var employee = new Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            PhoneNumber = employeeDto.PhoneNumber,
            Address = employeeDto.Address,
            City = employeeDto.City,
            Country = employeeDto.Country,
            YearsOfExperience = employeeDto.YearsOfExperience,
            DateOfBirth = employeeDto.DateOfBirth,
            Skills = employeeDto.Skills
        };

        if (employeeDto.ProfilePictureFile != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                employeeDto.ProfilePictureFile.CopyTo(memoryStream);
                employee.ProfilePicture = memoryStream.ToArray();
            }
           
        }

        _employeeDataAccess.AddEmployee(employee);
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetEmployee(int id)
    {
        var employee = _employeeDataAccess.GetEmployee(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPut("Update/{id}")]
    public IActionResult UpdateEmployee(int id, [FromForm] Employee employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var employee = _employeeDataAccess.GetEmployee(id);
        if (employee == null)
        {
            return NotFound();
        }

        employee.FirstName = employeeDto.FirstName;
        employee.LastName = employeeDto.LastName;
        employee.Email = employeeDto.Email;
        employee.PhoneNumber = employeeDto.PhoneNumber;
        employee.Address = employeeDto.Address;
        employee.City = employeeDto.City;
        employee.Country = employeeDto.Country;
        employee.YearsOfExperience = employeeDto.YearsOfExperience;
        employee.DateOfBirth = employeeDto.DateOfBirth;
        employee.Skills = employeeDto.Skills;

        if (employeeDto.ProfilePictureFile != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                employeeDto.ProfilePictureFile.CopyTo(memoryStream);
                employee.ProfilePicture = memoryStream.ToArray();
            }
        }

        _employeeDataAccess.UpdateEmployee(employee);
        return NoContent();
    }

    [HttpDelete("Delete/{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        var employee = _employeeDataAccess.GetEmployee(id);
        if (employee == null)
        {
            return NotFound();
        }

        _employeeDataAccess.DeleteEmployee(id);
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult SearchEmployees(string? name, string? email, string? phone, string? skills, int? experience)
    {
        var employees = _employeeDataAccess.SearchEmployees(name, email, phone, skills, experience);
        return Ok(employees);
    }

    

    [HttpGet("{id}/download-Profile-Picture")]

    public IActionResult DownloadProfile(int id)
    {
        var resume = employeeDb.Employees.FirstOrDefault(d=>d.EmployeeId == id);
        if (resume != null)
        {
            return File(resume.ProfilePicture, "application/octet-stream", resume.FirstName + ".jpg");
        }
        else
        {
            return NotFound("File not Found");
        }
    }

    [HttpGet("Download-pdf")]
    public IActionResult DownloadPdf(int id)
    {
        var data = employeeDb.Employees.Find(id);
       
        if (data != null)
        {
            var emp = new Employee
            {
                ProfilePicture = data.ProfilePicture,
                FirstName = data.FirstName,
                LastName = data.LastName,
                DateOfBirth = data.DateOfBirth,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                Address = data.Address,
                City = data.City,
                Country = data.Country,
                Skills = data.Skills,
                YearsOfExperience = data.YearsOfExperience,



            };
            var pdfBytes = _employeeDataAccess.GeneratePdf(emp);
            return File(pdfBytes, "application/pdf", "person_info.pdf");
        }
        else
        {
            return NotFound("Datanot Found");
        }

       
    }

}
