namespace TicketBooking.API.Services
{
	public interface IEmailValidationService{
		public Task<string> SendValidationCode(string fullName, string mail);
	}
}