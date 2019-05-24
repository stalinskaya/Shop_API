using System.Threading.Tasks;
using Shop.BLL.Interfaces;
using System.Net;
using System.Net.Mail;
using Shop.Models;

namespace Shop.BLL.Services
{
	public class EmailService : IEmailService
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("yanastalin99@gmail.com", "fuck164466")
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress("yanastalin99@gmail.com")
			};
			mailMessage.To.Add(email);
			mailMessage.Subject = subject;
			mailMessage.Body = message;
			return client.SendMailAsync(mailMessage);
		}
	}
}
