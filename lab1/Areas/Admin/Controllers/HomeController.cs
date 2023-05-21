using Microsoft.AspNetCore.Mvc;
using project.Areas.Admin.Attributes;

namespace lab1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class HomeController : Controller
    {
        //phai dat tag [Area("Admin")] de phan biet
        
        //sau do vao url: http://localhost/Admin de truy cap controller nay
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
