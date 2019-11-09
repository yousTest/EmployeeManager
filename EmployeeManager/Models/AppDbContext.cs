using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //just demo : you can do the seed in here, beter way to make een extension methode to seed the data
            modelBuilder.Entity<Employee>().HasData(

                new Employee
                {
                    Id = 3,
                    Name = "me",
                    Email = "me@gmail.com",
                    Department = Dept.IT
                });

            modelBuilder.Seed();
        }
    }
}
