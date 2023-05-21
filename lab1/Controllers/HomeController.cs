using lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project.Models;

namespace lab1.Controllers
{
    public class HomeController : Controller
    {
		//khoi tao doi tuong thao tac csdl
		public MyDbContext db = new MyDbContext();
		public IActionResult Index()
        {
			return View();
			//return Redirect("/Admin");
		}		
	}
}
