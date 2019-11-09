using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this  ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                
                new Employee
                {
                    Id = 4,
                    Name = "you",
                    Email = "you@gmail.com",
                    Department = Dept.IT
                },
                new Employee
                {
                    Id = 5,
                    Name = "him",
                    Email = "him@gmail.com",
                    Department = Dept.IT
                }
            );
        }
    }
}
