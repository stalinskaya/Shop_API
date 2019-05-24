using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.BLL.Interfaces;
using Shop.Models;
using Shop.UI.ViewModels;

namespace Shop.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		public readonly IOrderService orderService;
		public readonly IOrderProductService orderProductService;
		public readonly IProductService productService;
		public readonly IFileService fileService;
		public readonly IAccountService accountService;

		public OrderController(
			IOrderService serv,
			IFileService _file,
			IOrderProductService _ordpr,
			IProductService _pr,
			IAccountService _acc)
		{
			orderService = serv;
			fileService = _file;
			orderProductService = _ordpr;
			productService = _pr;
			accountService = _acc;
		}

		public IEnumerable<Order> Get()
		{
			return orderService.GetAllOrders();
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int id)
		{
			var product = orderService.GetOrder(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		//public IActionResult Details(int? id)
		//{
		//	var order = orderService.GetOrder(id);
		//	var user = accountService.FindUserById(order.ApplicationUserId).Result;
		//	ViewBag.User = user.UserName;
		//	var orderProducts = orderProductService.GetAll();
		//	var ordered_productIDs = orderProducts.Where(c => c.OrderID == order.OrderID);
		//	var products = productService.GetProducts();
		//	var ordered_products = products.Where(c => ordered_productIDs.Any(a => a.ProductID == c.ProductID));
		//	ViewBag.Products = ordered_products;
		//	return View(order);
		//}

		[HttpGet]
		[Route("Report")]
		public IActionResult Report(ReportViewModel reportViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var orders = orderService.GetAllOrders().ToList();
			var filteredOrders = new List<Order>();
			foreach (var order in orders)
			{
				if (order.OrderDate <= reportViewModel.TillDate || order.OrderDate >= reportViewModel.FromDate)
					filteredOrders.Add(order);
			}
			return Ok(filteredOrders);
		}

		[HttpGet("Export")]
		public IActionResult Export(ReportViewModel reportViewModel)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var orders = orderService.GetAllOrders().ToList();
			var filteredOrders = new List<Order>();
			foreach (var order in orders)
			{
				if (order.OrderDate <= reportViewModel.TillDate || order.OrderDate >= reportViewModel.FromDate)
					filteredOrders.Add(order);
			}
			fileService.Export(reportViewModel.FromDate, reportViewModel.TillDate, filteredOrders);
			return Ok();
		}

		protected void Dispose(bool disposing)
		{
			orderService.Dispose();
		}
	}
}