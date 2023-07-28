using System.Net;
using System.Net.Mail;
using TicketBooking.API.Interfaces;
using TicketBooking.API.Helper;

namespace TicketBooking.API.Repository
{
	public class EmailValidationRepository : IEmailValidationRepository
	{
		private readonly SmtpClient __smtpClient;

		public EmailValidationRepository()
		{

			__smtpClient = new SmtpClient(ConfigurationString.SmtpClient, 587)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(
					ConfigurationString.EmailClient,
					ConfigurationString.EmailPassword
				)
			};
		}

		public async Task<string> SendValidationCode(string fullName, string mail)
		{
			var code = GetCode();
			var mailTitle = GetMailTitle(fullName);
			var mailContent = GetMailContent(fullName, code, mail);
			var message = new MailMessage(
				from: ConfigurationString.EmailClient,
				to: mail,
				mailTitle,
				mailContent
			)
			{ IsBodyHtml = true };

			try
			{
				await __smtpClient.SendMailAsync(message);
			}
			catch (Exception)
			{
				return "";
			}

			return code;
		}

		private string GetMailTitle(string name)
		{
			return $"[EventBooking] {name.ToUpper()}'S PAYMENT INFORMATION";
		}

		private string GetCode()
		{
			return new Random().Next(100000, 999999).ToString();
		}

		private string GetMailContent(string name, string code, string mail)
		{
			return
			@$"<html lang=""en"">
				<head>    
					<meta 
						content=""text/html; charset=utf-8"" 
						http-equiv=""Content-Type"">
					<title>
						Customer payment information
					</title>
				</head>
				<body>
					<h3>
						Hello {name}, thanks for using our service.
						<br/>
						Your confirmation code is <strong style=""color: blue;"">{code}</strong>
						<br/>
						Link to your bookings: <a style=""color: blue;"" href=""http://localhost:3000/my-booking/{mail}"">Booking</a>
					</h3>
				</body>
			</html>";
		}
	}
}