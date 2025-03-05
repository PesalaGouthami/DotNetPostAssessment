using Microsoft.EntityFrameworkCore;
using ResumeTrackingSystem.Model;

namespace ResumeTrackingSystemAPI.Model
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
    }
}
