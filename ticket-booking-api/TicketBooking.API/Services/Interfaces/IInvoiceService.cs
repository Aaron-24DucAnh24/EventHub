using TicketBooking.API.Dto;
namespace TicketBooking.API.Services
{
	public interface IInvoiceService
	{
		public List<InvoiceResponse>? GetInvoices(string mail);
		public string AddInvoice(InvoiceRequest invoice, string code);
		public bool ValidateInvoice(string invoiceId, string code);
	}
}