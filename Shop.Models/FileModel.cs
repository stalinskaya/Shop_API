using Microsoft.AspNetCore.Http;

namespace Shop.Models
{
	public class FileModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }

		public int ProductID { get; set; }
		public virtual Product Product { get; set; }

		public string ApplicationUserId { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }
	}
}