using ASP.NETCoreWebAP.Domain;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreWebAP.Infrastructure
{
    public class StudentsDBContext : DbContext
    {
        public StudentsDBContext(DbContextOptions<StudentsDBContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
    }
}
