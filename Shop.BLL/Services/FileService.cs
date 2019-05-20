using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using Shop.BLL.Infrastructure;
using Shop.BLL.Interfaces;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Shop.BLL.Services
{
	public class FileService : IFileService
	{
		IUnitOfWork Database { get; set; }
		IHostingEnvironment _environment;

		public FileService(IUnitOfWork uow, IHostingEnvironment _environment)
		{
			Database = uow;
			this._environment = _environment;
		}

		public async Task UploadImages(IFormFileCollection uploadedImages, int productId)
		{
			
			foreach (var fileimg in uploadedImages)
			{

				var myUniqueFileName = Convert.ToString(Guid.NewGuid());
				var FileExtension = Path.GetExtension(fileimg.FileName);
				var newFileName = myUniqueFileName + FileExtension;

				// сохраняем файл в папку Files в каталоге wwwroot

				FileModel file = new FileModel
				{
					Name = newFileName,
					Path = "/Files/" + newFileName,
					ProductID = productId,
					ApplicationUserId = null
				};
				Database.Files.Create(file);

				using (var fileStream = new FileStream(_environment.WebRootPath + "/Files/"+ newFileName, FileMode.Create))
				{
					await fileimg.CopyToAsync(fileStream);
				}
			}
			Database.Save();
		}

		public async Task UploadImage(IFormFile fileimg, string userId)
		{
			string path = "/Users/";
			var path_img = path + fileimg.FileName;

			var myUniqueFileName = Convert.ToString(Guid.NewGuid());
			var FileExtension = Path.GetExtension(fileimg.FileName);
			var newFileName = myUniqueFileName + FileExtension;

			FileModel file = new FileModel
			{
				Name = newFileName,
				Path = "/Users/" + newFileName,
				ProductID = 0,
				ApplicationUserId = userId
			};
			Database.Files.Create(file);

			using (var fileStream = new FileStream(_environment.WebRootPath + "/Users/" + newFileName, FileMode.Create))
			{
				await fileimg.CopyToAsync(fileStream);
			}
			Database.Save();
		}

		public IEnumerable<FileModel> GetFiles (int id)
		{
			var files = Database.Files.Find(c => c.ProductID == id);
			return files;
		}

		public void Export(DateTime fromDate, DateTime tillDate, List<Order> orders)
		{
			string sWebRootFolder = _environment.WebRootPath;
			string sFileName = @"demo.xlsx";
			string URL = string.Format("/Excel/{0}", sFileName);
			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
			if (file.Exists)
			{
				file.Delete();
				file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
			}

			using (ExcelPackage package = new ExcelPackage(file))
			{
				// add a new worksheet to the empty workbook
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
				//First add the headers
				worksheet.Cells[1, 1].Value = "OrderDate";
				worksheet.Cells[1, 2].Value = "AplicationUserId";

				int number = 1;
				foreach (var order in orders)
				{
					number += 1;
					worksheet.Cells["A" + number].Value = order.OrderDate.ToString();
					worksheet.Cells["B" + number].Value = order.ApplicationUserId;
				}

				package.Save(); //Save the workbook.
			}
		}
	}
}