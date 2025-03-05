using Microsoft.AspNetCore.Mvc;
using ResumeApiConsume.Models;

namespace ResumeApiConsume.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {

            return View(employee);
        }



    }
}
