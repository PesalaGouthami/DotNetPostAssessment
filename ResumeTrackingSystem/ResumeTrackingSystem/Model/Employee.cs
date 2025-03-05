using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace ResumeTrackingSystem.Model
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        [Column("employeeid")]
        //[Required]
        public int EmployeeId { get; set; }
        //[Required]
        [Column("firstname")]
        public string FirstName { get; set; }
        //[Required]
        [Column("lastname")]
        public string LastName { get; set; }
        //[Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",ErrorMessage = "Invalid email format")]
        [Column("email")]
        public string Email { get; set; }
        //[Required]
        [Column("phonenumber")]
        [RegularExpression(@"^\d{10}$",ErrorMessage = "Phone number must be exactly 10 digits")]
        public string PhoneNumber { get; set; }
        //[Required]
        [Column("address")]
        public string Address { get; set; }
        //[Required]
        [Column("city")]
        public string City { get; set; }
        //[Required]
        [Column("country")]
        public string Country { get; set; }
        //[Required]
        [Column("yearofexperience")]
        [Range(0,50,ErrorMessage ="Expereinec must be between 0 - 50")]
        public int YearsOfExperience { get; set; }
        [Column("profilepicture")]
        public byte[]? ProfilePicture { get; set; }  

        [NotMapped]
        public IFormFile ProfilePictureFile { get; set; }

        //[Required]
        [DataType(DataType.Date)]
        [Column("dateofbirth")]
        public DateOnly DateOfBirth { get; set; }

        [Column("skills")]
        public string Skills { get; set; }
        




    }
}
