using EmployeeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.ViewModels
{
    public class HomeDetailsViewModel
    {
        public Employee employee { get; set; }
        public string pageTitle { get; set; }
    }
}
