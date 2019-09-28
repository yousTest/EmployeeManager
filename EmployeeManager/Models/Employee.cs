using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage ="Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email Adres")]
        public string Email { get; set; }

        public Dept? Department { get; set; }
    }
}
