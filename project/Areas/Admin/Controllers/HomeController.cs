using Microsoft.AspNetCore.Mvc;
//de nhin thay cac file trong folder Areas/Attributes thi phai khai bao dong sau
using project.Areas.Admin.Attributes;

namespace project.Areas.Admin.Controllers
{
    //phai dat tag [Area("Admin")] de phan biet voi HomeController o ben ngoai Area nay
    //khi do go url: http://localhost/Admin thi moi truy cap vao duoc Controller nay
    [Area("Admin")]
    //goi Attribute CheckLogin (nam trong folder Areas/Admin/Attributes)
    [CheckLogin]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
