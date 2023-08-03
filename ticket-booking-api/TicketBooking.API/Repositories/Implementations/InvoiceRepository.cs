using Microsoft.EntityFrameworkCore;
using TicketBooking.API.DBContext;
using TicketBooking.API.Models;

namespace TicketBooking.API.Repository
{
  public class InvoiceRepository : IInvoiceRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public InvoiceRepository(ApplicationDbContext applicationDbContext)
    {
      _dbContext = applicationDbContext;
    }

    public List<Invoice> GetInvoices(string mail)
    {
      return _dbContext.Invoices
        .Where(x => x.Mail == mail)
        .Where(x => x.IsValidated)
        .Include(x => x.Event)
        .Include(x => x.Seats)
        .ToList();
    }

    public Invoice? GetInvoiceWithCode(string id, string code)
    {
      return _dbContext.Invoices
        .Where(x => x.Id == id)
        .Where(x => x.Code == code)
        .Where(x => !x.IsValidated)
        .Include(x => x.Seats)
        .FirstOrDefault();
    }

    bool IInvoiceRepository.AddInvoice(Invoice invoice)
    {
      _dbContext.Add(invoice);
      return _dbContext.SaveChanges() != 0;
    }

    bool IInvoiceRepository.UpdateInvoice(Invoice invoice)
    {
      _dbContext.Update(invoice);
      return _dbContext.SaveChanges() != 0;
    }
  }
}