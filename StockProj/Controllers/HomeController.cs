using Microsoft.AspNetCore.Mvc;
using StockProj.Models;
using System.Diagnostics;

namespace StockProj.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }
        public IActionResult CommonError()
        {
            ViewBag.ErrorMsg = HttpContext.Items["errorMsg"];
            return View();
        }
    }
}
