using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.Models;

namespace Shop.BLL.Interfaces
{
	public interface IEmailService
	{
		Task SendEmailAsync(string email, string subject, string message);
	}
}
