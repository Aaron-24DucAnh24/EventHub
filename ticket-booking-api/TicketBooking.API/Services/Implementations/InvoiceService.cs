using TicketBooking.API.Dto;
using TicketBooking.API.Models;
using TicketBooking.API.Enum;
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
			var invoices = _invoiceRepository.GetInvoices(mail);

			var result = new List<InvoiceResponse>();

			if (invoices.Count == 0)
				return result;


			foreach (var invoice in invoices)
			{
				var invoiceResponse = new InvoiceResponse
				{
					EventId = invoice.EventId,
					Title = invoice.Event.Title,
					Date = invoice.Event.Date,
					InvoiceId = invoice.Id,
					StageName = invoice.Event.StageName,
					Location = invoice.Event.Location,
					Duration = invoice.Event.Duration,
					Image = invoice.Event.Image,
					Seats = new List<SeatResponse>(),
				};

				foreach (var seat in invoice.Seats)
				{
					var seatEvent = _seatRepository.GetSeatEvent(seat.Id, invoice.EventId);

					var seatResponse = new SeatResponse()
					{
						Name = seat.Name,
						Type = seat.Type,
						Price = seatEvent.Price,
					};

					invoiceResponse.Seats.Add(seatResponse);
				}

				result.Add(invoiceResponse);
			}

			return result;
		}

		public string AddInvoice(InvoiceRequest invoiceRequest, string code)
		{
			var invoiceId = Guid.NewGuid().ToString();

			var invoice = new Invoice()
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

			if (!AddSeatInvoice(invoiceRequest.seatIds, invoiceId))
				return "";

			return invoiceId;
		}

		public bool ValidateInvoice(string invoiceId, string code)
		{
			var invoice = _invoiceRepository.GetInvoiceWithCode(invoiceId, code);

			if (invoice == null)
				return false;

			var updateSeatEvent = UpdateSeatEvent(invoice.Seats.ToList(), invoice.EventId);

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
			var seatIds = new List<string>();

			foreach (var seat in seats)
			{
				seatIds.Add(seat.Id);
			}

			var seatEvents = _seatRepository.GetSeatEvents(seatIds, eventId);

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