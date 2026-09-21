using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Products.Commands.CreateProduct;
using ProductService.Application.Features.Products.Commands.Deactivate;
using ProductService.Application.Features.Products.Commands.DecreaseStock;
using ProductService.Application.Features.Products.Commands.IncreaseStock;
using ProductService.Application.Features.Products.Commands.Reactivate;
using ProductService.Application.Features.Products.Commands.UpdatePrice;
using ProductService.Application.Features.Products.Queries.GetAllProduct;
using ProductService.Application.Features.Products.Queries.GetProductById;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery(id);

            var result = await _mediator.Send(
                query,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var query = new GetAllProductsQuery();

            var result = await _mediator.Send(
                query,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/price")]
        public async Task<IActionResult> UpdatePrice(
            Guid id,
            UpdatePriceCommand command,
            CancellationToken cancellationToken)
        {
            var updatedCommand = command with { Id = id };
            var result = await _mediator.Send(
                updatedCommand,
                cancellationToken);
            if (result.IsFailure)
                return HandleFailure(result);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/stock/decrease")]
        public async Task<IActionResult> DecreaseStock(
        Guid id,
        DecreaseStockCommand request,
        CancellationToken cancellationToken)
        {
            var command = new DecreaseStockCommand(id,
                request.quantity);
            var result = await _mediator.Send(
                command,
                cancellationToken);
            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/stock/increase")]
        public async Task<IActionResult> IncreaseStock(
            Guid id,
            IncreaseStockCommand request,
            CancellationToken cancellationToken)
        {
            var command = new IncreaseStockCommand(id,
                request.quantity);
            var result = await _mediator.Send(
                command,
                cancellationToken);
            if (result.IsFailure)
                return HandleFailure(result);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
        {
            var command = new DeactivateCommand(id);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/reactivate")]
        public async Task<IActionResult> Reactivate(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new ReactivateCommand(id);

            var result = await _mediator.Send(
                command,
                cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(result);
        }
    }
}
