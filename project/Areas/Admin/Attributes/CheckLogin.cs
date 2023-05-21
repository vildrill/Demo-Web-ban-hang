//de ke thua class ActionFilterAttribute thi phai using dong ben duoi
using Microsoft.AspNetCore.Mvc.Filters;
namespace project.Areas.Admin.Attributes
{
    public class CheckLogin : ActionFilterAttribute //ke thua class ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //---
            //lay gia tri cua bien session
            string _email = context.HttpContext.Session.GetString("admin_email");
            //neu session khong ton tai thi di chuyen den trang login
            /*
                String.IsNullOrEmpty(_email)  <=>  String.IsNullOrEmpty(_email) == true  -> bien _email rong
                !String.IsNullOrEmpty(_email) <=>  String.IsNullOrEmpty(_email) == false -> bien _email khong rong
             */
            if (String.IsNullOrEmpty(_email))
            {
                context.HttpContext.Response.Redirect("/Admin/Account/Login");
            }
            //---
            base.OnActionExecuting(context);
        }
    }
}
