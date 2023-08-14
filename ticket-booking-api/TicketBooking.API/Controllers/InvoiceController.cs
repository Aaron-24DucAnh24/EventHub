using Microsoft.AspNetCore.Mvc;
using TicketBooking.API.Dtos;
using TicketBooking.API.Extensions;
using TicketBooking.API.Services;
using TicketBooking.API.Constants;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

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
		[AllowAnonymous]
		public ActionResult GetInvoice(string mail)
		{
			List<InvoiceResponse>? result = _invoicesService.GetInvoices(mail);

			if (result == null)
				return NotFound();

			return Ok(result);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> AddInvoiceAsync(InvoiceRequest invoiceRequest)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(invoiceRequest);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
				return BadRequest(ModelState);
			}

			string code = await _emailValidationService.SendValidationCodeAsync(
				invoiceRequest.FullName,
				invoiceRequest.Mail
			);

			if (code == "")
			{
				return Problem(ResponseStatus.SERVICE_ERROR);
			}

			string invoiceId = _invoicesService.AddInvoice(invoiceRequest, code);

			if (invoiceId == "")
			{
				ModelState.AddModelError("", ResponseStatus.ADD_ERROR);
				return BadRequest(ModelState);
			}

			return Ok(invoiceId);
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult ValidateInvoice(
				[FromQuery] string invoiceId,
				[FromQuery] string code)
		{
			if (string.IsNullOrEmpty(invoiceId) || string.IsNullOrEmpty(code))
			{
				ModelState.AddModelError("", ResponseStatus.INVALID_REQUEST_PARAMETER);
				return BadRequest();
			}

			bool result = _invoicesService.ValidateInvoice(invoiceId, code);

			if (!result)
				return Problem(ResponseStatus.UPDATE_ERROR);

			return Ok(result.ToString());
		}
	}
}