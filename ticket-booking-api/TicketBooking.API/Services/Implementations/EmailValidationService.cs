using System.Net;
using System.Net.Mail;
using TicketBooking.API.Helper;

namespace TicketBooking.API.Services
{
	public class EmailValidationService : IEmailValidationService
	{
		private readonly SmtpClient _smtpClient;

		public EmailValidationService()
		{

			_smtpClient = new SmtpClient(ConfigurationString.SmtpClient, 587)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(
					ConfigurationString.EmailClient,
					ConfigurationString.EmailPassword
				)
			};
		}

		public async Task<string> SendValidationCodeAsync(string fullName, string mail)
		{
			string code = GetCode();
			string mailTitle = GetMailTitle(fullName);
			string mailContent = GetMailContent(fullName, code, mail);
			string? emailClient = ConfigurationString.EmailClient;

			if (emailClient == null)
				return "";

			MailMessage message = new(
				from: emailClient,
				to: mail,
				mailTitle,
				mailContent
			)
			{ IsBodyHtml = true };

			try
			{
				await _smtpClient.SendMailAsync(message);
			}
			catch (Exception)
			{
				return "";
			}

			return code;
		}

		private static string GetMailTitle(string name)
		{
			return $"[EventBooking] {name.ToUpper()}'S PAYMENT INFORMATION";
		}

		private static string GetCode()
		{
			return new Random().Next(100000, 999999).ToString();
		}

		private static string GetMailContent(string name, string code, string mail)
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