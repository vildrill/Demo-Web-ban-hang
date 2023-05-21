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
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View("Index");
		}
	}
}
