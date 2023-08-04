using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Dtos;
using TicketBooking.API.Helper;
using TicketBooking.API.Services;
using FluentValidation;
using FluentValidation.Results;

namespace TicketBooking.API.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceController : ControllerBase
	{
		private readonly IInvoiceService _invoicesService;
		private readonly IEmailValidationService _emailValidationService;
		private readonly IValidator<InvoiceRequest> _validator;

		public InvoiceController(
			IInvoiceService invoicesService,
			IEmailValidationService emailValidationService,
			IValidator<InvoiceRequest> validator
		)
		{
			_invoicesService = invoicesService;
			_emailValidationService = emailValidationService;
			_validator = validator;
		}

		[HttpGet("{mail}")]
		[ProducesResponseType(200, Type = typeof(List<InvoiceResponse>))]
		public ActionResult GetInvoice(string mail)
		{
			List<InvoiceResponse>? result = _invoicesService.GetInvoices(mail);

			if(result ==null)
				return NotFound();

			return Ok(result);	
		}

		[HttpPost]
		[ProducesResponseType(204, Type = typeof(string))]
		public async Task<ActionResult> AddInvoice(InvoiceRequest invoiceRequest)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(invoiceRequest);

			if(!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
				return BadRequest(ModelState);
			}

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

			return Ok(invoiceId);
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(string))]
		public ActionResult ValidateInvoice(
				[FromQuery] string invoiceId,
				[FromQuery] string code)
		{
			if (string.IsNullOrEmpty(invoiceId) || string.IsNullOrEmpty(code))
			{
				ModelState.AddModelError("", ResponseStatus.InvalidRequestParam);
				return BadRequest();
			}

			bool result = _invoicesService.ValidateInvoice(invoiceId, code);

			if (!result)
				return Problem(ResponseStatus.UpdateError);

			return Ok(result.ToString());
		}
	}
}