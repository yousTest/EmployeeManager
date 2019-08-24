using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Controllers
{
    public class DepartmentController : Controller
    {
        public string List()
        {
            return "list() of department Controller";
        }

        public string Details()
        {
            return "Details() of department controller";
        }
    }
}
