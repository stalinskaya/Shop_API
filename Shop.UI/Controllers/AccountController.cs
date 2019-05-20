using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop.BLL.Interfaces;
using Shop.Models;
using Shop.UI.ViewModels;

namespace Shop.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		
		public readonly IAccountService accountService;
		public readonly IEmailService emailService;
		public readonly IFileService fileService;
		readonly IHostingEnvironment _appEnvironment;

		public AccountController(IAccountService serv, IEmailService _emailService, IHostingEnvironment hostingEnvironment, IFileService fileService)
		{
			accountService = serv;
			emailService = _emailService;
			_appEnvironment = hostingEnvironment;
			this.fileService = fileService;
		}

		public IEnumerable<ApplicationUser> Get()
		{
			return accountService.GetUsers();
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(string id)
		{
			var user = accountService.FindUserById(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		[HttpPost]
		[Route("Register")]
		//POST : /api/ApplicationUser/Register
		public async Task<Object> Post([FromForm] RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = new ApplicationUser
			{
				Email = model.Email,
				UserName = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName
			};
			//if (model.File != null)
			//{
			//	await fileService.UploadImage(model.File, user.Id);
			//}
			try
			{
				var result = await accountService.Create(user, model.Password);
				//if (result.Succeeded)
				//{
				//	var findUser = await accountService.FindUserById(result.Property);
				//	var code = await accountService.GenerateCode(findUser);

				//	var callbackUrl = Url.Action(
				//		"ConfirmEmail",
				//		"Account",
				//		new { userId = findUser.Id, code = code.Property },
				//		protocol: HttpContext.Request.Scheme);

				//	await emailService.SendEmailAsync(user.Email, "Confirm your account",
				//		$"Confirm the registration by clicking on the link: <a href='{callbackUrl}'>link</a>");
				//	return Ok();
				//}
				//else
				//	ModelState.AddModelError("", result.Message);
				//return BadRequest();
				return Ok();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost]
		[Route("Login")]
		//POST : /api/ApplicationUser/Login
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			var user = await accountService.FindUserByEmail(model.Email);
			if (user != null && accountService.CheckPassword(user, model.Password).IsCompletedSuccessfully)
			{
				var token = accountService.GenerateToken(user);
				return Ok(new { token });
			}
			else
				return BadRequest(new { message = "Username or password is incorrect." });
		}
	}
}