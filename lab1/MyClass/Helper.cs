//nhìn thấy folder Models phải using nó
using lab1.Models;
using project.Models;
using System.Collections.Generic;
using System.Linq;
namespace lab1.MyClass
{
    public class Helper
    {
        public static MyDbContext db = new MyDbContext();
        //lấy ảnh quảng cáo theo nội dung truyền vào
        public static List<ItemAdv> GetAdv(int _Position)
        {
            List<ItemAdv> list_adv = db.Adv.Where(item => item.Position == _Position).ToList();
            return list_adv;
        }
		public static string GetCategoryName(int _CategoryId)
		{
			ItemCategory record = db.Categories.Where(item => item.Id == _CategoryId).FirstOrDefault();
			return record.Name;
		}
	}
}
