using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
