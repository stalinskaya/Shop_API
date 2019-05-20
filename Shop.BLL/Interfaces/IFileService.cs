using Microsoft.AspNetCore.Http;
using Shop.BLL.Infrastructure;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Interfaces
{
	public interface IFileService
	{
		Task UploadImages(IFormFileCollection uploadedImages, int productId);
		IEnumerable<FileModel> GetFiles(int id);
		void Export(DateTime fromDate, DateTime tillDate, List<Order> orders);
		Task UploadImage(IFormFile fileimg, string userId);
	}
}
