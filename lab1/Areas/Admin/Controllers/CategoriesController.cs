using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;
using project.Models;
using X.PagedList;
using System.Collections.Generic;
using project.Areas.Admin.Attributes;
using System.Security.Cryptography;
using lab1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data; // sử dụng cho DataTable, SqlConnection, DataAdapter, DataCommand,...
using Microsoft.Data.SqlClient;
using project.Models;
namespace final1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class CategoriesController : Controller
    {
        public MyDbContext db = new MyDbContext();
        //Tạo biến toàn cục để đọc các thông số từ file appsettings.json
        public IConfiguration configuration;
        public CategoriesController(IConfiguration _configuration)
        {
            configuration = _configuration;

        }
        public IActionResult Index(int? page)
        {
            DataTable dtCategories = new DataTable();
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from categories where ParentId = 0 order by Id desc", conn);
                da.Fill(dtCategories);
            }
            int current_page = page ?? 1;
            int record_per_page = 4;
            List<ItemCategory> listCategories = new List<ItemCategory>();
            foreach (DataRow item in dtCategories.Rows)
            {
                listCategories.Add(new ItemCategory() { Id = Convert.ToInt32(item["Id"].ToString()), Name = item["Name"].ToString(), ParentId = Convert.ToInt32(item["ParentId"].ToString()) });
            }
            return View("Index", listCategories.ToPagedList(current_page, record_per_page));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            DataTable dtCategories = new DataTable();
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from categories where Id = " + _id, conn);
                da.Fill(dtCategories);
            }  
            DataRow itemCategory = dtCategories.NewRow();
            if (dtCategories.Rows.Count > 0)
                itemCategory = dtCategories.Rows[0];
            int _CurrentId = 0;
            if(itemCategory != null)
                _CurrentId = Convert.ToInt32(itemCategory["Id"]);
            ViewBag.listCategory = db.Categories.Where(item => item.ParentId == 0 && item.Id
            != _CurrentId).OrderByDescending(item => item.Id).ToList();
            ViewBag.action = "/Admin/Categories/UpdatePost/" + _id;
            return View("CreateUpdate",itemCategory);
        }
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
            string _parent_id = fc["parent_id"].ToString().Trim();
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update Categories set Name=@var_name,ParentId=@var_parent_id where Id=@var_id", conn);
                cmd.Parameters.AddWithValue("@var_name", _name);
                cmd.Parameters.AddWithValue("@var_parent_id",_parent_id);
                cmd.Parameters.AddWithValue("@var_id", _id);
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Categories");
        }
        public IActionResult Create()
        {
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Categories/CreatePost";
            return View("CreateUpdate");
        }
        //khi ấn nút submit thì trang sẽ ở trạng thái POST
        //url: /Admin/Categories/UpdatePost/Id
        //ở trạng thái POST thì phải khai báo tag sau
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _name = fc["name"].ToString().Trim();
            string _parent_id = fc["parent_id"].ToString().Trim();
            //lay chuoi ket noi -> chuoi nay nam trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                //update, delete, insert thì phải open đối tượng kết nối
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Categories(Name, ParentId) values (@var_name,@var_parent_id)", conn);
                cmd.Parameters.AddWithValue("@var_name", _name);
                cmd.Parameters.AddWithValue("@var_parent_id", _parent_id);
                //thực thi truy cấn
                cmd.ExecuteNonQuery();
            }
            //di chuyển đến một url khác
            return Redirect("/Admin/Categories");
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay chuoi ket noi -> chuoi nay nam trong file appsettings.json
            string strDbConnectString = configuration.GetConnectionString("DbConnectString").ToString();
            using (SqlConnection conn = new SqlConnection(strDbConnectString))
            {
                //update, delete, insert thì phải open đối tượng kết nối
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from Categories where Id=@var_id or ParentId=@var_id", conn);
                cmd.Parameters.AddWithValue("@var_id", _id);
                //thực thi truy cấn
                cmd.ExecuteNonQuery();
            }
            //di chuyển đến một url khác
            return Redirect("/Admin/Categories");
        }
    }
}
