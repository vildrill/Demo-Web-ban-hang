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
using Newtonsoft.Json;

namespace lab1.Controllers
{
	public class ProductsController : Controller
	{
		public MyDbContext db = new MyDbContext();
		public IActionResult Index(int? page, int? id)
		{
			//---
			int CategoryId = id ?? 0;
			//truyền biến CategoryId ra View
			ViewBag.CategoryId = CategoryId;
			//return Content(CategoryId.ToString());
			//---
			//lấy trang hiện tại
			int current_page = page ?? 1;
			//định nghĩa số bản ghi trên một trang
			int record_per_page = 8;
			//lấy tất cả các bản ghi trong table Products
			List<ItemProducts> list_record = db.Products.OrderByDescending(item => item.Id).ToList();
			//Có thể tiếp tục truy vấn tiếp kết quả đã lấy về
			if (CategoryId > 0)
			{
				//lấy các sản phẩm có danh mục tương ứng với CategoryId truyền vào
				//---
				var list_products = db.CategoriesProducts.Where(item => item.CategoryId == CategoryId).Select(item => new { item.ProductId });
				List<int> id_products = new List<int>();
				foreach (var item in list_products)
				{
					id_products.Add(item.ProductId);
				}
				//lọc tiếp biến list_record
				list_record = list_record.Where(item => id_products.Contains(item.Id)).ToList();
				//---
				string strOrder = "";
				if (!String.IsNullOrEmpty(Request.Query["order"])) 
					strOrder = Request.Query["order"];
				ViewBag.Order = strOrder;
				switch (strOrder)
				{
					case "name-asc":
						list_record = list_record.OrderBy(item => item.Name).ToList();
						break;
					case "name-desc":
						list_record = list_record.OrderByDescending(item => item.Name).ToList();
						break;
					case "price-asc":
						list_record = list_record.OrderBy(item => item.Price).ToList();
						break;
					case "price-desc":
						list_record = list_record.OrderByDescending(item => item.Price).ToList();
						break;
				}
			}
			//truyền giá trị ra view có phân trang
			return View("Index", list_record.ToPagedList(current_page, record_per_page));
		}

		public IActionResult Detail(int? id)
		{
			int _id = id ?? 0;
			ItemProducts record = db.Products.Where(item => item.Id == _id).FirstOrDefault();
			return View("Detail", record);
		}
		public IActionResult Rating(int? id)
		{
			if (!String.IsNullOrEmpty(HttpContext.Session.GetString("customer_email")))
			{
				int _ProductId = id ?? 0;
				int _Star = !String.IsNullOrEmpty(Request.Query["star"]) ? Convert.ToInt32(Request.Query["star"]) : 0;
				ItemRating record = new ItemRating();
				record.ProductId = _ProductId;
				record.Star = _Star;
				db.Rating.Add(record);
				db.SaveChanges();
				return Redirect("/Products/Detail/" + _ProductId);
			}
			else
				return Redirect("/Account/Login");
		}	
	}
}
