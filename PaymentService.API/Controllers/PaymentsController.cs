using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Features.Payments.Commands.CreatePayment;
using PaymentService.Application.Features.Payments.Commands.ProcessPaymobWebhook;

namespace PaymentService.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ApiController
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result.Value);
        }


        //[AllowAnonymous]
        //[HttpPost("webhook")]
        //public IActionResult Webhook()
        //{
        //    return Ok();
        //}
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook(
        PaymobWebhookRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ProcessPaymobWebhookCommand(request),
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok();
        }
    }
}
