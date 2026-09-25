using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Orders.Commands.CancelOrder;
using OrderService.Application.Features.Orders.Commands.Checkout;
using OrderService.Application.Features.Orders.Queries.GetMyOrders;
using OrderService.Application.Features.Orders.Queries.GetOrderByID;

namespace OrderService.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class OrdersController : ApiController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetOrderByIDQuery(id);

            var result = await _mediator.Send(
                query,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result.Value);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders(CancellationToken cancellationToken)
        {
            var query = new GetMyOrdersQuery();

            var result = await _mediator.Send(
                query,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result.Value);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
        {
            var command = new CheckoutCommand();

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new CancelOrderCommand(id);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return NoContent();
        }

    }
}


