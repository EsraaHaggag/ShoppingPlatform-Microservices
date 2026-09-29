using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Carts.Commands.AddCartItem;
using OrderService.Application.Features.Carts.Commands.ClearCart;
using OrderService.Application.Features.Carts.Commands.RemoveItem;
using OrderService.Application.Features.Carts.Commands.UpdateCartItemQuantity;
using OrderService.Application.Features.Carts.Queries.GetCartItems;

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
        public async Task<IActionResult> Create(
            AddCartItemCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetCartItemsQuery(),
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpPut("items/{productId:guid}")]
        public async Task<IActionResult> UpdateQuantity(Guid productId,
         int quantity, CancellationToken cancellationToken)
        {
            var command = new UpdateCartItemQuantityCommand(
                productId,
                quantity);

            var result = await _mediator.Send(
                command,
                cancellationToken);
            if (result.IsFailure)
                return HandleFailure(result);
            return Ok(result);
        }

        [HttpDelete("items/{productId:guid}")]
        public async Task<IActionResult> Remove(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveCartItemCommand(productId);
            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Clear(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ClearCartCommand(),
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }
    }
}
