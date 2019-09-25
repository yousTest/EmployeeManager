using EmployeeManager.Models;
using EmployeeManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Route("~/Home")]
        [Route("~/")]
        public ViewResult Index()
        {
            IEnumerable<Employee> model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        [Route("{Id?}")]
        public ViewResult Details(int? Id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                pageTitle = "Details page",
                employee = _employeeRepository.GetEmployee(Id??1)
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public RedirectToActionResult Create(Employee employee)
        {
            _employeeRepository.AddEmployee(employee);
            return RedirectToAction("Details", new { id = employee.Id });
        }
    }   
}
