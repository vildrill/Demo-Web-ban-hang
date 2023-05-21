using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
//su dung doi tuong thao tac IFormCollection
using Microsoft.AspNetCore.Http;
//nhin thay cac file .cs ben trong folder Models
using project.Models;
//su dung entity framework
using Microsoft.EntityFrameworkCore;
//phan trang
using X.PagedList;
//nhin thay file CheckLogin.cs tron thu muc Attributes
using project.Areas.Admin.Attributes;
//doi tuong thao tac file
using System.IO;
//su dung kieu List
using System.Collections.Generic;
//doi tuong ma hoa password
using BC = BCrypt.Net.BCrypt;
using lab1.Models;

namespace lab1.Controllers
{

	public class SearchController : Controller
	{
		public MyDbContext db = new MyDbContext();
		public IActionResult SearchPrice(int? page)
		{
			double fromPrice = !String.IsNullOrEmpty(Request.Query["fromPrice"]) ? Convert.ToDouble(Request.Query["fromPrice"]) : 0;
			double toPrice = !String.IsNullOrEmpty(Request.Query["toPrice"]) ? Convert.ToDouble(Request.Query["toPrice"]) : 0;

			//---

			//truyền biến CategoryId ra View
			ViewBag.fromPrice = fromPrice;
			ViewBag.toPrice = toPrice;
			//return Content(CategoryId.ToString());
			//---
			//lấy trang hiện tại
			int current_page = page ?? 1;
			//định nghĩa số bản ghi trên một trang
			int record_per_page = 12;
			//lấy tất cả các bản ghi trong table Products

			//Có thể tiếp tục truy vấn tiếp kết quả đã lấy về
			List<ItemProducts> list_record = db.Products.Where(item => item.Price - item.Price * item.Discount / 100 >= fromPrice && item.Price - item.Price * item.Discount / 100 <= toPrice).OrderByDescending(item => item.Id).ToList();
			//truyền giá trị ra view có phân trang
			return View("SearchPrice", list_record.ToPagedList(current_page, record_per_page));
		}
		public IActionResult Tag(int? page, int? id)
		{
			int _id = id ?? 0;

			//---

			//truyền biến CategoryId ra View
			ViewBag._id = id;
			//return Content(CategoryId.ToString());
			//---
			//lấy trang hiện tại
			int current_page = page ?? 1;
			//định nghĩa số bản ghi trên một trang
			int record_per_page = 12;
			//lấy tất cả các bản ghi trong table Products
			//Có thể tiếp tục truy vấn tiếp kết quả đã lấy về
			List<ItemProducts> list_record = (from tag in db.Tags
											  join tag_product in db.TagsProducts on tag.Id
											  equals tag_product.TagId
											  join product in db.Products on tag_product.ProductId
											  equals product.Id
											  where tag.Id == _id
											  select product).ToList();
			//truyền giá trị ra view có phân trang
			return View("SearchTag", list_record.ToPagedList(current_page, record_per_page));
		}
		public IActionResult SearchName(int? page)
		{
			string key = !String.IsNullOrEmpty(Request.Query["key"]) ? Request.Query["key"] : "0";


			//---
			//truyền biến CategoryId ra View
			ViewBag.key = key;
			//return Content(CategoryId.ToString());
			//---
			//lấy trang hiện tại
			int current_page = page ?? 1;
			//định nghĩa số bản ghi trên một trang
			int record_per_page = 4;
			//lấy tất cả các bản ghi trong table Products
			//Có thể tiếp tục truy vấn tiếp kết quả đã lấy về
			List<ItemProducts> list_record = db.Products.Where(item => item.Name.Contains(key) || item.Description.Contains(key) || item.Content.Contains(key)).OrderByDescending(item => item.Id).ToList();
			return View("SearchName", list_record.ToPagedList(current_page, record_per_page));
		}
		public string Ajax()
		{
			string key = !String.IsNullOrEmpty(Request.Query["key"]) ? Request.Query["key"] : "0";
			List<ItemProducts> list_record = db.Products.Where(item => item.Name.Contains(key) || item.Description.Contains(key) || item.Content.Contains(key)).OrderByDescending(item => item.Id).ToList();
			string str = "";
			foreach (var item in list_record)
			{
				str = str + "<li><a href='/Products/Detail/" + item.Id + "'><img src='/Upload/Products/" + item.Photo + "'>" + item.Name + "</a></li>";
			}
			return str;



		}
	}
}
