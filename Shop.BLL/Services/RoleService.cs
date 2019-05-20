using Microsoft.AspNetCore.Identity;
using Shop.BLL.Infrastructure;
using Shop.BLL.Interfaces;
using Shop.DAL.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Services
{
	public class RoleService : IRoleService
	{
		IUnitOfWork Database { get; set; }
		private UserManager<ApplicationUser> UserManager;
		private RoleManager<IdentityRole> RoleManager;

		public RoleService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			Database = uow;
			UserManager = userManager;
			RoleManager = roleManager;
		}

		public async Task<OperationDetails> Create(string name)
		{
			var role = await RoleManager.FindByNameAsync(name);
			if (role == null)
			{
				var result = await RoleManager.CreateAsync(new IdentityRole(name));
				if (result.Errors.Any())
				{
					return new OperationDetails(false, result.Errors.FirstOrDefault().ToString(), "");
				}
				else
				{
					return new OperationDetails(true, "Adding role is successful", "Id");
				}
			}
			else
			{
				return new OperationDetails(false, "Role with this name already exists", "Name");
			}
		}

		public List<IdentityRole> GetAllRoles()
		{
			var roles = RoleManager.Roles.ToList();
			return roles;
		}

		public async Task<OperationDetails> Delete(string id)
		{
			IdentityRole role = await RoleManager.FindByIdAsync(id);
			if (role != null)
			{
				IdentityResult result = await RoleManager.DeleteAsync(role);
				if (result.Errors.Any())
				{
					return new OperationDetails(false, result.Errors.FirstOrDefault().ToString(), "");
				}
				else
				{
					return new OperationDetails(true, "Deleting role is successful", "Id");
				}
			}
			else
			{
				return new OperationDetails(false, "Role with this name doesn't exists", "Name");
			}
		}

		public List<ApplicationUser> GetAllUsers()
		{
			var users = UserManager.Users.ToList();
			return users;
		}

		public async Task<ApplicationUser> FindUserById(string userId)
		{
			var user = await UserManager.FindByIdAsync(userId);
			return user ?? null;
		}

		public async Task<IList<string>> GetUserRoles(ApplicationUser user)
		{
			var userRoles = await UserManager.GetRolesAsync(user);
			return userRoles ?? null;
		}

		public async Task<OperationDetails> AddToRoles(ApplicationUser user, IEnumerable<string> addedRoles)
		{
			var result = await UserManager.AddToRolesAsync(user, addedRoles);
			if (result.Errors.Any())
			{
				return new OperationDetails(false, result.Errors.FirstOrDefault().ToString(), "");
			}
			else
			{
				return new OperationDetails(true, "Editing roles is successful", "Id");
			}
		}

		public async Task<OperationDetails> RemoveFromRoles(ApplicationUser user, IEnumerable<string> removedRoles)
		{
			var result = await UserManager.AddToRolesAsync(user, removedRoles);
			if (result.Errors.Any())
			{
				return new OperationDetails(false, result.Errors.FirstOrDefault().ToString(), "");
			}
			else
			{
				return new OperationDetails(true, "Editing roles is successful", "Id");
			}
		}
	}
}
