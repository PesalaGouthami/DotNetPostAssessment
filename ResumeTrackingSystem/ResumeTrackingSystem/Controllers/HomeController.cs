using System.IO;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeTrackingSystem.Data;
using ResumeTrackingSystem.Model;



[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IEmployeeDataAccess _employeeDataAccess;

    public HomeController(IEmployeeDataAccess employeeDataAccess)
    {
        _employeeDataAccess = employeeDataAccess;
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
            Console.WriteLine($"Profile picture uploaded successfully: {employeeDto.ProfilePictureFile.FileName}");
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

    [HttpGet("{id}/download")]
    public IActionResult DownloadResume(int id)
    {
        var employee = _employeeDataAccess.GetEmployee(id);
        if (employee == null)
        {
            return NotFound();
        }

        var converter = new SynchronizedConverter(new PdfTools());
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = $"<h1>{employee.FirstName} {employee.LastName}</h1><p>Email: {employee.Email}</p><p>Phone: {employee.PhoneNumber}</p><p>Address: {employee.Address}, {employee.City}, {employee.Country}</p><p>Experience: {employee.YearsOfExperience} years</p><p>Skills: {employee.Skills}</p>",
                    WebSettings = { DefaultEncoding = "utf-8" },
                }
            }
        };

        var pdf = converter.Convert(doc);
        return File(pdf, "application/pdf", $"{employee.FirstName}_{employee.LastName}_Resume.pdf");
    }
}
