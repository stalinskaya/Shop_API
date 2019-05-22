using Shop.BLL.Infrastructure;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Interfaces
{
	public interface IAccountService : IDisposable
	{
		Task<object> Create(ApplicationUser user, string pass);
		Task<ApplicationUser> FindUserById(string userId);
		Task<ApplicationUser> FindUserByName(string userName);
		Task<ApplicationUser> FindUserByEmail(string email);
		Task<OperationDetails> ConfirmEmail(ApplicationUser user, string code);
		Task<OperationDetails> EmailConfirmed(ApplicationUser user);
		Task<OperationDetails> Authentication(string email, string password, bool rememberMe, bool value);
		Task<OperationDetails> CheckPassword(ApplicationUser user, string password);
		IEnumerable<ApplicationUser> GetUsers();
		string GenerateToken(ApplicationUser user);
		Task<OperationDetails> Exit();
	}
}
