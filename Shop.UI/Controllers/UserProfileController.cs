using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.BLL.Interfaces;

namespace Shop.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserProfileController : ControllerBase
	{
		public readonly IAccountService accountService;

		public UserProfileController(IAccountService serv)
		{
			this.accountService = serv;
		}

		[HttpGet]
		[Authorize]
		//GET : /api/UserProfile
		public async Task<Object> GetUserProfile()
		{
			string userId = User.Claims.First(c => c.Type == "UserID").Value;
			var user = await accountService.FindUserById(userId);
			return new
			{
				user.FirstName,
				user.LastName,
				user.Email,
				user.UserName
			};
		}
	}
}