using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;
using lab1.Models;
using project.Models;

namespace lab1.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountController : Controller
	{
		public MyDbContext db = new MyDbContext();
		public IActionResult Login()
		{
			return View("Login");
		}
		public IActionResult LoginPost(IFormCollection fc)
        {
			string _email = fc["email"].ToString().Trim();
			string _password = fc["password"].ToString().Trim();
			ItemUser record = db.Users.Where(item => item.Email == _email).FirstOrDefault();
			if(record != null)
            {
				if(BC.Verify(_password, record.Password) == true)
                {
					HttpContext.Session.SetString("admin_email", _email);
					HttpContext.Session.SetString("admin_id", record.Id.ToString());
					return Redirect("/Admin/Home");
				}					
            }
			return Redirect("/Admin/Account/Login?notify=invalid");
		}
		public IActionResult Loggout()
        {
			HttpContext.Session.Remove("admin_email");
			return Redirect("/Admin/Account/Login");
        }
	}
}
