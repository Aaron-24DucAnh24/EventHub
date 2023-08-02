using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public interface IInvoiceRepository
  {
    public List<Invoice> GetInvoices(string mail);
    public Invoice? GetInvoiceWithCode(string id, string code);
    public bool AddInvoice(Invoice invoice);
    public bool UpdateInvoice(Invoice invoice);
  }
}