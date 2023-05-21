using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace lab1.Models
{
	[Table("Blog")]
	public class ItemBlog
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Topic { get; set; }
		public DateTime Time { get; set; }
		public string Photo { get; set; }
		public string Author { get; set; }


	}
}
