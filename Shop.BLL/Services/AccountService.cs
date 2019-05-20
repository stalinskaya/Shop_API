using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shop.BLL.Infrastructure;
using Shop.BLL.Interfaces;
using Shop.DAL.Interfaces;
using Shop.Models;
using Shop.BLL.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Shop.BLL.Services
{
	public class AccountService : IAccountService
	{
		IUnitOfWork Database { get; set; }
		private UserManager<ApplicationUser> UserManager;
		private SignInManager<ApplicationUser> SignInManager;
		private readonly ApplicationSettings applicationSettings;
		
		public AccountService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> options)
		{
			Database = uow;
			UserManager = userManager;
			SignInManager = signInManager;
			this.applicationSettings = options.Value;
		}

		public async Task<OperationDetails> Create(ApplicationUser newUser, string password)
		{
			var user = await UserManager.FindByEmailAsync(newUser.Email);
			if (user == null)
			{
				user = new ApplicationUser
				{
					Email = newUser.Email,
					UserName = newUser.Email,
					FirstName = newUser.FirstName,
					LastName = newUser.LastName
				};
				var result = await UserManager.CreateAsync(user, password);
				await UserManager.AddToRoleAsync(user, "Member");
				if (result.Errors.Any())
				{
					return new OperationDetails(false, result.Errors.FirstOrDefault().ToString(), "");
				}
				else
				{
					return new OperationDetails(true, "Successful registration", user.Id);
				}
			}
			else
			{
				return new OperationDetails(false, "User with this login already exists", "Email");
			}
		}


		public async Task<IdentityResult> Create(ApplicationUser newUser)
		{
			return await UserManager.CreateAsync(newUser);
		}

		public async Task<ApplicationUser> FindUserById(string userId)
		{
			var user = await UserManager.FindByIdAsync(userId);
			if (user != null)
				return user;
			return null;
		}

		public async Task<ApplicationUser> FindUserByName(string userName)
		{
			var user = await UserManager.FindByNameAsync(userName);
			if (user != null)
				return user;
			return null;
		}

		public async Task<ApplicationUser> FindUserByEmail(string email)
		{
			var user = await UserManager.FindByEmailAsync(email);
			if (user != null)
				return user;
			return null;
		}

		public async Task<OperationDetails> ConfirmEmail(ApplicationUser user, string code)
		{
			var success = await UserManager.ConfirmEmailAsync(user, code);
			return success.Succeeded ? new OperationDetails(true, "Success", "") : new OperationDetails(false, "Error", "");
		}

		public async Task<OperationDetails> GenerateCode(ApplicationUser user)
		{
			var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
			if (code != null)
				return new OperationDetails(true, "Success", code);
			return new OperationDetails(false, "Error", "");
		}

		public string GenerateToken(ApplicationUser user)
		{
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim("UserID",user.Id.ToString())
					}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.CreateToken(tokenDescriptor);
			var token = tokenHandler.WriteToken(securityToken);
			return token;
		}

		public async Task<OperationDetails> EmailConfirmed(ApplicationUser user)
		{
			var confirm = await UserManager.IsEmailConfirmedAsync(user);
			if (!confirm)
				return new OperationDetails(false, "Error", "");
			return new OperationDetails(true, "Success", "");
		}

		public async Task<OperationDetails> Authentication(string email, string password, bool rememberMe, bool value)
		{
			var success = await SignInManager.PasswordSignInAsync(email, password, rememberMe, value);
			if (success.Succeeded)
				return new OperationDetails(true, "Successful login", "");
			return new OperationDetails(false, "Error login", "");
		}

		public async Task<OperationDetails> CheckPassword(ApplicationUser user, string password)
		{
			var success = await UserManager.CheckPasswordAsync(user, password);
			if (!success)
				return new OperationDetails(false, "Error", "");
			return new OperationDetails(true, "Success", "");
		}

		public async Task<OperationDetails> Exit()
		{
			await SignInManager.SignOutAsync();
			return new OperationDetails(true, "Successful exit", "");
		}

		public IEnumerable<ApplicationUser> GetUsers()
		{
			return Database.Users.GetAll();
		}

		public void Dispose()
		{
			Database.Dispose();
		}
	}
}