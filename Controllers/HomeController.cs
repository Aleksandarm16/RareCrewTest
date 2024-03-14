using Microsoft.AspNetCore.Mvc;
using RareCrewTest.Models;
using RareCrewTest.Services;
using System.Diagnostics;
using System.Text.Json;

namespace RareCrewTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmpolyeeService _empolyeeService;
        public HomeController()
        {
                _empolyeeService = new EmpolyeeService();
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _empolyeeService.GetEmployeesWorkHours());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred: " + ex.Message;
                return View();
            }
        }      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
