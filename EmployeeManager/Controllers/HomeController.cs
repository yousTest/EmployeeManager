using EmployeeManager.Models;
using EmployeeManager.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
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
                employee = _employeeRepository.GetEmployee(Id ?? 1)
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(HomeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Employee newEmployee = new Employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.AddEmployee(newEmployee);
                return RedirectToAction("Details", new { id = newEmployee.Id });
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            Employee employeeToDelete = _employeeRepository.GetEmployee(id);
            if (employeeToDelete !=null)
            {
                _employeeRepository.Delete(employeeToDelete);
            }
            return RedirectToAction("index");
        }
    }
}
