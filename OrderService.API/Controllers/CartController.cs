using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Carts.Commands.AddCartItem;

namespace OrderService.API.Controllers
{
    public class CartController : ApiController
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCartItemCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }
    }
}
