using Microsoft.AspNetCore.Identity;
using Shop.BLL.Infrastructure;
using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.BLL.Interfaces
{
	public interface IRoleService
	{
		Task<OperationDetails> Create(string name);
		List<IdentityRole> GetAllRoles();
		Task<OperationDetails> Delete(string id);
		List<ApplicationUser> GetAllUsers();
		Task<ApplicationUser> FindUserById(string userId);
		Task<IList<string>> GetUserRoles(ApplicationUser user);
		Task<OperationDetails> AddToRoles(ApplicationUser user, IEnumerable<string> addedRoles);
		Task<OperationDetails> RemoveFromRoles(ApplicationUser user, IEnumerable<string> removedRoles);
	}
}
