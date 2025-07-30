using Microsoft.EntityFrameworkCore;
using StudentData.Models;

namespace StudentData
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
    }
}