using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "FirstName")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "LastName")]
		public string LastName { get; set; }

		[Required]
		[Display(Name = "День рождения")]
		[DataType(DataType.Date)]
		public DateTime Birthday { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль")]
		public string PasswordConfirm { get; set; }

		//public IFormFile File { get; set; }
	}
}
