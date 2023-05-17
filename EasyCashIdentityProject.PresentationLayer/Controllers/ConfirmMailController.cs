using EasyCashIdentityProject.PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
	public class ConfirmMailController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
            var value = TempData["Mail"];
            ViewBag.v = value + " aaa";


            return View();
		}

        [HttpPost]
        public IActionResult Index(ConfirmViewModel confirmViewModel)
        {


            return View();
        }
    }
}
