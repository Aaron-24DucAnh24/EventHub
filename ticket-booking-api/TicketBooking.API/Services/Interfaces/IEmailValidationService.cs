namespace TicketBooking.API.Services
{
	public interface IEmailValidationService{
		public Task<string> SendValidationCodeAsync(string fullName, string mail);
	}
}