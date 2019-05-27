using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.BLL.Interfaces;
using Shop.Models;
using System.Net.Http;
using Shop.UI.ViewModels;
using Shop.BLL.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Shop.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		public readonly IProductService productService;
		public readonly IFileService fileService;

		public ProductController(IProductService productService, IFileService fileService)
		{
			this.productService = productService;
			this.fileService = fileService;
		}

		public IEnumerable<Product> Get()
		{
			return productService.GetProducts();
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int id)
		{
			var product = productService.GetProduct(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[Authorize(Roles ="Admin")]
		[HttpPost]
		public IActionResult Post([FromForm] ProductViewModel productViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var product = new Product
			{
				Name = productViewModel.Name,
				Price = productViewModel.Price,
				AddDate = DateTime.Now
			};
			productService.MakeProduct(product);
			if (productViewModel.Files != null)
			{
				fileService.UploadImages(productViewModel.Files, product.ProductID);
			}
			return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromForm] Product product)
		{
			if (product == null)
				return BadRequest();

			var existingProduct = productService.GetProduct(id);

			if (existingProduct == null)
				return NotFound();
			try
			{
				existingProduct.Name = product.Name;
				existingProduct.Price = product.Price;
				existingProduct.AddDate = DateTime.Now;

				productService.EditProduct(existingProduct);
			}
			catch (ValidationException ex)
			{
				ModelState.AddModelError(ex.Property, ex.Message);
			}

			return Ok(product);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			productService.DeleteProduct(id);
			return NoContent();
		}
	}
}