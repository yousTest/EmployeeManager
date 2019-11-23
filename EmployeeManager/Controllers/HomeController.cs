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
        public ViewResult Details(int Id)
        {
            Employee employee = _employeeRepository.GetEmployee(Id);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", Id);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                pageTitle = "Employee Details",
                Employee = employee
            };
            return View(homeDetailsViewModel);
        }

        public IActionResult Delete(int id)
        {
            Employee employeeToDelete = _employeeRepository.GetEmployee(id);
            if (employeeToDelete != null)
            {
                if (employeeToDelete.PhotoPath != null)
                {
                    System.IO.File.Delete(hostingEnvironment.WebRootPath + "/images/" + employeeToDelete.PhotoPath);
                }
                _employeeRepository.Delete(employeeToDelete);
            }
            return RedirectToAction("index");
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
                string uniqueFileName = ProcessPhoto(model);
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



        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            HomeEditViewModel model = new HomeEditViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(HomeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath + "/images/" + model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessPhoto(model);

                }

                _employeeRepository.Update(employee);
                return RedirectToAction("Details", new { id = employee.Id });
            }
            return View(model);
        }

        private string ProcessPhoto(HomeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }














        //for multiple files
        //public IActionResult Create(HomeCreateViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (model.PhotoList != null && model.PhotoList.Count > 0)
        //        {
        //            string uniqueFileName = null;
        //            string uploadFolder = null;
        //            string filePath = null;
        //            FileStream stream = null;

        //            foreach (var photo in model.PhotoList)
        //            {
        //                if (photo != null)
        //                {
        //                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

        //                    uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        //                    filePath = Path.Combine(uploadFolder, uniqueFileName);

        //                    stream = new FileStream(filePath, FileMode.Create);
        //                    using (stream)
        //                    {
        //                        photo.CopyTo(stream);
        //                    }
        //                }
        //            }
        //            Employee newEmployee = new Employee()
        //            {
        //                Name = model.Name,
        //                Email = model.Email,
        //                Department = model.Department,
        //                PhotoPath = uniqueFileName
        //            };
        //            _employeeRepository.AddEmployee(newEmployee);
        //            return RedirectToAction("details", new { id = newEmployee.Id });
        //        }
        //    }
        //    return View();
        //}


    }
}
