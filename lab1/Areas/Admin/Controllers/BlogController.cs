using Microsoft.AspNetCore.Mvc;
//thao tac voi IFormCollection
using Microsoft.AspNetCore.Http;
//doi tuong ma hoa password -> co the gan vao mot bien de su dung bien nay
using BC = BCrypt.Net.BCrypt;
//de nhin thay cac class trong folder Models thi phai using dong duoi
using project.Models;
//doi tuong phan trang
using X.PagedList;
//su dung kieu List
using System.Collections.Generic;
//de nhin thay file CheckLogin.cs trong thu muc Attributes
using project.Areas.Admin.Attributes;
using System.Security.Cryptography;
using System.Data;//sử dụng cho các đối tượng: DataTable, SqlConnection, DataAdapter, DataCommand...
using Microsoft.Data.SqlClient;
using lab1.Models;
using System.Drawing;

namespace lab1.Areas.Admin.Controllers
{
	[Area("Admin")]
	//Gọi Attribute CheckLogin để kiểm tra đăng nhập
	[CheckLogin]
	public class BlogController : Controller
	{
		//khởi tạo đối tượng thao tác csdl
		public MyDbContext db = new MyDbContext();
		public IActionResult Index(int? page)
		{
			//lấy trang hiện tại
			int current_page = page ?? 1;
			//định nghĩa số bản ghi trên một trang
			int record_per_page = 4;
			//lấy tất cả các bản ghi trong table Products
			List<ItemBlog> list_record = db.Blog.OrderByDescending(item => item.Id).ToList();
			//truyền giá trị ra view có phân trang
			return View("Index", list_record.ToPagedList(current_page, record_per_page));
		}
		public IActionResult Update(int? id)
		{
			int _id = id ?? 0;
			//lay mot ban ghi
			ItemBlog record = db.Blog.Where(item => item.Id == _id).FirstOrDefault();
			//tạo biến action để đưa vào thuộc tính action của thẻ form
			ViewBag.action = "/Admin/Blog/UpdatePost/" + _id;
			//gọi view, truyền dữ liệu ra view
			return View("CreateUpdate", record);
		}
		[HttpPost]
		public IActionResult UpdatePost(IFormCollection fc, int? id)
		{
			int _id = id ?? 0;
			//lấy dữ liệu của thẻ form thông qua đối tượng fc
			string _title = fc["title"].ToString().Trim();	
			string _author = fc["author"].ToString().Trim();            
            string _topic = fc["topic"].ToString().Trim();
			string _content = fc["content"].ToString().Trim();

			//lấy một bản ghi
			ItemBlog record = db.Blog.Where(item => item.Id == _id).FirstOrDefault();
			if (record != null)
			{
				record.Title = _title;
				record.Author = _author;
                record.Time = DateTime.Now;
                record.Topic = _topic;
				record.Content = _content;
				//lay thong tin cua file
				string _file_name = "";
				try
				{
					//lay ten file
					_file_name = Request.Form.Files[0].FileName;
				}
				catch {; }
				if (!string.IsNullOrEmpty(_file_name))
				{
					//upload anh moi
					var timestamp = DateTime.Now.ToFileTime();
					_file_name = timestamp + "_" + _file_name;
					//lay duong dan cua file
					string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Blog", _file_name);
					//upload file
					using (var stream = new FileStream(_Path, FileMode.Create))
					{
						Request.Form.Files[0].CopyTo(stream);
					}
					//update gia tri vao cot Photo trong csdl
					record.Photo = _file_name;
					//cập nhật lại table
					db.SaveChanges();
				}				
			}
			//di chuyển đến action có tên là Index
			return RedirectToAction("Index");
		}
		public IActionResult Create()
		{
			//tạo biến action để đưa vào thuộc tính action của thẻ form
			ViewBag.action = "/Admin/Blog/CreatePost";
			return View("CreateUpdate");
		}
		[HttpPost]
		public IActionResult CreatePost(IFormCollection fc)
		{
			//lấy dữ liệu của thẻ form thông qua đối tượng fc
			string _title = fc["title"].ToString().Trim();
			string _author = fc["author"].ToString().Trim();
			
			string _topic = fc["topic"].ToString().Trim();
			string _content = fc["content"].ToString().Trim();
			//lay mot ban ghi
			ItemBlog record = new ItemBlog();
			record.Title = _title;
			record.Author = _author;
			record.Time = DateTime.Now;
            record.Topic = _topic;
			record.Content = _content;
			//lay thong tin cua file
			string _file_name = "";
			try
			{
				//lay ten file
				_file_name = Request.Form.Files[0].FileName;
			}
			catch {; }
			if (!string.IsNullOrEmpty(_file_name))
			{
				//upload anh moi
				var timestamp = DateTime.Now.ToFileTime();
				_file_name = timestamp + "_" + _file_name;
				//lay duong dan cua file
				string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Blog", _file_name);
				//upload file
				using (var stream = new FileStream(_Path, FileMode.Create))
				{
					Request.Form.Files[0].CopyTo(stream);
				}
				//update gia tri vao cot Photo trong csdl
				record.Photo = _file_name;
				//cập nhật lại table
				db.SaveChanges();
			}
			//them ban ghi vao table Products
			db.Blog.Add(record);
			db.SaveChanges();			
			//di chuyển đến action có tên là Index
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int? id)
		{
			int _id = id ?? 0;
			//lay mot ban ghi
			ItemBlog record = db.Blog.Where(item => item.Id == _id).FirstOrDefault();
			//xoa ban ghi khoi csdl
			db.Blog.Remove(record);
			//cap nhat lai table Products
			db.SaveChanges();
			//di chuyển đến action có tên là Index
			return RedirectToAction("Index");
		}
	}
}
