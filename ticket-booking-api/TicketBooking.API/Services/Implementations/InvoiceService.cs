using TicketBooking.API.Dtos;
using TicketBooking.API.Models;
using TicketBooking.API.Enums;
using TicketBooking.API.Repository;

namespace TicketBooking.API.Services
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly ISeatRepository _seatRepository;

		public InvoiceService(
			IInvoiceRepository invoiceRepository, 
			ISeatRepository seatRepository)
		{
			_invoiceRepository = invoiceRepository;
			_seatRepository = seatRepository;
		}

		public List<InvoiceResponse>? GetInvoices(string mail)
		{
			List<Invoice> invoices = _invoiceRepository.GetInvoices(mail);
			List<InvoiceResponse> result = new();

			if (invoices.Count == 0)
				return result;

			foreach (var invoice in invoices)
			{
				InvoiceResponse invoiceResponse = new()
				{
					EventId = invoice.EventId,
					Title = invoice.Event.Title,
					Date = invoice.Event.Date,
					InvoiceId = invoice.Id,
					StageName = invoice.Event.StageName,
					Location = invoice.Event.Location,
					Duration = invoice.Event.Duration,
					Image = invoice.Event.Image,
					Seats = new List<SeatInInvoice>(),
				};

				foreach (var seat in invoice.Seats)
				{
					SeatEvent? seatEvent = _seatRepository.GetSeatEvent(seat.Id, invoice.EventId);
					if(seatEvent != null)
					{
						invoiceResponse.Seats.Add(new SeatInInvoice()
						{
							Name = seat.Name,
							Type = seat.Type,
							Price = seatEvent.Price,
						});
					}
				}
				result.Add(invoiceResponse);
			}

			return result;
		}

		public string AddInvoice(InvoiceRequest invoiceRequest, string code)
		{
			var invoiceId = Guid.NewGuid().ToString();

			Invoice invoice = new()
			{
				Id = invoiceId,
				Name = invoiceRequest.FullName,
				Mail = invoiceRequest.Mail,
				Phone = invoiceRequest.Phone,
				EventId = invoiceRequest.EventId,
				Code = code,
			};

			if (!_invoiceRepository.AddInvoice(invoice))
				return "";

			if (!AddSeatInvoice(invoiceRequest.SeatIds, invoiceId))
				return "";

			return invoiceId;
		}

		public bool ValidateInvoice(string invoiceId, string code)
		{
			Invoice? invoice = _invoiceRepository.GetInvoiceWithCode(invoiceId, code);

			if (invoice == null)
				return false;

			bool updateSeatEvent = UpdateSeatEvent(invoice.Seats.ToList(), invoice.EventId);

			if (!updateSeatEvent)
				return false;

			invoice.IsValidated = true;
			return _invoiceRepository.UpdateInvoice(invoice);
		}

		private bool AddSeatInvoice(List<string> seatIds, string invoiceId)
		{
			foreach (var seatId in seatIds)
			{
				SeatInvoice seatInvoice = new()
				{
					SeatId = seatId,
					InvoiceId = invoiceId,
				};

				if(!_seatRepository.AddSeatInvoice(seatInvoice))
					return false;
			}

			return true;
		}

		private bool UpdateSeatEvent(List<Seat> seats, string eventId)
		{
			List<string> seatIds = new();

			foreach (var seat in seats)
			{
				seatIds.Add(seat.Id);
			}

			List<SeatEvent> seatEvents = _seatRepository.GetSeatEvents(seatIds, eventId);

			foreach (var seatEvent in seatEvents)
			{
				seatEvent.SeatStatus = SeatStatus.Picked;
				if(!_seatRepository.UpdateSeatEvent(seatEvent))
					return false;
			}

			return true;
		}
	}
}