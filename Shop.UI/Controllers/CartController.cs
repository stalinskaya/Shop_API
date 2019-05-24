using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.BLL.Interfaces;
using Shop.BLL.Services;
using Shop.Models;

namespace Shop.UI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		public readonly IProductService productService;
		public readonly IAccountService accountService;
		public readonly IOrderService orderService;
		//public List<Item> cart;

		public CartController(IProductService pr_serv, IAccountService acc_serv, IOrderService order_serv)
		{
			productService = pr_serv;
			accountService = acc_serv;
			orderService = order_serv;
			//cart = items;
		}

		[HttpGet, Route("GetCart")]
		
		public IActionResult GetCart()
		{
			var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
			if (cart == null)
			{
				return BadRequest("Empty");
			}
			else return new ObjectResult(cart);
		}

		[HttpPost("{id}"), Route("AddItem/{id}")]
		[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult AddItem(int id)
		{
			Product product = new Product();
			if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
			{
				List<Item> cart = new List<Item>
				{
					new Item { Product = productService.GetProduct(id), Quantity = 1 }
				};
				SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			}
			else
			{
				List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
				int index = IsExist(id);
				if (index != -1)
				{
					cart[index].Quantity++;
				}
				else
				{
					cart.Add(new Item { Product = productService.GetProduct(id), Quantity = 1 });
				}
				SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			}
			return Ok();
		}

		[HttpDelete]
		[Route("remove/{id}")]
		public IActionResult Remove(int id)
		{
			List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
			int index = IsExist(id);
			cart.RemoveAt(index);
			SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			return NoContent();
		}

		[HttpPost]
		[Route("purchase")]
		public IActionResult Purchase()
		{
			List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
			if (cart.Count == 0)
			{
				ModelState.AddModelError("", "Sorry, your cart is empty!");
			}
			if (ModelState.IsValid)
			{
				var userId = User.Claims.First(c => c.Type == "UserID").Value; ;
				orderService.ProcessOrder(cart.ToList(), userId);
				cart.Clear();
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

		private int IsExist(int id)
		{
			List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
			for (int i = 0; i < cart.Count; i++)
			{
				if (cart[i].Product.ProductID.Equals(id))
				{
					return i;
				}
			}
			return -1;
		}
	}
}