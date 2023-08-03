using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Dto;
using TicketBooking.API.Helper;
using TicketBooking.API.Services;

namespace TicketBooking.API.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceController : ControllerBase
	{
		private readonly IInvoiceService _invoicesService;
		private readonly IEmailValidationService _emailValidationService;

		public InvoiceController(
			IInvoiceService invoicesService,
			IEmailValidationService emailValidationService
		)
		{
			_invoicesService = invoicesService;
			_emailValidationService = emailValidationService;
		}

		[HttpGet("{mail}")]
		[ProducesResponseType(200, Type = typeof(List<InvoiceResponse>))]
		public ActionResult GetInvoice(string mail)
		{
			var result = _invoicesService.GetInvoices(mail);

			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(result);	
		}

		[HttpPost]
		[ProducesResponseType(204, Type = typeof(string))]
		public async Task<ActionResult> AddInvoice(InvoiceRequest invoiceRequest)
		{
			string code = await _emailValidationService.SendValidationCode(
				invoiceRequest.FullName,
				invoiceRequest.Mail
			);

			if (code == "")
			{
				return Problem(ResponseStatus.ServiceError);
			}

			string invoiceId = _invoicesService.AddInvoice(invoiceRequest, code);

			if (invoiceId == "")
			{
				ModelState.AddModelError("", ResponseStatus.AddError);
				return BadRequest(ModelState);
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(invoiceId);
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(string))]
		public ActionResult ValidateInvoice(
				[FromQuery] string invoiceId,
				[FromQuery] string code)
		{
			bool result = _invoicesService.ValidateInvoice(invoiceId, code);

			if (!result)
				return Problem(ResponseStatus.UpdateError);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(result.ToString());
		}
	}
}