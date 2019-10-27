using Microsoft.EntityFrameworkCore;
using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManager.DataAccess
{
    public class StudentsDbContext : DbContext
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext> options)
            : base(options)
        {   

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
    }
}
