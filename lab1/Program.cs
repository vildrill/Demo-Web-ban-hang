

var builder = WebApplication.CreateBuilder(args);
//---
//khai báo để hệ thống sử dụng mô hình MVC
//builder.Services.AddMvc();
//hoặc
builder.Services.AddControllersWithViews();
//khai bao session
builder.Services.AddSession();
//---
var app = builder.Build();
//su dung session
app.UseSession();

//app.MapGet("/", () => "Hello World!");
//---
//khai báo để project hiểu được và có thể load các file trong thư mục wwwroot
app.UseStaticFiles();
//cấu hình đường dẫn
app.MapControllerRoute(name: "area", pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
//---
app.Run();